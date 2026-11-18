
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    /// <summary>
    /// Документ Шаблон расписания
    /// Общая информация об элементе расписания
    /// </summary>
    public class Document_РасписаниеНаДату
    {
        public Guid Ref_Key { get; set; }
        public string DataVersion { get; set; }
        public string Number { get; set; }
        public DateTime? Date { get; set; }
        public bool? DeletionMark { get; set; }
        public bool? Posted { get; set; }
        public Guid? Организация_Key { get; set; }
        public DateTime? ДатаРасписания { get; set; }
        public Guid? Отделение_Key { get; set; }
        public string ДеньНедели { get; set; }
        public string ВидНедели { get; set; }
        public short? НомерНедели { get; set; }
        public Guid? РасписаниеЗвонков_Key { get; set; }
        public Guid? Ответственный_Key { get; set; }
        public Guid? Автор_Key { get; set; }
        public string Комментарий { get; set; }
        public Guid? ПериодОбучения_Key { get; set; }
        public string ФормаОбучения { get; set; }

        public virtual ICollection<Document_РасписаниеНаДату_УчебныеГруппы_RowType> УчебныеГруппы { get; set; }
        public virtual ICollection<Document_РасписаниеНаДату_Расписание_RowType> Расписание { get; set; }
        public virtual ICollection<Document_РасписаниеНаДату_Преподаватели_RowType> Преподаватели { get; set; }


        //public List<РасписаниеПреподавателя> РасписаниеПреподавателя { get; set; }
       
        
        //// Навигационные свойства
        public string Организация { get; set; }
        public string ПериодОбучения { get; set; }
        public string РасписаниеЗвонков { get; set; }
        public string Отделение { get; set; }
        public string Ответственный { get; set; }
        public string Автор { get; set; }




    }






}
