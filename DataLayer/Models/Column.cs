using System.Collections.Generic;

namespace DataLayer.Models
{
    public class Column
    {
        public Column()
        {
            Values = new List<string>();
            //Values = new Dictionary<int, string>();
        }
        public string Name { get; set; }
        public Type Type { get; set; }
        public List<string> Values { get; set; }
        //public Dictionary<int, string> Values { get; set; }
    }
}