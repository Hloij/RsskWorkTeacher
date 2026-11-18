using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Список учебных групп
    /// </summary>
    public class ListCatalog_УчебныеГруппы
    {
        public string OdataMetadata { get; set; }
        public List<Catalog_УчебныеГруппы> value { get; set; }
    }
}
