using Food_Backend.Controllers;
using Food_Backend.Entity;
using Food_Backend.Middleware;
using Food_Backend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Food_Backend.Service
{
    
    public class UserService :IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public List<User> Get()
        {
           var response = (_userRepository.FindAll()).ToList();

            return response;
        }
        public async Task<User> Get(int id)
        {
            var response = await _userRepository.FindAsync(id);
            return response;
        }
        public async Task<User> Create(User model)
        {
            if(model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            if (string.IsNullOrEmpty(model.Username))
            {
                throw new ServiceException("UserName Required");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ServiceException("password Required");
            }
            var userExist = await _userRepository.Exist(o => o.Username== model.Username);
            if (userExist)
            {
                throw new ServiceException("UserName Already Exist");
            }
            model.Password = General.EnccyptText(model.Password);
            if (model.UserType == null || model.UserType ==0)
            {
                model.UserType = (int)UserType.User;
            }

            var user = await _userRepository.CreateAsync(model);

            return user;
            
        }
        public async Task<User> Update(User model)
        {

            if (model == null)
            {
                throw new ServiceException("Required Param Missing");
            }
            if (string.IsNullOrEmpty(model.Username))
            {
                throw new ServiceException("UserName Required");
            }
            var userExist = await _userRepository.Exist(o => o.Username == model.Username);
            if (userExist)
            {
                throw new ServiceException("UserName Already Exist");
            }
            await _userRepository.UpdateAsync(model);

            return model;
        }
        public async Task<bool> Delete(int id)
        {
           var user = await _userRepository.FindAsync(id);
            if(user == null)
            {
                throw new ServiceException("User Not Found");
            }
            _userRepository.Delete(user);
            return true;
        }

        public async Task<LoginInfo> LogIn(Login login)
        {
            var password = General.EnccyptText(login.Password);

            var User = await _userRepository.FindByCondition(o => o.Username == login.Username && o.Password == password).FirstOrDefaultAsync();

            if (User == null)
            {
                throw new ServiceException($"Invalid username or password");
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JwtKey"]);

                var tokenDiscription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, User.Username),
                        new Claim("UserId", User.Id.ToString()),
                        new Claim("UserType", User.UserType.ToString()),

                    }),
                    Expires = DateTime.Now.AddDays(3),
                    Issuer = _configuration["ApplicationSettings:Issuer"],
                    Audience = _configuration["ApplicationSettings:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDiscription);
                User.Password = null;


                return new LoginInfo { User = User, Token = tokenHandler.WriteToken(token) };
            }

        }
       
    }
    public interface IUserService 
    {
        Task<User> Create(User model);
        List<User> Get();
        Task<User> Get(int id);
        Task<User> Update(User model);
        Task<bool> Delete(int id);
        Task<LoginInfo> LogIn(Login login);

    }
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginInfo
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
