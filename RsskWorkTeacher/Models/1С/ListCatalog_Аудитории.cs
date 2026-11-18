using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Список аудитории
    /// </summary>
    public class ListCatalog_Аудитории
    {
        public string OdataMetadata { get; set; }
        public List<Catalog_Аудитории> value { get; set; }
    }
}
