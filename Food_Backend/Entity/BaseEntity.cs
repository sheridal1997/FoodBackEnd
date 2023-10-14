using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Food_Backend.Entity
{
    public abstract class BaseEnntity<TKey> : IBaseEntity<TKey>
    {

        [NotMapped]
        object IBaseEntity.Id
        {
            get => this.Id;
            set => this.Id = (TKey)value;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
        public string? CreatedUserId { get; set; } = null!;
        public DateTime? CreateTime { get; set; }
        public string? EditUserId { get; set; } = null!;
        public DateTime? EditTime { get; set; }
        [JsonIgnore]
        public bool IsDelete { get; set; }  
    }
    public interface IBaseEntity<TKey> : IBaseEntity
    {
        TKey Id { get; set; }
    }
    public interface IBaseEntity
    {
        object Id { get; set; }
        string? CreatedUserId { get; set; }
        DateTime? CreateTime { get; set; }
        string? EditUserId { get; set; }
        DateTime? EditTime { get; set; }
        bool IsDelete { get; set; }
    }
}
