using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Catalog_Сотрудники
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
        public Guid? Физлицо_Key { get; set; }
        public string ТипЗанятости { get; set; }
        public bool? Преподаватель { get; set; }
        public Guid? Организация_Key { get; set; }
        public bool? Мастер { get; set; }
        public DateTime? ДатаПредоставленияСправкиОбОтсутствииСудимости { get; set; }
        public string НомерСанитарнойКнижки { get; set; }
        public string ВидОбразованияСотрудника { get; set; }
        public Guid? ОсновнаяТерритория_Key { get; set; }
        public Guid? ОсновнаяАудитория_Key { get; set; }
        public byte[] Факсимиле_Base64Data { get; set; }
        public bool? СтавитьПечать { get; set; }
        public string СсылкаДистанционногоПодключения { get; set; }

        //public List<ПреподаваемыеДисциплины> ПреподаваемыеДисциплины { get; set; }
        //public List<УченыеСтепени> УченыеСтепени { get; set; }
        //public List<УченыеЗвания> УченыеЗвания { get; set; }
        //public List<ДополнительныеРеквизиты> ДополнительныеРеквизиты { get; set; }
        //public List<РуководствоПрактиками> РуководствоПрактиками { get; set; }
        //public string Факсимиле_Type { get; set; }
        //public Stream Факсимиле { get; set; }

        //// Навигационные свойства
        //public CatalogСотрудники Parent { get; set; }
        //public Физлицо Физлицо { get; set; }
        //public Организация Организация { get; set; }
        //public ОсновнаяТерритория ОсновнаяТерритория { get; set; }
        //public ОсновнаяАудитория ОсновнаяАудитория { get; set; }
    }
}
