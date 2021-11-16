using System.Collections.Generic;

namespace DataLayer.Models
{
    public class Table
    {
        public Table()
        {
            Columns = new List<Column>();
        }
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        
    }
}