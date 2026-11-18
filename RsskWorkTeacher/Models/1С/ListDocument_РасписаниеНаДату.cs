using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Список расписаний на дату
    /// </summary>
    public class ListDocument_РасписаниеНаДату
    {
        public string OdataMetadata { get; set; }
        public List<Document_РасписаниеНаДату> value { get; set; }
    }
}
