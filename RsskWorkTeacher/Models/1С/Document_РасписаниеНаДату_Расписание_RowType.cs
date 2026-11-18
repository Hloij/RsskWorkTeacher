using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Document_РасписаниеНаДату_Расписание_RowType
    {
        public Guid Ref_Key { get; set; }
        public long LineNumber { get; set; }
        public Guid? УчебнаяГруппа_Key { get; set; }
        public short? Подгруппа { get; set; }
        public short? НомерПары { get; set; }
        public string Дисциплина { get; set; }
        public Guid? Преподаватель_Key { get; set; }
        public Guid? Территория_Key { get; set; }
        public Guid? Аудитория_Key { get; set; }
        public Guid? ВидНагрузки_Key { get; set; }
        public short? Часов { get; set; }
        public string СсылкаНаПодключение { get; set; }
        public Guid? Поток_Key { get; set; }
        public string Дисциплина_Type { get; set; }
    }
}
