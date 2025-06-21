using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Models
{
    public class MonthlyStat
    {
        public int Month { get; set; }
        public int Count { get; set; }
    }

    public class OrderMonthlyStat
    {
        public int Month { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ReportViewModel
    {
        public List<MonthlyStat> UserStats { get; set; }
        public List<OrderMonthlyStat> OrderStats { get; set; }

        public List<BestSellingFinal> TopSellingProducts { get; set; }
    }

}
