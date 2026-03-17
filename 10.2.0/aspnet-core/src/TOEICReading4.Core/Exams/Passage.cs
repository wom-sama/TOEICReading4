using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TOEICReading4.Exams
{
    [Table("Passages")]
    public class Passage : Entity<int>
    {
        public int ExamId { get; set; }
        
        public int PartNumber { get; set; }

        public string Content { get; set; }

        [ForeignKey(nameof(ExamId))]
        public virtual Exam Exam { get; set; }

        // Mối quan hệ: 1 Đoạn văn có nhiều Câu hỏi (dành cho Part 6, 7)
        public virtual ICollection<Question> Questions { get; set; }
    }
}