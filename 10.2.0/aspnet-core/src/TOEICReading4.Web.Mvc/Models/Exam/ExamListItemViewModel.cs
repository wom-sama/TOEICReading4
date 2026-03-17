using System;

namespace TOEICReading4.Web.Models.Exam
{
    public class ExamListItemViewModel
    {
        public TOEICReading4.Exams.Exam Exam { get; set; }

        public bool HasAttempt { get; set; }

        public long? LatestAttemptId { get; set; }

        public int? LatestScore { get; set; }

        public DateTime? LastCompletedAt { get; set; }
    }
}
