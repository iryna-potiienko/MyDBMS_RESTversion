using System.Collections.Generic;

namespace MVCWebApp.Models
{
    public class TableProjectionViewModel
    {
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public string ColumnsNames { get; set; }
    }
}