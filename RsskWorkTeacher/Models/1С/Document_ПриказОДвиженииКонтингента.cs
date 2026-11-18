using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagruzka.models
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class RootDocument_ПриказОДвиженииКонтингента
    {
        [JsonProperty("odata.metadata")]
        public string odatametadata { get; set; }
        public List<Document_ПриказОДвиженииКонтингента> value { get; set; }
    }

    public class Document_ПриказОДвиженииКонтингента
    {
        public string Ref_Key { get; set; }
        public string DataVersion { get; set; }
        public bool DeletionMark { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public bool Posted { get; set; }
        public string ВидПриказа { get; set; }
        public string Организация_Key { get; set; }
        public string УчебнаяГруппа_Key { get; set; }
        public bool Списком { get; set; }
        public string Формулировка { get; set; }
        public string Комментарий { get; set; }
        public string УчебнаяГруппаНовая_Key { get; set; }
        public int Курс { get; set; }
        public string Ответственный_Key { get; set; }
        public string Заголовок { get; set; }
        public string Основание { get; set; }
        public string Автор_Key { get; set; }
        public string Отделение_Key { get; set; }
        public string РегистрационныйНомер_Key { get; set; }
        public string НовоеНазваниеГруппы { get; set; }
        public string Причина { get; set; }
        public string ПриемнаяКампания_Key { get; set; }
        public string ДокументОснование_Key { get; set; }
        public string АналитикаПричины_Key { get; set; }
        public bool ВАкадемическийОтпуск { get; set; }
        public string ПродолжениеФормулировки { get; set; }
        public bool НеПрошёлИГА { get; set; }
        public string ОбразовательнаяОрганизация_Key { get; set; }
        public bool ПодписанЭП { get; set; }
        public string ФормаОбученияДоПеревода { get; set; }
        public string БазаОсвоенияПрограммыДоПеревода { get; set; }
        public string Протокол_Key { get; set; }
        public List<СписокСтудентов> СписокСтудентов { get; set; }
        public List<object> Подписи { get; set; }
        public List<object> Согласования
        { get; set; }
        public List<object> Ознакомление
        { get; set; }

        [JsonProperty("Организация@navigationLinkUrl")]
        public string navigationLinkUrl { get; set; }

        [JsonProperty("УчебнаяГруппа@navigationLinkUrl")]
        public string navigationLinkUrl1 { get; set; }

        [JsonProperty("Автор@navigationLinkUrl")]
        public string navigationLinkUrl2 { get; set; }

        [JsonProperty("Отделение@navigationLinkUrl")]
        public string navigationLinkUrl3 { get; set; }
    }

    public class СписокСтудентов
    {
        public string Ref_Key { get; set; }
        public string LineNumber { get; set; }
        public string Студент_Key { get; set; }
        public string ФизЛицо_Key { get; set; }
        public string ВидФинансирования { get; set; }
        public string АнкетаАбитуриента_Key { get; set; }
        public string НомерПриказаОЗачислении { get; set; }
        public DateTime ДатаПриказаОЗачислении { get; set; }
        public string НомерПриказаОбАкадемическомОтпуске { get; set; }
        public DateTime ДатаПриказаОбАкадемическомОтпуске { get; set; }
        public DateTime Срок { get; set; }
        public int Курс { get; set; }
        public string ВидПеревода { get; set; }
        public string ПриказОЗачислении_Key { get; set; }
        public string ВидБюджета { get; set; }
        public DateTime ДатаНачалаДействияПриказа { get; set; }
    }


}
