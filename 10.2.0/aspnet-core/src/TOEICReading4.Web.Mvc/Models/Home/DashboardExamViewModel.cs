using System;

namespace TOEICReading4.Web.Models.Home
{
    public class DashboardExamViewModel
    {
        public int ExamId { get; set; }

        public string Title { get; set; }

        public int DurationMinutes { get; set; }

        public int QuestionCount { get; set; }

        public bool HasAttempt { get; set; }

        public long? LatestAttemptId { get; set; }

        public int? LatestScore { get; set; }

        public DateTime ActivityDate { get; set; }
    }
}
