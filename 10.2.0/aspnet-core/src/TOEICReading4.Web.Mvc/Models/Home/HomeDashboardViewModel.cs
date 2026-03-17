using System.Collections.Generic;

namespace TOEICReading4.Web.Models.Home
{
    public class HomeDashboardViewModel
    {
        public int TotalExams { get; set; }

        public int CompletedExams { get; set; }

        public int AvailableExams { get; set; }

        public int BestScore { get; set; }

        public IReadOnlyList<DashboardExamViewModel> RecentExams { get; set; } = new List<DashboardExamViewModel>();
    }
}
