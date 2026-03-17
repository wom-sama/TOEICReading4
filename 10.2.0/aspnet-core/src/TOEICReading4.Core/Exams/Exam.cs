using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TOEICReading4.Exams
{
    [Table("Exams")]
    public class Exam : Entity<int>
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public int DurationMinutes { get; set; }

        public DateTime CreatedAt { get; set; }

        // Mối quan hệ: 1 Đề thi có nhiều Đoạn văn và Câu hỏi
        public virtual ICollection<Passage> Passages { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}