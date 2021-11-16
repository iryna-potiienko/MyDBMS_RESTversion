using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatabaseLayer.Models
{
    public class DataValue
    {
        [JsonIgnore]
        [Key]
        public int Id { get; private set; }
        public string Data { get; set; }
        [Required]
        public int RowNumber { get; set; }
        [JsonIgnore]
        public int ColumnId { get; set; }
    }
}