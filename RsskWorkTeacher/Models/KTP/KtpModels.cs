using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RsskWorkTeacher.Models
{
    public class KtpRow : INotifyPropertyChanged
    {
        private string _lessonNums;
        private string _themeName;
        private string _hours;
        private string _dates;
        private string _type;
        private string _aids;
        private string _tasks;

        public string LessonNums { get => _lessonNums; set { _lessonNums = value; OnPropertyChanged(); } }
        public string ThemeName { get => _themeName; set { _themeName = value; OnPropertyChanged(); } }
        public string Hours { get => _hours; set { _hours = value; OnPropertyChanged(); } }
        public string Dates { get => _dates; set { _dates = value; OnPropertyChanged(); } }
        public string Type { get => _type; set { _type = value; OnPropertyChanged(); } }
        public string Aids { get => _aids; set { _aids = value; OnPropertyChanged(); } }
        public string Tasks { get => _tasks; set { _tasks = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public KtpRow Clone()
        {
            return new KtpRow
            {
                LessonNums = this.LessonNums,
                ThemeName = this.ThemeName,
                Hours = this.Hours,
                Dates = this.Dates,
                Type = this.Type,
                Aids = this.Aids,
                Tasks = this.Tasks
            };
        }
    }

    public class KtpData : INotifyPropertyChanged
    {
        private string _groups;
        private string _teachers;
        private string _period;
        private string _assessmentForm;

        public string Groups { get => _groups; set { _groups = value; OnPropertyChanged(); } }
        public string Teachers { get => _teachers; set { _teachers = value; OnPropertyChanged(); } }
        public string Period { get => _period; set { _period = value; OnPropertyChanged(); } }
        public string AssessmentForm { get => _assessmentForm; set { _assessmentForm = value; OnPropertyChanged(); } }

        public ObservableCollection<KtpRow> Rows { get; set; } = new ObservableCollection<KtpRow>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}