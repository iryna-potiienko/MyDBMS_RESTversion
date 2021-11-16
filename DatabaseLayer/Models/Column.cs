using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatabaseLayer.Models
{
    public class Column
    {
        public Column()
        {
            Values = new List<DataValue>();
        }
        
        [JsonIgnore]
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; set; }
        public Type Type { get; set; }
        public List<DataValue> Values { get; }
        
        [JsonIgnore]
        public int TableId { get; set; }
    }
}