using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Каталог учебные группы
    /// </summary>
    public class Catalog_УчебныеГруппы
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
        public Guid? Специальность_Key { get; set; }
        public short? ПорядковыйНомерНаКурсе { get; set; }
        public string ГодПоступления { get; set; }
        public Guid? Отделение_Key { get; set; }
        public Guid? РабочийУчебныйПлан_Key { get; set; }
        public Guid? Организация_Key { get; set; }
        public string Статус { get; set; }
        public string ФормаОбучения { get; set; }
        public string ОсновнойВидФинансирования { get; set; }
        public DateTime? ДатаОкончанияОбучения { get; set; }
        public Guid? Специализация_Key { get; set; }
        public Guid? ПрограммаСПО_Key { get; set; }

        //public List<CatalogУчебныеГруппыСеместры> Семестры { get; set; }
        //public List<CatalogУчебныеГруппыДополнительныеРеквизиты> ДополнительныеРеквизиты { get; set; }

        //// Навигационные свойства
        //public CatalogУчебныеГруппы Parent { get; set; }
        //public Speciality Специальность { get; set; }
        //public Department Отделение { get; set; }
        //public StudyPlan РабочийУчебныйПлан { get; set; }
        //public Organization Организация { get; set; }
        //public SPOProgram ПрограммаСПО { get; set; }
    }
}
