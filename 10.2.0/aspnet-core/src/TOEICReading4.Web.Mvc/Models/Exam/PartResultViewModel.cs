using System;

namespace TOEICReading4.Web.Models.Exam
{
    public class PartResultViewModel
    {
        public int Correct { get; set; }

        public int Total { get; set; }

        public int Percentage => Total == 0 ? 0 : (int)Math.Round((double)Correct / Total * 100);
    }
}
