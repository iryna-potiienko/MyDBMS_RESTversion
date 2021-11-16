using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatabaseLayer.Models
{
    public class Database
    {
        public Database()
        {
            Tables = new List<Table>();
        }
        
        [JsonIgnore]
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; set; }
        public List<Table> Tables { get; set; }
    }
}