using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;                       // ← добавлено для ContentBlocks.Select
using System.Runtime.CompilerServices;
using System.Text;

namespace RsskWorkTeacher.Models.RP
{
    public class EvaluationCriteria
    {
        public string LearningOutcome { get; set; }   // Результаты обучения
        public string Indicators { get; set; }        // Показатели освоенности компетенций
        public string AssessmentMethod { get; set; }  // Методы оценки
    }

    public class CompetenceSelection : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Для таблицы разработчиков
    public class Developer
    {
        public string FullName { get; set; }
        public string Position { get; set; }
    }

    // Для таблицы компетенций
    public class Competency : INotifyPropertyChanged
    {
        private string _code;
        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(); }
        }

        private string _knowHow;
        public string KnowHow
        {
            get => _knowHow;
            set { _knowHow = value; OnPropertyChanged(); }
        }

        private string _knowledge;
        public string Knowledge
        {
            get => _knowledge;
            set { _knowledge = value; OnPropertyChanged(); }
        }
        private string _skil;
        public string Skil
        {
            get => _skil;
            set { _skil = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Для тематического плана
    public class ContentBlock
    {
        public string Description { get; set; }
        public string Hours { get; set; }

        public ContentBlock Clone() => new ContentBlock { Description = this.Description, Hours = this.Hours };
    }

    public class ThemePlan
    {
        public string ThemeName { get; set; }
        public string TotalHours { get; set; }
        public string Competences { get; set; }
        public bool IsSection { get; set; } = false;

        public ObservableCollection<ContentBlock> ContentBlocks { get; set; } = new ObservableCollection<ContentBlock>();

        public ThemePlan Clone()
        {
            return new ThemePlan
            {
                ThemeName = this.ThemeName,
                TotalHours = this.TotalHours,
                Competences = this.Competences,
                ContentBlocks = new ObservableCollection<ContentBlock>(this.ContentBlocks.Select(cb => cb.Clone()))
            };
        }
    }

    // Для литературы
    public class LibraryRecord
    {
        public string Record { get; set; }
    }

    // Для программного обеспечения
    public class Software
    {
        public string Name { get; set; }
        public string LicenseType { get; set; }
    }

    public class ModuleStructureRow : INotifyPropertyChanged
    {
        private string _competencyCodes;
        private string _moduleName;
        private string _totalHours;
        private string _practicalPrepHours;
        private string _mdkHours;
        private string _trainingSessionsHours;
        private string _courseworkHours;
        private string _independentWorkHours;
        private string _educationalPracticeHours;
        private string _industrialPracticeHours;
        private string _assessmentHours;

        public string CompetencyCodes { get => _competencyCodes; set { _competencyCodes = value; OnPropertyChanged(); } }
        public string ModuleName { get => _moduleName; set { _moduleName = value; OnPropertyChanged(); } }
        public string TotalHours { get => _totalHours; set { _totalHours = value; OnPropertyChanged(); } }
        public string PracticalPrepHours { get => _practicalPrepHours; set { _practicalPrepHours = value; OnPropertyChanged(); } }
        public string MdkHours { get => _mdkHours; set { _mdkHours = value; OnPropertyChanged(); } }
        public string TrainingSessionsHours { get => _trainingSessionsHours; set { _trainingSessionsHours = value; OnPropertyChanged(); } }
        public string CourseworkHours { get => _courseworkHours; set { _courseworkHours = value; OnPropertyChanged(); } }
        public string IndependentWorkHours { get => _independentWorkHours; set { _independentWorkHours = value; OnPropertyChanged(); } }
        public string EducationalPracticeHours { get => _educationalPracticeHours; set { _educationalPracticeHours = value; OnPropertyChanged(); } }
        public string IndustrialPracticeHours { get => _industrialPracticeHours; set { _industrialPracticeHours = value; OnPropertyChanged(); } }
        public string AssessmentHours { get => _assessmentHours; set { _assessmentHours = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
   public class ModuleLaborIntensityModel : INotifyPropertyChanged
    {
        private string _learnTotal;
        private string _learnPractical;
        private string _courseworkTotal;
        private string _courseworkPractical;
        private string _selfWorkTotal;
        private string _practiceTotal;
        private string _practicePractical;
        private string _practiceEduTotal;
        private string _practiceEduPractical;
        private string _practiceIndTotal;
        private string _practiceIndPractical;
        private string _attestation;
        private string _totalHours;
        private string _totalPractical;

        // 1. Учебные занятия
        public string LearnTotal { get => _learnTotal; set { _learnTotal = value; OnPropertyChanged(); } }
        public string LearnPractical { get => _learnPractical; set { _learnPractical = value; OnPropertyChanged(); } }

        // 2. Курсовая работа
        public string CourseworkTotal { get => _courseworkTotal; set { _courseworkTotal = value; OnPropertyChanged(); } }
        public string CourseworkPractical { get => _courseworkPractical; set { _courseworkPractical = value; OnPropertyChanged(); } }

        // 3. Самостоятельная работа
        public string SelfWorkTotal { get => _selfWorkTotal; set { _selfWorkTotal = value; OnPropertyChanged(); } }

        // 4. Практика, в т.ч.:
        public string PracticeTotal { get => _practiceTotal; set { _practiceTotal = value; OnPropertyChanged(); } }
        public string PracticePractical { get => _practicePractical; set { _practicePractical = value; OnPropertyChanged(); } }

        // 5. учебная
        public string PracticeEduTotal { get => _practiceEduTotal; set { _practiceEduTotal = value; OnPropertyChanged(); } }
        public string PracticeEduPractical { get => _practiceEduPractical; set { _practiceEduPractical = value; OnPropertyChanged(); } }

        // 6. производственная
        public string PracticeIndTotal { get => _practiceIndTotal; set { _practiceIndTotal = value; OnPropertyChanged(); } }
        public string PracticeIndPractical { get => _practiceIndPractical; set { _practiceIndPractical = value; OnPropertyChanged(); } }

        // 7. Промежуточная аттестация
        public string Attestation { get => _attestation; set { _attestation = value; OnPropertyChanged(); } }

        // 8. Всего
        public string TotalHours { get => _totalHours; set { _totalHours = value; OnPropertyChanged(); } }
        public string TotalPractical { get => _totalPractical; set { _totalPractical = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class WorkingProgramViewModel : INotifyPropertyChanged
    {
        // --- Вкладка 1: Титульный лист ---
        protected string _title = "Министерство науки и высшего образования Российской Федерации\r\nФедеральное государственное бюджетное образовательное \r\nучреждение высшего образования \r\n«РЯЗАНСКИЙ ГОСУДАРСТВЕННЫЙ РАДИОТЕХНИЧЕСКИЙ УНИВЕРСИТЕТ\r\n ИМЕНИ В.Ф. УТКИНА»\r\n\r\nКолледж Рязанского государственного радиотехнического университета \r\nимени В.Ф. Уткина\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\t\t\r\n\r\nРАБОЧАЯ ПРОГРАММА\r\nучебной дисциплины\r\n";
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        // === ИСПРАВЛЕНО: геттер/сеттер больше не перекручены ===
        private string _disciplineName = "ОСНОВЫ ПРОЕКТИРОВАНИЯ БАЗ ДАННЫХ";
        public string DisciplineName
        {
            get => _disciplineName;
            set => SetProperty(ref _disciplineName, value);
        }

        private string _disciplineKod = "ОП.07";
        public string DisciplineKod
        {
            get => _disciplineKod;
            set => SetProperty(ref _disciplineKod, value);
        }
        public string CityAndYear { get; } = $"Рязань {DateTime.Now.Year}";

        // Год набора: если сейчас сентябрь или позже, то следующий год, иначе текущий
        public string EnrollmentYear { get; } = DateTime.Now.Month >= 9
            ? (DateTime.Now.Year + 1).ToString()
            : DateTime.Now.Year.ToString();
        /// <summary>Алиас под имя, которое используется в XAML (tbdicipKod).</summary>
        public string DisciplineNameKod
        {
            get => DisciplineKod;
            set => DisciplineKod = value;
        }

        private string _specialty = "09.02.13 Интеграция решений с применением технологий искусственного интеллекта";
        public string Specialty { get => _specialty; set => SetProperty(ref _specialty, value); }

        private string _qualification = "Специалист по работе с искусственным интеллектом";
        public string Qualification { get => _qualification; set => SetProperty(ref _qualification, value); }

        private string _educationForm = "очная";
        public string EducationForm { get => _educationForm; set => SetProperty(ref _educationForm, value); }

        private string _protocolNumber = "3";
        public string ProtocolNumber { get => _protocolNumber; set => SetProperty(ref _protocolNumber, value); }

        private DateTime? _protocolDate = new DateTime(2026, 10, 1);
        public DateTime? ProtocolDate { get => _protocolDate; set => SetProperty(ref _protocolDate, value); }

        private string _protocolPredsedatel = "Юдаев А.Н.";
        public string ProtocolPredsedatel { get => _protocolPredsedatel; set => SetProperty(ref _protocolPredsedatel, value); }

        private string _protocolSovetNumber = "1";
        public string ProtocolSovetNumber { get => _protocolSovetNumber; set => SetProperty(ref _protocolSovetNumber, value); }

        private DateTime? _protocolSovetDate = new DateTime(2026, 10, 5);
        public DateTime? ProtocolSovetDate { get => _protocolSovetDate; set => SetProperty(ref _protocolSovetDate, value); }
      
        public ObservableCollection<Developer> Developers { get; set; } = new ObservableCollection<Developer>();

        // --- Вкладка 2: Раздел 1 ---
        private string _purpose = "формирование знаний о принципах проектирования баз данных, освоение методологии их моделирования и разработки, развитие навыков структурирования, организации и оптимизации данных для эффективного решения задач в профессиональной деятельности.";
        public string Purpose { get => _purpose; set => SetProperty(ref _purpose, value); }

        private string _competenciesLabel = "Компетенции, раскрываемые в дисциплине:";
        public string CompetenciesLabel { get => _competenciesLabel; set => SetProperty(ref _competenciesLabel, value); }
       
        public ObservableCollection<Competency> Competencies { get; set; } = new ObservableCollection<Competency>();

        // --- Вкладка 3: Раздел 2.1 (Объем) ---
        private string _attestation = "-";
        /// <summary>Формы промежуточной аттестации по семестрам.</summary>
        public string Attestation
        {
            get => _attestation;
            set => SetProperty(ref _attestation, value);
        }
        private string _chasRabProg1 = "104";
        public string ChasRabProg1 { get => _chasRabProg1; set => SetProperty(ref _chasRabProg1, value); }

        private string _chasRabProg2 = "70";
        public string ChasRabProg2 { get => _chasRabProg2; set => SetProperty(ref _chasRabProg2, value); }

        private string _chasOsnov = "92";
        public string ChasOsnov { get => _chasOsnov; set => SetProperty(ref _chasOsnov, value); }

        private string _teaorChasOsnov = "22";
        public string TeaorChasOsnov { get => _teaorChasOsnov; set => SetProperty(ref _teaorChasOsnov, value); }

        private string _practChas1 = "70";
        public string PractChas1 { get => _practChas1; set => SetProperty(ref _practChas1, value); }

        private string _practChas2 = "70";
        public string PractChas2 { get => _practChas2; set => SetProperty(ref _practChas2, value); }

        private string _kursachChas = "-";
        public string KursachChas { get => _kursachChas; set => SetProperty(ref _kursachChas, value); }

        private string _samRabChas = "4";
        public string SamRabChas { get => _samRabChas; set => SetProperty(ref _samRabChas, value); }

        private string _semestr = "4 семестр";
        public string Semestr { get => _semestr; set => SetProperty(ref _semestr, value); }

        private string _atesChas = "8 (подготовка 2, экзамен 6)";
        public string AtesChas { get => _atesChas; set => SetProperty(ref _atesChas, value); }

        // --- Вкладка 3: Раздел 2.2 (Тематический план) ---
        public ObservableCollection<ThemePlan> Themes { get; set; } = new ObservableCollection<ThemePlan>();

        // --- Вкладка 4: Раздел 3 ---
        private string _trebKab = "Специальные помещения для проведения занятий всех видов, в том числе для групповых и индивидуальных консультаций, а также проведения текущего контроля, промежуточной аттестации и самостоятельной работы:\n1) кабинет «Общепрофессиональных дисциплин и профессиональных модулей»...\n2) лаборатория «Программирования и баз данных»...";
        public string TrebKab { get => _trebKab; set => SetProperty(ref _trebKab, value); }
        public KtpData Ktp { get; set; } = new KtpData();
        public ObservableCollection<LibraryRecord> OsnovLibrary { get; set; } = new ObservableCollection<LibraryRecord>();
        public ObservableCollection<LibraryRecord> DopLibrary { get; set; } = new ObservableCollection<LibraryRecord>();
        public ObservableCollection<Software> Softwares { get; set; } = new ObservableCollection<Software>();
        public ObservableCollection<EvaluationCriteria> Evaluations { get; set; } = new ObservableCollection<EvaluationCriteria>();
        public void GenerateKtpFromThemes()
        {
            Ktp.Rows.Clear();
            int currentLesson = 1;

            foreach (var theme in Themes)
            {
                if (theme.IsSection)
                {
                    Ktp.Rows.Add(new KtpRow { LessonNums = "", ThemeName = theme.ThemeName, Hours = "", Type = "", Aids = "", Tasks = "" });
                    continue;
                }

                // Пропускаем самостоятельную работу и аттестацию, если они не нужны в КТП
                if (theme.ThemeName.StartsWith("Промежуточная", StringComparison.OrdinalIgnoreCase) )
                {
                    int th = int.TryParse(theme.TotalHours, out int t) ? t : 0;
                    if (th > 0) Ktp.Rows.Add(new KtpRow { LessonNums = "", ThemeName = theme.ThemeName, Hours = th.ToString(), Type = "Пром.аттестация.", Aids = "", Tasks = "" });
                    continue;
                }
                if (theme.ThemeName.StartsWith("Самостоятельная", StringComparison.OrdinalIgnoreCase)) continue;
                foreach (var block in theme.ContentBlocks)
                {
                    int hours = int.TryParse(block.Hours, out int h) ? h : 0;
                    if (hours <= 0) continue;

                    bool isPractice = block.Description.IndexOf("Практическ", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                      block.Description.IndexOf("П.Р.", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                      block.Description.IndexOf("Лаборатор", StringComparison.OrdinalIgnoreCase) >= 0;

                    string type = isPractice ? "Пр.зан." : "лекция";
                    string aids = isPractice ? "Компьютер" : "доска, проектор, раздаточный материал";

                    // Вычисление номеров занятий (из расчета 2 часа на занятие)
                    int lessonCount = hours / 2;
                    string lessonNums = "";
                    if (lessonCount == 1) lessonNums = currentLesson.ToString();
                    else if (lessonCount > 1) lessonNums = $"{currentLesson}, {currentLesson + lessonCount - 1}";
                    currentLesson += lessonCount;
                    block.Description.Replace("Практические занятия:\n", "");
                    // Берем первую строку описания для названия
                    string title = block.Description;
                    
                        Ktp.Rows.Add(new KtpRow
                        {
                            LessonNums = lessonNums,
                            ThemeName = $"{title}",
                            Hours = (hours).ToString(),
                            Type = type,
                            Aids = aids,
                            Tasks = ""
                        });
                    

                    
                }
            }
        }
        // --- Конструктор с предзаполнением данных ---
        public WorkingProgramViewModel()
        {

        }
        public WorkingProgramViewModel(string a)
        {
            // Разработчики
            Developers.Add(new Developer { FullName = "Юдаев А.Н.", Position = "преподаватель Колледжа РГРТУ" });
            // Компетенции (Для вкладки 1.2)
            Competencies.Add(new Competency
            {
                Code = "ОК 01",
                KnowHow = "- распознавать задачу и/или проблему в профессиональном и/или социальном контексте, анализировать и выделять её составные части;\n- определять этапы решения задачи...",
                Knowledge = "- актуальный профессиональный и социальный контекст, в котором приходится работать и жить;\n- структура плана для решения задач...",

            });
            Competencies.Add(new Competency
            {
                Code = "ОК 02",
                KnowHow = "- определять задачи для поиска информации, планировать процесс поиска, выбирать необходимые источники информации;\n- выделять наиболее значимое...",
                Knowledge = "- номенклатура информационных источников, применяемых в профессиональной деятельности;\n- приемы структурирования информации...",

            });

            CompetenciesLabel += "\n";
            foreach (var item in Competencies)
            {
                CompetenciesLabel += item.Code + " описание из уп";
                CompetenciesLabel += "\n";
            }

            // ==========================================
            // ТЕМАТИЧЕСКИЙ ПЛАН (Раздел 2.2)
            // ==========================================

            // 1. Добавляем Раздел 1
            Themes.Add(new ThemePlan
            {
                ThemeName = "Раздел 1. Основы проектирования баз данных",
                IsSection = true // Указываем, что это раздел
            });

            // Тема 1.1
            var theme1 = new ThemePlan
            {
                ThemeName = "Тема 1.1. Введение в базы данных",
                TotalHours = "10",
                Competences = "ОК 01, ОК 02, ОК 04, ОК 05, ОК 09"
            };
            theme1.ContentBlocks.Add(new ContentBlock
            {
                Description = "Понятие базы данных, её роль и применение. Основы реляционной модели данных.",
                Hours = "4"
            });
            theme1.ContentBlocks.Add(new ContentBlock
            {
                Description = "Практические занятия:\nАнализ структуры базы данных на примере реальной системы.\nПостроение ER-диаграммы для простой предметной области.",
                Hours = "6"
            });
            Themes.Add(theme1);

            // Тема 1.2
            var theme2 = new ThemePlan
            {
                ThemeName = "Тема 1.2. Концептуальное проектирование баз данных",
                TotalHours = "12",
                Competences = "ОК 01, ОК 02, ОК 04, ОК 05, ОК 09"
            };
            theme2.ContentBlocks.Add(new ContentBlock
            {
                Description = "Основы концептуального проектирования, ER-диаграммы, основные сущности и связи.",
                Hours = "2"
            });
            theme2.ContentBlocks.Add(new ContentBlock
            {
                Description = "Практические занятия:\nОпределение сущностей и атрибутов для заданной предметной области.\nПостроение сложной ER-диаграммы с учётом нормализации.",
                Hours = "10"
            });
            Themes.Add(theme2);

            // 2. Добавляем Раздел 2
            Themes.Add(new ThemePlan
            {
                ThemeName = "Раздел 2. Логическое и физическое проектирование баз данных",
                IsSection = true
            });

            // Тема 2.1
            var theme3 = new ThemePlan
            {
                ThemeName = "Тема 2.1. Основы SQL и работа с запросами",
                TotalHours = "18",
                Competences = "ОК 01, ОК 02,\nОК 04, ОК 05,\nОК 09"
            };
            theme3.ContentBlocks.Add(new ContentBlock
            {
                Description = "Основы языка SQL, создание таблиц, выполнение основных запросов (SELECT, INSERT, UPDATE, DELETE).",
                Hours = "4"
            });
            theme3.ContentBlocks.Add(new ContentBlock
            {
                Description = "Практические занятия:\nНаписание SQL-запросов для выборки данных из базы.\nСоздание и модификация данных в таблицах.",
                Hours = "14"
            });
            Themes.Add(theme3);

            // Далее идет "Самостоятельная работа" и "Промежуточная аттестация"...

            // Литература
            OsnovLibrary.Add(new LibraryRecord { Record = "Ткаченко, С. Н., Основы проектирования баз данных : учебник / С. Н. Ткаченко. — Москва : КноРус, 2024. — 176 с." });
            DopLibrary.Add(new LibraryRecord { Record = "Удахина, С. В. Базы данных : учебное пособие / С. В. Удахина. — Санкт-Петербург : СПБГУ ГА им. А.А. Новикова, 2025." });

            // Программное обеспечение
            Softwares.Add(new Software { Name = "Операционная система Windows", LicenseType = "Коммерческая лицензия" });
            Softwares.Add(new Software { Name = "Draw.io", LicenseType = "Свободное ПО" });
            Softwares.Add(new Software { Name = "SQL Server Developer", LicenseType = "Свободное ПО для образовательных целей" });

            Evaluations.Add(new EvaluationCriteria
            {
                LearningOutcome = "ОК 01. Выбирать способы решения задач профессиональной деятельности применительно к различным контекстам",
                Indicators = "Оценка «отлично» – Выбор эффективного способа решения задачи; реализация решения с учетом профессионального контекста.\nОценка «хорошо» – Выбор решения с минимальными недочетами.\nОценка «удовлетворительно» – Выбор решения с ограниченной эффективностью.",
                AssessmentMethod = "Экзамен"
            });

            Evaluations.Add(new EvaluationCriteria
            {
                LearningOutcome = "ОК 02. Использовать современные средства поиска, анализа и интерпретации информации, и информационные технологии для выполнения задач профессиональной деятельности",
                Indicators = "Оценка «отлично» – Использование современных средств анализа информации, интерпретация данных с высокой точностью.\nОценка «хорошо» – Использование информационных средств с минимальными ошибками.\nОценка «удовлетворительно» – Использование информационных технологий с ограниченными возможностями анализа.",
                AssessmentMethod = "Экзамен"
            });

            Evaluations.Add(new EvaluationCriteria
            {
                LearningOutcome = "ОК 04. Эффективно взаимодействовать и работать в коллективе и команде",
                Indicators = "Оценка «отлично» – Эффективное взаимодействие в коллективе, демонстрация лидерских качеств.\nОценка «хорошо» – Взаимодействие в коллективе с минимальными трудностями.\nОценка «удовлетворительно» – Участие в работе команды с ограниченным вкладом.",
                AssessmentMethod = "Экзамен"
            });

            Evaluations.Add(new EvaluationCriteria
            {
                LearningOutcome = "ОК 05. Осуществлять устную и письменную коммуникацию на государственном языке Российской Федерации с учетом особенностей социального и культурного контекста",
                Indicators = "Оценка «отлично» – Устная и письменная коммуникация на высоком уровне с учетом особенностей культурного контекста.\nОценка «хорошо» – Коммуникация с минимальными грамматическими ошибками.\nОценка «удовлетворительно» – Коммуникация с ограниченным пониманием культурных особенностей.",
                AssessmentMethod = "Экзамен"
            });

            Evaluations.Add(new EvaluationCriteria
            {
                LearningOutcome = "ОК 09. Пользоваться профессиональной документацией на государственном и иностранном языках",
                Indicators = "Оценка «отлично» – Свободное использование профессиональной документации на обоих языках.\nОценка «хорошо» – Использование документации с минимальными ошибками.\nОценка «удовлетворительно» – Использование документации на базовом уровне.",
                AssessmentMethod = "Экзамен"
            });

            // ==========================================
            // САМОСТОЯТЕЛЬНАЯ РАБОТА
            // ==========================================
            var independentWork = new ThemePlan
            {
                ThemeName = "Самостоятельная работа обучающихся",
                TotalHours = "2",
                Competences = "ОК 01, ОК 02,\nОК 04, ОК 05,\nОК 09"
            };
            independentWork.ContentBlocks.Add(new ContentBlock
            {
                Description = "Проработка конспекта лекций и учебной литературы:\n" +
                              "- изучение теории информационных технологий;\n" +
                              "- изучение основ работы с документами;\n" +
                              "- изучение особенностей работы с офисным пакетом;\n" +
                              "- изучение особенностей работы с табличными документами;\n" +
                              "- изучение особенностей работы с изобразительными средствами и средствами создания презентаций.",
                Hours = ""
            });
            Themes.Add(independentWork);

            // ==========================================
            // ПРОМЕЖУТОЧНАЯ АТТЕСТАЦИЯ
            // ==========================================
            var intermediateAssessment = new ThemePlan
            {
                ThemeName = "Промежуточная аттестация обучающихся",
                TotalHours = "4",
                Competences = "ОК 01, ОК 02,\nОК 04, ОК 05,\nОК 09"
            };
            intermediateAssessment.ContentBlocks.Add(new ContentBlock
            {
                Description = "1 семестр, в том числе:",
                Hours = ""
            });
            intermediateAssessment.ContentBlocks.Add(new ContentBlock
            {
                Description = "подготовка к аттестации",
                Hours = "2"
            });
            intermediateAssessment.ContentBlocks.Add(new ContentBlock
            {
                Description = "зачет с оценкой",
                Hours = "2"
            });
            Themes.Add(intermediateAssessment);


        }
        public string CheckHoursConsistency()
        {
            int totalThemesHours = 0;
            int theoryHours = 0;
            int practiceHours = 0;
            int independentHours = 0;
            int assessmentHours = 0;

            foreach (var theme in Themes)
            {
                if (theme.IsSection) continue;

                int themeHours = int.TryParse(theme.TotalHours, out int th) ? th : 0;
                totalThemesHours += themeHours;

                if (theme.ThemeName.StartsWith("Самостоятельная работа", StringComparison.OrdinalIgnoreCase))
                {
                    independentHours += themeHours;
                    continue;
                }

                if (theme.ThemeName.StartsWith("Промежуточная аттестация", StringComparison.OrdinalIgnoreCase))
                {
                    assessmentHours += themeHours;
                    continue;
                }

                // Считаем теорию и практику в обычных темах
                foreach (var block in theme.ContentBlocks)
                {
                    int bh = int.TryParse(block.Hours, out int h) ? h : 0;
                    if (block.Description.IndexOf("Практическ", StringComparison.OrdinalIgnoreCase) >= 0)
                        practiceHours += bh;
                    else
                        theoryHours += bh;
                }
            }

            var errors = new List<string>();
            if (ChasRabProg1 != totalThemesHours.ToString())
                errors.Add($"• Общий объем: в п. 2.1 — {ChasRabProg1}, а по факту (п. 2.2) — {totalThemesHours}");

            if (TeaorChasOsnov != theoryHours.ToString())
                errors.Add($"• Теория: в п. 2.1 — {TeaorChasOsnov}, а по факту (п. 2.2) — {theoryHours}");

            if (PractChas1 != practiceHours.ToString())
                errors.Add($"• Практика: в п. 2.1 — {PractChas1}, а по факту (п. 2.2) — {practiceHours}");

            if (SamRabChas != independentHours.ToString())
                errors.Add($"• Самостоятельная работа: в п. 2.1 — {SamRabChas}, а по факту (п. 2.2) — {independentHours}");

            if (AtesChas != assessmentHours.ToString())
                errors.Add($"• Аттестация: в п. 2.1 — {AtesChas}, а по факту (п. 2.2) — {assessmentHours}");

            if (errors.Any()) return string.Join("\n", errors);
            return "";
        }

        // --- INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}