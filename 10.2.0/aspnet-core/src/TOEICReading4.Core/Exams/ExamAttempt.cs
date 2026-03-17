using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TOEICReading4.Exams
{
    [Table("ExamAttempts")]
    public class ExamAttempt : Entity<long>
    {
        public int ExamId { get; set; }

        public long UserId { get; set; }

        public DateTime CompletedAt { get; set; }

        public int TimeTakenSeconds { get; set; }

        public int Score { get; set; }

        public int CorrectCount { get; set; }

        public int IncorrectCount { get; set; }

        public int SkippedCount { get; set; }

        public int TotalQuestions { get; set; }

        public int Part5Correct { get; set; }

        public int Part5Total { get; set; }

        public int Part6Correct { get; set; }

        public int Part6Total { get; set; }

        public int Part7Correct { get; set; }

        public int Part7Total { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }
    }
}
