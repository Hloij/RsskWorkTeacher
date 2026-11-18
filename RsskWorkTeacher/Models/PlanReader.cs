// PlxModels.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace PlxModels
{
    public static class PlxNs
    {
        public const string Diffgram = "urn:schemas-microsoft-com:xml-diffgram-v1";
        public const string MsData = "urn:schemas-microsoft-com:xml-msdata";
        public const string DsMMISDB = "http://tempuri.org/dsMMISDB.xsd";
    }

    internal static class PlxConvert
    {
        public static string ToXml(int? v) => v?.ToString(CultureInfo.InvariantCulture);
        public static string ToXml(decimal? v) => v?.ToString(CultureInfo.InvariantCulture);
        public static string ToXml(bool? v) => v?.ToString().ToLowerInvariant();
        public static string ToXml(DateTime? v) => v?.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture);

        public static int? ToInt(string v) => string.IsNullOrEmpty(v) ? null : int.Parse(v, CultureInfo.InvariantCulture);
        public static decimal? ToDecimal(string v) => string.IsNullOrEmpty(v) ? null : decimal.Parse(v, CultureInfo.InvariantCulture);
        public static bool? ToBool(string v) => string.IsNullOrEmpty(v) ? null : bool.Parse(v);
        public static DateTime? ToDateTime(string v) => string.IsNullOrEmpty(v) ? null : DateTime.Parse(v, CultureInfo.InvariantCulture);
    }

    [XmlRoot("Документ")]
    public class Document
    {
        [XmlAttribute("CrAppVer")] public string CrAppVer { get; set; }
        [XmlAttribute("CrWrite")] public string CrWrite { get; set; }
        [XmlAttribute("AppVersion")] public string AppVersion { get; set; }
        [XmlAttribute("MinAppVersion")] public string MinAppVersion { get; set; }
        [XmlAttribute("Тип")] public string Тип { get; set; }
        [XmlAttribute("PrevName")] public string PrevName { get; set; }
        [XmlAttribute("PrevWrite")] public string PrevWrite { get; set; }
        [XmlAttribute("LastName")] public string LastName { get; set; }
        [XmlAttribute("LastWrite")] public string LastWrite { get; set; }
        [XmlAttribute("WrPth")] public string WrPth { get; set; }
        [XmlAttribute("UserName")] public string UserName { get; set; }
        [XmlAttribute("UserPC")] public string UserPC { get; set; }
        [XmlAttribute("ЧислоКурсов")] public int ЧислоКурсов { get; set; }
        [XmlAttribute("СеместровНаКурсе")] public int СеместровНаКурсе { get; set; }
        [XmlAttribute("СессийНаКурсе")] public int СессийНаКурсе { get; set; }
        [XmlAttribute("ЭлементовВНеделе")] public int ЭлементовВНеделе { get; set; }
        [XmlAttribute("ТипГОСа")] public string ТипГОСа { get; set; }
        [XmlAttribute("КодУровняОбразования")] public int КодУровняОбразования { get; set; }
        [XmlAttribute("КодФормыОбучения")] public int КодФормыОбучения { get; set; }

        [XmlElement("diffgram", Namespace = PlxNs.Diffgram)]
        public Diffgram Diffgram { get; set; }
    }

    public class Diffgram
    {
        [XmlElement("dsMMISDB", Namespace = PlxNs.DsMMISDB)]
        public DsMMISDB Ds { get; set; }
    }

    public class DsMMISDB
    {
        [XmlElement("ООП")] public List<Oop> OopList { get; set; } = new();
        [XmlElement("ПланыКомпетенции")] public List<PlanCompetency> PlanCompetencies { get; set; } = new();
        [XmlElement("Компетенции")] public List<Competency> Competencies { get; set; } = new();
        [XmlElement("ПланыСтроки")] public List<PlanRow> PlanRows { get; set; } = new();
        [XmlElement("Планы")] public List<Plan> Plans { get; set; } = new();
        [XmlElement("ДолжЛица_Планы")] public List<PlanOfficial> PlanOfficials { get; set; } = new();
        [XmlElement("Заезды")] public List<Session> Sessions { get; set; } = new();
        [XmlElement("ПланыГрафикиЯчейки")] public List<ScheduleCell> ScheduleCells { get; set; } = new();
        [XmlElement("ПланыКомпетенцииДисциплины")] public List<DisciplineCompetency> DisciplineCompetencies { get; set; } = new();
        [XmlElement("ПланыНовыеГрафики")] public List<NewSchedule> NewSchedules { get; set; } = new();
        [XmlElement("ПланыНовыеЧасы")] public List<NewHour> NewHours { get; set; } = new();
        [XmlElement("ПланыРазбиения")] public List<Split> Splits { get; set; } = new();
        [XmlElement("ПланыЦиклы")] public List<Cycle> Cycles { get; set; } = new();
        [XmlElement("ПрограммаПодготовки")] public List<TrainingProgram> TrainingPrograms { get; set; } = new();
        [XmlElement("СправочникБазы")] public List<BaseDirectory> BaseDirectories { get; set; } = new();
        [XmlElement("СправочникВидОбъекта")] public List<ObjectKindDirectory> ObjectKindDirectories { get; set; } = new();
        [XmlElement("СправочникВидыПрактик")] public List<PracticeTypeDirectory> PracticeTypeDirectories { get; set; } = new();
        [XmlElement("СправочникТипаЧасов")] public List<HourTypeDirectory> HourTypeDirectories { get; set; } = new();
        [XmlElement("СправочникТипБлока")] public List<BlockTypeDirectory> BlockTypeDirectories { get; set; } = new();
        [XmlElement("СправочникТипОбъекта")] public List<ObjectTypeDirectory> ObjectTypeDirectories { get; set; } = new();
        [XmlElement("УровеньОбразования")] public List<EducationLevel> EducationLevels { get; set; } = new();
        [XmlElement("ФормаОбучения")] public List<StudyForm> StudyForms { get; set; } = new();
        [XmlElement("ВидыДеятельности")] public List<ActivityType> ActivityTypes { get; set; } = new();
        [XmlElement("ПланыВидыДеятельности")] public List<PlanActivityType> PlanActivityTypes { get; set; } = new();
        [XmlElement("Кафедры")] public List<Department> Departments { get; set; } = new();
        [XmlElement("Филиалы")] public List<Branch> Branches { get; set; } = new();
        [XmlElement("СправочникТипаРабот")] public List<WorkTypeDirectory> WorkTypeDirectories { get; set; } = new();
        [XmlElement("ВыбранныеВидыРабот")] public List<SelectedWorkType> SelectedWorkTypes { get; set; } = new();
        [XmlElement("СправочникВидыРабот")] public List<WorkTypeItem> WorkTypeItems { get; set; } = new();
        [XmlElement("ПараметрыПлана")] public List<PlanParameters> PlanParameters { get; set; } = new();
        [XmlElement("СкрытьВидыРабот")] public List<HiddenWorkType> HiddenWorkTypes { get; set; } = new();
        [XmlElement("ЗаездыНормы")] public List<SessionNorm> SessionNorms { get; set; } = new();
        [XmlElement("ДолжностныеЛица")] public List<Official> Officials { get; set; } = new();
        [XmlElement("Уровень_образования")] public List<EducationLevelRecord> EducationLevelRecords { get; set; } = new();
        [XmlElement("ПланыСтруктураОП")] public List<OpStructure> OpStructures { get; set; } = new();
        [XmlElement("ПланыВидыДеятельностиМодули")] public List<ModuleActivityType> ModuleActivityTypes { get; set; } = new();
        [XmlElement("госТребования")] public List<StateRequirement> StateRequirements { get; set; } = new();
    }

    public abstract class DiffRow
    {
        [XmlAttribute("id", Namespace = PlxNs.Diffgram)] public string DiffId { get; set; }
        [XmlAttribute("rowOrder", Namespace = PlxNs.MsData)] public int RowOrder { get; set; }
    }

    public class Oop : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Шифр")] public string Шифр { get; set; }
        [XmlAttribute("Название")] public string Название { get; set; }
        [XmlAttribute("УровеньОбразования")] public int УровеньОбразования { get; set; }
        [XmlAttribute("Квалификация")] public int Квалификация { get; set; }
        [XmlAttribute("НомерДокумента")] public string НомерДокумента { get; set; }
        [XmlAttribute("ТипГОСа")] public string ТипГОСа { get; set; }
        [XmlAttribute("Используется")] public bool Используется { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
        [XmlAttribute("Источник")] public int Источник { get; set; }
        [XmlAttribute("ПрограммаПодготовки")] public int ПрограммаПодготовки { get; set; }

        [XmlAttribute("ДатаДокумента")] public string ДатаДокументаXml { get => PlxConvert.ToXml(ДатаДокумента); set => ДатаДокумента = PlxConvert.ToDateTime(value); }
        [XmlIgnore] public DateTime? ДатаДокумента { get; set; }
        [XmlAttribute("Изменен")] public string ИзмененXml { get => PlxConvert.ToXml(Изменен); set => Изменен = PlxConvert.ToDateTime(value); }
        [XmlIgnore] public DateTime? Изменен { get; set; }
    }

    public class PlanCompetency : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодООП")] public int КодООП { get; set; }
        [XmlAttribute("ШифрКомпетенции")] public string ШифрКомпетенции { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("Номер")] public int Номер { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("Удалена")] public bool Удалена { get; set; }
        [XmlAttribute("КодКомпетенции")] public int КодКомпетенции { get; set; }
        [XmlAttribute("Тип")] public int Тип { get; set; }
        [XmlAttribute("Категория")] public string Категория { get; set; }
        [XmlAttribute("ПромежуточныйИндикатор")] public string ПромежуточныйИндикатор { get; set; }
        [XmlAttribute("КритерииОценивания")] public string КритерииОценивания { get; set; }

        [XmlAttribute("КодВидаДеятельности")] public string КодВидаДеятельностиXml { get => PlxConvert.ToXml(КодВидаДеятельности); set => КодВидаДеятельности = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КодВидаДеятельности { get; set; }
    }

    public class Competency : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодООП")] public int КодООП { get; set; }
        [XmlAttribute("ШифрКомпетенции")] public string ШифрКомпетенции { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("Номер")] public int Номер { get; set; }
        [XmlAttribute("Удалена")] public bool Удалена { get; set; }
        [XmlAttribute("Тип")] public int Тип { get; set; }
        [XmlAttribute("Категория")] public string Категория { get; set; }
        [XmlAttribute("ПромежуточныйИндикатор")] public string ПромежуточныйИндикатор { get; set; }
        [XmlAttribute("КритерииОценивания")] public string КритерииОценивания { get; set; }

        [XmlAttribute("КодВидаДеятельности")] public string КодВидаДеятельностиXml { get => PlxConvert.ToXml(КодВидаДеятельности); set => КодВидаДеятельности = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КодВидаДеятельности { get; set; }
    }

    public class PlanRow : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("КодООП")] public int КодООП { get; set; }
        [XmlAttribute("КодБлока")] public int КодБлока { get; set; }
        [XmlAttribute("Дисциплина")] public string Дисциплина { get; set; }
        [XmlAttribute("ТипОбъекта")] public int ТипОбъекта { get; set; }
        [XmlAttribute("ВидОбъекта")] public int ВидОбъекта { get; set; }
        [XmlAttribute("РассредПрактика")] public bool РассредПрактика { get; set; }
        [XmlAttribute("УровеньВложения")] public int УровеньВложения { get; set; }
        [XmlAttribute("ПризнакФизкультуры")] public bool ПризнакФизкультуры { get; set; }
        [XmlAttribute("СчитатьБезЗЕТ")] public bool СчитатьБезЗЕТ { get; set; }
        [XmlAttribute("СчитатьВПлане")] public bool СчитатьВПлане { get; set; }
        [XmlAttribute("НеСчитатьКонтроль")] public bool НеСчитатьКонтроль { get; set; }
        [XmlAttribute("ЗаСчетПолевых")] public bool ЗаСчетПолевых { get; set; }
        [XmlAttribute("ДисциплинаКод")] public string ДисциплинаКод { get; set; }
        [XmlAttribute("ЧасовПоЗЕТ")] public int ЧасовПоЗЕТ { get; set; }
        [XmlAttribute("ЧасовПоПлану")] public int ЧасовПоПлану { get; set; }
        [XmlAttribute("ПодлежитИзучениюЧасов")] public int ПодлежитИзучениюЧасов { get; set; }
        [XmlAttribute("НестандартНедПрактики")] public bool НестандартНедПрактики { get; set; }
        [XmlAttribute("ReadOnly")] public bool ReadOnly { get; set; }
        [XmlAttribute("Свернуть")] public bool Свернуть { get; set; }
        [XmlAttribute("ТипПерезачета")] public int ТипПерезачета { get; set; }
        [XmlAttribute("Адаптационная")] public bool Адаптационная { get; set; }
        [XmlAttribute("ЧасыВарМакс")] public int ЧасыВарМакс { get; set; }
        [XmlAttribute("ЧасыВарАуд")] public int ЧасыВарАуд { get; set; }
        [XmlAttribute("Номер")] public int Номер { get; set; }
        [XmlAttribute("Порядок")] public int Порядок { get; set; }
        [XmlAttribute("СкрытьВРПД")] public bool СкрытьВРПД { get; set; }
        [XmlAttribute("DVnotEquals")] public bool DVnotEquals { get; set; }
        [XmlAttribute("ПерезачтеноЧасовАуд")] public int ПерезачтеноЧасовАуд { get; set; }
        [XmlAttribute("NotCalcInSumKont")] public bool NotCalcInSumKont { get; set; }
        [XmlAttribute("ЦелиОсвоения")] public string ЦелиОсвоения { get; set; }
        [XmlAttribute("Факультатив")] public bool Факультатив { get; set; }
        [XmlAttribute("NotCalcInNagr")] public bool NotCalcInNagr { get; set; }
        [XmlAttribute("HideInTabPract")] public bool HideInTabPract { get; set; }
        [XmlAttribute("Дистанционная")] public bool Дистанционная { get; set; }
        [XmlAttribute("InForeignLanguage")] public bool InForeignLanguage { get; set; }
        [XmlAttribute("ПредметнаяОбласть")] public string ПредметнаяОбласть { get; set; }

        [XmlAttribute("Оценка")] public string ОценкаXml { get => PlxConvert.ToXml(Оценка); set => Оценка = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Оценка { get; set; }
        [XmlAttribute("Multiselect")] public string MultiselectXml { get => PlxConvert.ToXml(Multiselect); set => Multiselect = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Multiselect { get; set; }
        [XmlAttribute("КодКафедры")] public string КодКафедрыXml { get => PlxConvert.ToXml(КодКафедры); set => КодКафедры = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КодКафедры { get; set; }
        [XmlAttribute("КодРодителя")] public string КодРодителяXml { get => PlxConvert.ToXml(КодРодителя); set => КодРодителя = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КодРодителя { get; set; }
        [XmlAttribute("ВидПрактики")] public string ВидПрактикиXml { get => PlxConvert.ToXml(ВидПрактики); set => ВидПрактики = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? ВидПрактики { get; set; }
        [XmlAttribute("ЧасовВЗЕТ")] public string ЧасовВЗЕТXml { get => PlxConvert.ToXml(ЧасовВЗЕТ); set => ЧасовВЗЕТ = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? ЧасовВЗЕТ { get; set; }
    }

    public class Plan : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("УчебныйГод")] public string УчебныйГод { get; set; }
        [XmlAttribute("КодООП")] public int КодООП { get; set; }
        [XmlAttribute("КодФормыОбучения")] public int КодФормыОбучения { get; set; }
        [XmlAttribute("КодБазы")] public int КодБазы { get; set; }
        [XmlAttribute("КодОрганизации")] public int КодОрганизации { get; set; }
        [XmlAttribute("КодПодразделения")] public int КодПодразделения { get; set; }
        [XmlAttribute("КодПрофКафедры")] public int КодПрофКафедры { get; set; }
        [XmlAttribute("Сокращённое")] public bool Сокращённое { get; set; }
        [XmlAttribute("Дистанционное")] public bool Дистанционное { get; set; }
        [XmlAttribute("ДляИностранных")] public bool ДляИностранных { get; set; }
        [XmlAttribute("Титул")] public string Титул { get; set; }
        [XmlAttribute("Квалификация")] public string Квалификация { get; set; }
        [XmlAttribute("Специализация")] public string Специализация { get; set; }
        [XmlAttribute("ВоеннаяСпециальность")] public string ВоеннаяСпециальность { get; set; }
        [XmlAttribute("Предназначение")] public string Предназначение { get; set; }
        [XmlAttribute("СрокОбучения")] public int СрокОбучения { get; set; }
        [XmlAttribute("СрокОбученияМесяцев")] public int СрокОбученияМесяцев { get; set; }
        [XmlAttribute("СрокОбученияНедель")] public int СрокОбученияНедель { get; set; }
        [XmlAttribute("СрокОбученияДней")] public int СрокОбученияДней { get; set; }
        [XmlAttribute("ЧислоКурсов")] public int ЧислоКурсов { get; set; }
        [XmlAttribute("ЧислоСессий")] public int ЧислоСессий { get; set; }
        [XmlAttribute("КодГрафика")] public int КодГрафика { get; set; }
        [XmlAttribute("НомерФГОС")] public string НомерФГОС { get; set; }
        [XmlAttribute("НомерВерсииПриложения")] public string НомерВерсииПриложения { get; set; }
        [XmlAttribute("ИмяПриложения")] public string ИмяПриложения { get; set; }
        [XmlAttribute("НомерПользователя")] public string НомерПользователя { get; set; }
        [XmlAttribute("ИмяПользователя")] public string ИмяПользователя { get; set; }
        [XmlAttribute("ИмяФайла")] public string ИмяФайла { get; set; }
        [XmlAttribute("НомПротокСовета")] public string НомПротокСовета { get; set; }
        [XmlAttribute("Примечание")] public string Примечание { get; set; }
        [XmlAttribute("ГодНачалаПодготовки")] public int ГодНачалаПодготовки { get; set; }
        [XmlAttribute("ТипГОСа")] public string ТипГОСа { get; set; }
        [XmlAttribute("КодАктивногоООП")] public int КодАктивногоООП { get; set; }
        [XmlAttribute("ЗЕТвНеделю")] public decimal ЗЕТвНеделю { get; set; }
        [XmlAttribute("Точность")] public decimal Точность { get; set; }
        [XmlAttribute("ДвИГА")] public bool ДвИГА { get; set; }
        [XmlAttribute("ГвИГА")] public bool ГвИГА { get; set; }
        [XmlAttribute("СеместровНаКурсе")] public int СеместровНаКурсе { get; set; }
        [XmlAttribute("ЧасовВКредите")] public int ЧасовВКредите { get; set; }
        [XmlAttribute("Индивидуальный")] public int Индивидуальный { get; set; }
        [XmlAttribute("ФИО")] public string ФИО { get; set; }
        [XmlAttribute("Военные")] public bool Военные { get; set; }
        [XmlAttribute("КодУровняОбразования")] public int КодУровняОбразования { get; set; }
        [XmlAttribute("КодТипаПлана")] public int КодТипаПлана { get; set; }
        [XmlAttribute("СтатусВНагрузке")] public int СтатусВНагрузке { get; set; }
        [XmlAttribute("ИзученоКонтЧасов")] public int ИзученоКонтЧасов { get; set; }
        [XmlAttribute("СкрытьВРПД")] public bool СкрытьВРПД { get; set; }
        [XmlAttribute("СкрытьНаСайте")] public bool СкрытьНаСайте { get; set; }
        [XmlAttribute("MinAppVersion")] public string MinAppVersion { get; set; }
        [XmlAttribute("ПланОдобрен")] public string ПланОдобрен { get; set; }
        [XmlAttribute("СТреками")] public bool СТреками { get; set; }
        [XmlAttribute("ФамилияСтуд")] public string ФамилияСтуд { get; set; }
        [XmlAttribute("ИмяСтуд")] public string ИмяСтуд { get; set; }
        [XmlAttribute("ОтчествоСтуд")] public string ОтчествоСтуд { get; set; }
        [XmlAttribute("ПолСтудента")] public int ПолСтудента { get; set; }
        [XmlAttribute("НомерПротокКомиссии")] public string НомерПротокКомиссии { get; set; }
        [XmlAttribute("КурсНачалаОбуч")] public int КурсНачалаОбуч { get; set; }
        [XmlAttribute("СеместрНачалаОбуч")] public int СеместрНачалаОбуч { get; set; }
        [XmlAttribute("НомерПриказаСоздКомиссии")] public string НомерПриказаСоздКомиссии { get; set; }
        [XmlAttribute("НомПротокСоветаИзм")] public string НомПротокСоветаИзм { get; set; }
        [XmlAttribute("DOTType")] public int DOTType { get; set; }
        [XmlAttribute("SOOProfile")] public string SOOProfile { get; set; }
        [XmlAttribute("EduTypeName")] public string EduTypeName { get; set; }
        [XmlAttribute("GroupNameIUP")] public string GroupNameIUP { get; set; }
        [XmlAttribute("НомПротокКУГ")] public string НомПротокКУГ { get; set; }
        [XmlAttribute("НомПротокКУГИзм")] public string НомПротокКУГИзм { get; set; }
        [XmlAttribute("Профессионалитет")] public bool Профессионалитет { get; set; }

        [XmlAttribute("ДатаГОСа")] public string ДатаГОСаXml { get => PlxConvert.ToXml(ДатаГОСа); set => ДатаГОСа = PlxConvert.ToDateTime(value); }
        [XmlIgnore] public DateTime? ДатаГОСа { get; set; }
        [XmlAttribute("ДатаВерсииПриложения")] public string ДатаВерсииПриложенияXml { get => PlxConvert.ToXml(ДатаВерсииПриложения); set => ДатаВерсииПриложения = PlxConvert.ToDateTime(value); }
        [XmlIgnore] public DateTime? ДатаВерсииПриложения { get; set; }
        [XmlAttribute("ДатаУтверСоветом")] public string ДатаУтверСоветомXml { get => PlxConvert.ToXml(ДатаУтверСоветом); set => ДатаУтверСоветом = PlxConvert.ToDateTime(value); }
        [XmlIgnore] public DateTime? ДатаУтверСоветом { get; set; }
    }

    public class PlanOfficial : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("КодДолжЛица")] public int КодДолжЛица { get; set; }
        [XmlAttribute("Активный")] public bool Активный { get; set; }
        [XmlAttribute("Утвердил")] public bool Утвердил { get; set; }
        [XmlAttribute("КодООП")] public int КодООП { get; set; }
        [XmlAttribute("Тип")] public int Тип { get; set; }
        [XmlAttribute("НомерПриказа")] public string НомерПриказа { get; set; }
        [XmlAttribute("НомерПриказаИзм")] public string НомерПриказаИзм { get; set; }
        [XmlAttribute("НомерПриказаКУГ")] public string НомерПриказаКУГ { get; set; }
        [XmlAttribute("НомерПриказаКУГИзм")] public string НомерПриказаКУГИзм { get; set; }
        [XmlAttribute("Порядок")] public int Порядок { get; set; }
        [XmlAttribute("Дата")] public string ДатаXml { get => PlxConvert.ToXml(Дата); set => Дата = PlxConvert.ToDateTime(value); }
        [XmlIgnore] public DateTime? Дата { get; set; }
    }

    public class Session : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Курс")] public int Курс { get; set; }
        [XmlAttribute("Сессия")] public int Сессия { get; set; }
        [XmlAttribute("Название")] public string Название { get; set; }
        [XmlAttribute("Продолжительность")] public int Продолжительность { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
    }

    public class ScheduleCell : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодГрафика")] public int КодГрафика { get; set; }
        [XmlAttribute("КодВидаДеятельности")] public int КодВидаДеятельности { get; set; }
        [XmlAttribute("НомерНедели")] public int НомерНедели { get; set; }
        [XmlAttribute("НомерЧастиНедели")] public int НомерЧастиНедели { get; set; }
        [XmlAttribute("Курс")] public int Курс { get; set; }
        [XmlAttribute("Семестр")] public int Семестр { get; set; }
        [XmlAttribute("Переаттестована")] public bool Переаттестована { get; set; }
        [XmlAttribute("КоличествоЧастейВНеделе")] public int КоличествоЧастейВНеделе { get; set; }
        [XmlAttribute("НомерПервойНедели")] public int НомерПервойНедели { get; set; }
        [XmlAttribute("IsDist")] public bool IsDist { get; set; }
        [XmlAttribute("HideDiffPrMask")] public string HideDiffPrMask { get; set; }
        [XmlAttribute("DateElementGYP")] public string DateElementGYPXml { get => PlxConvert.ToXml(DateElementGYP); set => DateElementGYP = PlxConvert.ToDateTime(value); }
        [XmlIgnore] public DateTime? DateElementGYP { get; set; }
    }

    public class DisciplineCompetency : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодКомпетенции")] public int КодКомпетенции { get; set; }
        [XmlAttribute("КодСтроки")] public int КодСтроки { get; set; }
    }

    public class NewSchedule : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КоличествоЧастейВНеделе")] public int КоличествоЧастейВНеделе { get; set; }
        [XmlAttribute("HeaderMask")] public string HeaderMask { get; set; }
        [XmlAttribute("SymNames")] public string SymNames { get; set; }
    }

    public class NewHour : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодОбъекта")] public int КодОбъекта { get; set; }
        [XmlAttribute("КодВидаРаботы")] public int КодВидаРаботы { get; set; }
        [XmlAttribute("КодТипаЧасов")] public int КодТипаЧасов { get; set; }
        [XmlAttribute("Курс")] public int Курс { get; set; }
        [XmlAttribute("Семестр")] public int Семестр { get; set; }
        [XmlAttribute("Количество")] public decimal Количество { get; set; }
        [XmlAttribute("Переаттестовано")] public bool Переаттестовано { get; set; }
        [XmlAttribute("Сессия")] public string СессияXml { get => PlxConvert.ToXml(Сессия); set => Сессия = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Сессия { get; set; }
        [XmlAttribute("Недель")] public string НедельXml { get => PlxConvert.ToXml(Недель); set => Недель = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Недель { get; set; }
        [XmlAttribute("Дней")] public string ДнейXml { get => PlxConvert.ToXml(Дней); set => Дней = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Дней { get; set; }
        [XmlAttribute("ТипКомиссии")] public string ТипКомиссииXml { get => PlxConvert.ToXml(ТипКомиссии); set => ТипКомиссии = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? ТипКомиссии { get; set; }
    }

    public class Split : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПланыНовыеЧасы")] public int КодПланыНовыеЧасы { get; set; }
        [XmlAttribute("Суммировать")] public bool Суммировать { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("Преподаватель")] public string Преподаватель { get; set; }
        [XmlAttribute("НеДляВсех")] public bool НеДляВсех { get; set; }
        [XmlAttribute("Недель")] public string НедельXml { get => PlxConvert.ToXml(Недель); set => Недель = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Недель { get; set; }
        [XmlAttribute("Дней")] public string ДнейXml { get => PlxConvert.ToXml(Дней); set => Дней = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Дней { get; set; }
        [XmlAttribute("КоличествоСтудентов")] public string КоличествоСтудентовXml { get => PlxConvert.ToXml(КоличествоСтудентов); set => КоличествоСтудентов = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КоличествоСтудентов { get; set; }
        [XmlAttribute("НормативНаСтуд")] public string НормативНаСтудXml { get => PlxConvert.ToXml(НормативНаСтуд); set => НормативНаСтуд = PlxConvert.ToDecimal(value); }
        [XmlIgnore] public decimal? НормативНаСтуд { get; set; }
        [XmlAttribute("НормативНаСтудВНед")] public string НормативНаСтудВНедXml { get => PlxConvert.ToXml(НормативНаСтудВНед); set => НормативНаСтудВНед = PlxConvert.ToDecimal(value); }
        [XmlIgnore] public decimal? НормативНаСтудВНед { get; set; }
        [XmlAttribute("НормативНаПодгр")] public string НормативНаПодгрXml { get => PlxConvert.ToXml(НормативНаПодгр); set => НормативНаПодгр = PlxConvert.ToDecimal(value); }
        [XmlIgnore] public decimal? НормативНаПодгр { get; set; }
        [XmlAttribute("НормативНаПодгрВНед")] public string НормативНаПодгрВНедXml { get => PlxConvert.ToXml(НормативНаПодгрВНед); set => НормативНаПодгрВНед = PlxConvert.ToDecimal(value); }
        [XmlIgnore] public decimal? НормативНаПодгрВНед { get; set; }
        [XmlAttribute("НомерКомиссии")] public string НомерКомиссииXml { get => PlxConvert.ToXml(НомерКомиссии); set => НомерКомиссии = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? НомерКомиссии { get; set; }
    }

    public class Cycle : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("ТипБлока")] public int ТипБлока { get; set; }
        [XmlAttribute("ТипОбъекта")] public int ТипОбъекта { get; set; }
        [XmlAttribute("Факультативы")] public bool Факультативы { get; set; }
        [XmlAttribute("Номер")] public int Номер { get; set; }
        [XmlAttribute("Идентификатор")] public string Идентификатор { get; set; }
        [XmlAttribute("Цикл")] public string Цикл { get; set; }
        [XmlAttribute("ЗЕТмин")] public decimal ЗЕТмин { get; set; }
        [XmlAttribute("ЗЕТмакс")] public decimal ЗЕТмакс { get; set; }
        [XmlAttribute("Физкультура")] public bool Физкультура { get; set; }
        [XmlAttribute("СтарыйКод")] public int СтарыйКод { get; set; }
        [XmlAttribute("АудМин")] public decimal АудМин { get; set; }
        [XmlAttribute("АудМакс")] public decimal АудМакс { get; set; }
        [XmlAttribute("Проверка")] public int Проверка { get; set; }
        [XmlAttribute("КодРодителя")] public string КодРодителяXml { get => PlxConvert.ToXml(КодРодителя); set => КодРодителя = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КодРодителя { get; set; }
    }

    public class TrainingProgram : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
        [XmlAttribute("Уровень")] public int Уровень { get; set; }
    }

    public class BaseDirectory : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("Уровень")] public int Уровень { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
    }

    public class ObjectKindDirectory : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
    }

    public class PracticeTypeDirectory : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
    }

    public class HourTypeDirectory : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
    }

    public class BlockTypeDirectory : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Название")] public string Название { get; set; }
        [XmlAttribute("ИзООП")] public bool ИзООП { get; set; }
    }

    public class ObjectTypeDirectory : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Название")] public string Название { get; set; }
        [XmlAttribute("Составляющий")] public bool Составляющий { get; set; }
    }

    public class EducationLevel : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Уровень")] public string Уровень { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
    }

    public class StudyForm : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("ФормаОбучения")] public string ФормаОбучения { get; set; }
        [XmlAttribute("Сокращение")] public string Сокращение { get; set; }
        [XmlAttribute("Вывод")] public bool Вывод { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
    }

    public class ActivityType : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("КодООП")] public int КодООП { get; set; }
        [XmlAttribute("Тип")] public int Тип { get; set; }
        [XmlAttribute("НаименованиеПоПеречню")] public string НаименованиеПоПеречню { get; set; }
    }

    public class PlanActivityType : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("КодВидаДеятельности")] public int КодВидаДеятельности { get; set; }
        [XmlAttribute("КодООП")] public int КодООП { get; set; }
        [XmlAttribute("Основной")] public bool Основной { get; set; }
    }

    public class Department : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Название")] public string Название { get; set; }
        [XmlAttribute("Сокращение")] public string Сокращение { get; set; }
        [XmlAttribute("Номер")] public int Номер { get; set; }
        [XmlAttribute("ЗавКафедрой")] public string ЗавКафедрой { get; set; }
        [XmlAttribute("Аудитория")] public string Аудитория { get; set; }
        [XmlAttribute("Телефон")] public string Телефон { get; set; }
        [XmlAttribute("ВнутрТелефон")] public string ВнутрТелефон { get; set; }
        [XmlAttribute("EMail")] public string EMail { get; set; }
        [XmlAttribute("URL")] public string URL { get; set; }
        [XmlAttribute("Удалена")] public bool Удалена { get; set; }
        [XmlAttribute("ПрефиксКафедры")] public string ПрефиксКафедры { get; set; }
        [XmlAttribute("ЦМК")] public bool ЦМК { get; set; }
        [XmlAttribute("Примечание")] public string Примечание { get; set; }
        [XmlAttribute("Выпускающая")] public bool Выпускающая { get; set; }
    }

    public class Branch : DiffRow
    {
        [XmlAttribute("Код_филиала")] public int Код_филиала { get; set; }
        [XmlAttribute("Полное_название")] public string Полное_название { get; set; }
        [XmlAttribute("Краткое_Название")] public string Краткое_Название { get; set; }
        [XmlAttribute("Головное_учреждение")] public bool Головное_учреждение { get; set; }
        [XmlAttribute("Директор")] public string Директор { get; set; }
        [XmlAttribute("КодРодителя")] public string КодРодителяXml { get => PlxConvert.ToXml(КодРодителя); set => КодРодителя = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КодРодителя { get; set; }
    }

    public class WorkTypeDirectory : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Название")] public string Название { get; set; }
        [XmlAttribute("Отображать")] public bool Отображать { get; set; }
    }

    public class SelectedWorkType : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("КодВидаРабот")] public int КодВидаРабот { get; set; }
    }

    public class WorkTypeItem : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("Название")] public string Название { get; set; }
        [XmlAttribute("Аббревиатура")] public string Аббревиатура { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
        [XmlAttribute("Caption")] public string Caption { get; set; }
        [XmlAttribute("КодТипРабот")] public string КодТипРаботXml { get => PlxConvert.ToXml(КодТипРабот); set => КодТипРабот = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? КодТипРабот { get; set; }
        [XmlAttribute("Контактный")] public string КонтактныйXml { get => PlxConvert.ToXml(Контактный); set => Контактный = PlxConvert.ToBool(value); }
        [XmlIgnore] public bool? Контактный { get; set; }
        [XmlAttribute("Аудиторный")] public string АудиторныйXml { get => PlxConvert.ToXml(Аудиторный); set => Аудиторный = PlxConvert.ToBool(value); }
        [XmlIgnore] public bool? Аудиторный { get; set; }
        [XmlAttribute("Отображать")] public string ОтображатьXml { get => PlxConvert.ToXml(Отображать); set => Отображать = PlxConvert.ToBool(value); }
        [XmlIgnore] public bool? Отображать { get; set; }
        [XmlAttribute("HasInter")] public string HasInterXml { get => PlxConvert.ToXml(HasInter); set => HasInter = PlxConvert.ToBool(value); }
        [XmlIgnore] public bool? HasInter { get; set; }
        [XmlAttribute("HasDistr")] public string HasDistrXml { get => PlxConvert.ToXml(HasDistr); set => HasDistr = PlxConvert.ToBool(value); }
        [XmlIgnore] public bool? HasDistr { get; set; }
        [XmlAttribute("Назначение")] public string НазначениеXml { get => PlxConvert.ToXml(Назначение); set => Назначение = PlxConvert.ToInt(value); }
        [XmlIgnore] public int? Назначение { get; set; }
        [XmlAttribute("HasPrPreparing")] public string HasPrPreparingXml { get => PlxConvert.ToXml(HasPrPreparing); set => HasPrPreparing = PlxConvert.ToBool(value); }
        [XmlIgnore] public bool? HasPrPreparing { get; set; }
    }

    public class PlanParameters : DiffRow
    {
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("count")] public int count { get; set; }
        [XmlAttribute("HinW54")] public string HinW54 { get; set; }
        [XmlAttribute("EqZET36")] public string EqZET36 { get; set; }
        [XmlAttribute("ShowIndex")] public bool ShowIndex { get; set; }
        [XmlAttribute("ShowCmpt")] public bool ShowCmpt { get; set; }
        [XmlAttribute("ShowKafCode")] public bool ShowKafCode { get; set; }
        [XmlAttribute("ShowKafName")] public bool ShowKafName { get; set; }
        [XmlAttribute("TypeHours")] public int TypeHours { get; set; }
        [XmlAttribute("RUPHeader")] public string RUPHeader { get; set; }
        [XmlAttribute("RUPMinistry")] public string RUPMinistry { get; set; }
        [XmlAttribute("ProfilHeader")] public string ProfilHeader { get; set; }
        [XmlAttribute("PeriodName")] public string PeriodName { get; set; }
        [XmlAttribute("PeriodAbbr")] public string PeriodAbbr { get; set; }
        [XmlAnyAttribute] public XmlAttribute[] ExtraAttributes { get; set; }
    }

    public class HiddenWorkType : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("КодВидаРабот")] public int КодВидаРабот { get; set; }
        [XmlAttribute("Itog")] public bool Itog { get; set; }
    }

    public class SessionNorm : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("Курс")] public int Курс { get; set; }
        [XmlAttribute("Дней")] public int Дней { get; set; }
        [XmlAttribute("Дорога")] public int Дорога { get; set; }
    }

    public class Official : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("ФИО")] public string ФИО { get; set; }
        [XmlAttribute("Должность")] public string Должность { get; set; }
        [XmlAttribute("Звание")] public string Звание { get; set; }
        [XmlAttribute("EMail")] public string EMail { get; set; }
        [XmlAttribute("КодОрганизации")] public int КодОрганизации { get; set; }
        [XmlAttribute("Url")] public string Url { get; set; }
    }

    public class EducationLevelRecord : DiffRow
    {
        [XmlAttribute("Код_записи")] public int Код_записи { get; set; }
        [XmlAttribute("Уровень")] public string Уровень { get; set; }
        [XmlAttribute("ВидПлана")] public string ВидПлана { get; set; }
        [XmlAttribute("Категория")] public string Категория { get; set; }
        [XmlAttribute("Вывод")] public bool Вывод { get; set; }
        [XmlAttribute("Префикс")] public string Префикс { get; set; }
        [XmlAttribute("Примечание")] public string Примечание { get; set; }
        [XmlAttribute("Шифр")] public string Шифр { get; set; }
    }

    public class OpStructure : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодПлана")] public int КодПлана { get; set; }
        [XmlAttribute("Наименование")] public string Наименование { get; set; }
        [XmlAttribute("MinH")] public int MinH { get; set; }
        [XmlAttribute("MaxH")] public int MaxH { get; set; }
        [XmlAttribute("Type")] public int Type { get; set; }
        [XmlAttribute("GroupId")] public int GroupId { get; set; }
        [XmlAttribute("HoursInRUP")] public int HoursInRUP { get; set; }
        [XmlAttribute("HoursInRUPExpert")] public int HoursInRUPExpert { get; set; }
        [XmlAttribute("HoursInRUPBaseH")] public int HoursInRUPBaseH { get; set; }
    }

    public class ModuleActivityType : DiffRow
    {
        [XmlAttribute("Код")] public int Код { get; set; }
        [XmlAttribute("КодСтроки")] public int КодСтроки { get; set; }
        [XmlAttribute("КодВидаДеятельности")] public int КодВидаДеятельности { get; set; }
    }

    public class StateRequirement : DiffRow
    {
        [XmlAttribute("КодТребования")] public int КодТребования { get; set; }
        [XmlAttribute("НаименованиеТребования")] public string НаименованиеТребования { get; set; }
        [XmlAttribute("НомерТребования")] public int НомерТребования { get; set; }
    }

    // ====================================================================
    // Хелперы
    // ====================================================================

    public static class PlxQuery
    {
        public static Dictionary<int, List<PlanCompetency>> BuildDisciplineCompetencyMap(DsMMISDB ds)
        {
            if (ds == null) throw new ArgumentNullException(nameof(ds));

            var planCompById = new Dictionary<int, PlanCompetency>(ds.PlanCompetencies.Count);
            foreach (var pc in ds.PlanCompetencies)
                planCompById[pc.Код] = pc;

            var result = new Dictionary<int, List<PlanCompetency>>();

            foreach (var link in ds.DisciplineCompetencies)
            {
                if (!planCompById.TryGetValue(link.КодКомпетенции, out var pc))
                    continue;

                if (!result.TryGetValue(link.КодСтроки, out var list))
                {
                    list = new List<PlanCompetency>();
                    result[link.КодСтроки] = list;
                }
                if (!list.Contains(pc))
                    list.Add(pc);
            }

            foreach (var list in result.Values)
                list.Sort((a, b) => a.Номер.CompareTo(b.Номер));

            return result;
        }

        public static List<PlanCompetency> GetCompetenciesForRow(DsMMISDB ds, int rowCode)
        {
            var map = BuildDisciplineCompetencyMap(ds);
            return map.TryGetValue(rowCode, out var list) ? list : new List<PlanCompetency>();
        }

        public static Competency? GetGlobal(DsMMISDB ds, PlanCompetency pc)
        {
            if (ds == null || pc == null) return null;
            return ds.Competencies.FirstOrDefault(c => c.Код == pc.КодКомпетенции);
        }

        public static string ShortCode(PlanCompetency pc)
            => pc == null ? "" : pc.ШифрКомпетенции.TrimEnd('.', ' ');
    }

    /// <summary>
    /// Результат расчёта часов для дисциплины/МДК.
    /// </summary>
    public class DisciplineHours
    {
        public int Total { get; set; }
        public int Theory { get; set; }
        public int Practice { get; set; }
        public int Independent { get; set; }
        public int Control { get; set; }
        public int PracticalPreparation { get; set; }
    }

    /// <summary>
    /// Калькулятор часов из учебного плана.
    /// </summary>
    public static class PlxHourCalculator
    {
        public static DisciplineHours Calculate(DsMMISDB ds, int rowCode)
        {
            var row = ds.PlanRows.FirstOrDefault(r => r.Код == rowCode);
            if (row == null) return new DisciplineHours();

            var hours = ds.NewHours.Where(h => h.КодОбъекта == rowCode).ToList();

            decimal Sum(int workType) => hours
                .Where(h => h.КодВидаРаботы == workType)
                .Sum(h => h.Количество);

            decimal SumType(int hourType) => hours
                .Where(h => h.КодТипаЧасов == hourType)
                .Sum(h => h.Количество);

            // Общий объём (1000) или из строки плана
            int total = row.ЧасовПоПлану;
            if (total == 0)
            {
                total = (int)Sum(1000);
            }

            // Теория (101)
            int theory = (int)Sum(101);

            // Практика (102 - лабы, 103 - практические, 140 - инд. проект)
            int lab = (int)Sum(102);
            int practice = (int)Sum(103);
            int individualProject = (int)Sum(140);
            int totalPractice = lab + practice + individualProject;

            // Самостоятельная (107)
            int independent = (int)Sum(107);

            // Контроль (108)
            int control = (int)Sum(108);

            // Практическая подготовка (КодТипаЧасов = 5)
            int practicalPrep = (int)SumType(5);

            return new DisciplineHours
            {
                Total = total,
                Theory = theory,
                Practice = totalPractice,
                Independent = independent,
                Control = control,
                PracticalPreparation = practicalPrep
            };
        }
    }
}