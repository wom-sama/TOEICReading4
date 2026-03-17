using System.Collections.Generic;

namespace TOEICReading4.Web.Models.Exam
{
    public class ExamSubmissionDto
    {
        public int ExamId { get; set; }

        public int TimeTakenSeconds { get; set; }

        public Dictionary<int, string> Answers { get; set; }
    }
}
