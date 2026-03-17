using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TOEICReading4.Exams
{
    [Table("Questions")]
    public class Question : Entity<int>
    {
        public int ExamId { get; set; }

        public int? PassageId { get; set; } // Nullable vì Part 5 không có đoạn văn

        public int PartNumber { get; set; }

        public int QuestionNumber { get; set; }

        public string Content { get; set; }

        [MaxLength(500)]
        public string OptionA { get; set; }

        [MaxLength(500)]
        public string OptionB { get; set; }

        [MaxLength(500)]
        public string OptionC { get; set; }

        [MaxLength(500)]
        public string OptionD { get; set; }

        [MaxLength(1)]
        public string CorrectKey { get; set; } // 'A', 'B', 'C', 'D'

        public string Explanation { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }

        [ForeignKey(nameof(PassageId))]
        public virtual Passage Passage { get; set; }
    }
}