using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Каталог дисциплины
    /// </summary>
    public class Catalog_Дисциплины
    {
        public Guid Ref_Key { get; set; }
        public bool? Predefined { get; set; }
        public string PredefinedDataName { get; set; }
        public string DataVersion { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid? Parent_Key { get; set; }
        public bool? IsFolder { get; set; }
        public bool? DeletionMark { get; set; }
        public string СокращенноеНаименование { get; set; }
        public string ПолноеНаименование { get; set; }
        public bool? ПроводитьВПодгруппах { get; set; }
        public Guid? ОсновнаяАудитория_Key { get; set; }
        public Guid? ОсновнойПреподаватель_Key { get; set; }

        //public List<CatalogДисциплиныПодходящиеАудитории> ПодходящиеАудитории { get; set; }
        //public List<CatalogДисциплиныДополнительныеРеквизиты> ДополнительныеРеквизиты { get; set; }

        //// Навигационные свойства
        //public CatalogДисциплины Parent { get; set; }
        //public Auditorium ОсновнаяАудитория { get; set; }
        //public Teacher ОсновнойПреподаватель { get; set; }
    }
}
