using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatabaseLayer.Models
{
    public class Table
    {
        public Table()
        {
            Columns = new List<Column>();
        }
        [JsonIgnore]
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; set; }
        public List<Column> Columns { get; }
        public int RowsNumber { get; set; }
        
        [JsonIgnore]
        public int DatabaseId { get; set; }
    }
}