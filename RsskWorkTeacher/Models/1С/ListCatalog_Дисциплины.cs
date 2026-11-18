using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Список дисциплин
    /// </summary>
    public class ListCatalog_Дисциплины
    {
        public string OdataMetadata { get; set; }
        public List<Catalog_Дисциплины> value { get; set; }
    }
}
