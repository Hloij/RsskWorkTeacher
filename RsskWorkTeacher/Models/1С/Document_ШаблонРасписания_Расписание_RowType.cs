using System;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace ConsoleApp2
{
    /// <summary>
    /// Шаблон расписания строки расписания
    /// </summary>
    public class Document_ШаблонРасписания_Расписание_RowType
    {
        public Guid Ref_Key { get; set; }
        public Int64 LineNumber { get; set; }
        public Guid? УчебнаяГруппа_Key { get; set; }
        public string ДеньНедели { get; set; }
        public string ВидНедели { get; set; }
        public Int16 Подгруппа { get; set; }
        public Int16 НомерПары { get; set; }
        public string Дисциплина { get; set; }
        public Guid? Преподаватель_Key { get; set; }
        public Guid? Территория_Key { get; set; }
        public Guid? Аудитория_Key { get; set; }
        public Int16 НомерПотока { get; set; }
        public Guid? ВидНагрузки_Key { get; set; }
        public Guid? Поток_Key { get; set; }
        public string Дисциплина_Type { get; set; }

    }




}
