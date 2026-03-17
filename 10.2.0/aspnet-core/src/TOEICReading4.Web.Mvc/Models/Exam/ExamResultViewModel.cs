using System;

namespace TOEICReading4.Web.Models.Exam
{
    public class ExamResultViewModel
    {
        public long AttemptId { get; set; }

        public int ExamId { get; set; }

        public string ExamTitle { get; set; }

        public DateTime CompletedAt { get; set; }

        public int Score { get; set; }

        public int TimeTakenSeconds { get; set; }

        public int CorrectCount { get; set; }

        public int IncorrectCount { get; set; }

        public int SkippedCount { get; set; }

        public int TotalQuestions { get; set; }

        public PartResultViewModel Part5 { get; set; } = new PartResultViewModel();

        public PartResultViewModel Part6 { get; set; } = new PartResultViewModel();

        public PartResultViewModel Part7 { get; set; } = new PartResultViewModel();
    }
}
