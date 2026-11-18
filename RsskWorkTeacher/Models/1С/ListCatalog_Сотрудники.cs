using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Список сотрудников (тут же и преподаватели)
    /// </summary>
    public class ListCatalog_Сотрудники
    {
        public string OdataMetadata { get; set; }
        public List<Catalog_Сотрудники> value { get; set; }
    }
}
