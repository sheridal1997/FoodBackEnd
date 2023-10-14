

using System.Text;

namespace Food_Backend.Controllers
{
    public class General 
    {
        public static string secretKey = "gDP1H21qMa";
        private readonly HttpContext _contex;
        private readonly IConfiguration _configuration;

        public General(HttpContext contex, IConfiguration configuration)
        {
            _contex = contex;
            _configuration = configuration;
        }

        public static string EnccyptText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text + secretKey;
                byte[] hashText = Encoding.UTF8.GetBytes(text);
                return Convert.ToBase64String(hashText);
            }
            else
            {
                return "";
            }
        }
        public static string DecryotText(string enccyptText)
        {
            if (!string.IsNullOrEmpty(enccyptText))
            {
                byte[] encryptByte = Convert.FromBase64String(enccyptText);
                string actualText = Encoding.UTF8.GetString(encryptByte);
                actualText = actualText.Substring(0, actualText.Length - secretKey.Length);

                return actualText;
            }
            else
            {
                return "";
            }
        }
        
        
    }
   
    public class SearchParams
    {
        public string SearchType { get; set; }
        public string SearchParam { get; set; }
    }
}

