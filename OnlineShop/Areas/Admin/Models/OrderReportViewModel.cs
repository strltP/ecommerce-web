namespace OnlineShop.Areas.Admin.Models
{
    public class OrderReportViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public Dictionary<string, int> OrdersPerDay { get; set; } = new();      // 7 ngày gần nhất
        public Dictionary<string, int> OrdersPerMonth { get; set; } = new();    // 12 tháng
        public Dictionary<string, decimal> RevenuePerMonth { get; set; } = new();
        public Dictionary<string, int> OrdersByStatus { get; set; } = new();
        public Dictionary<string, decimal> RevenuePerDay { get; set; } = new();
        public Dictionary<int, decimal> RevenuePerYear { get; set; } = new();
       
        public Dictionary<int, int> OrdersPerYear { get; set; } = new();
       

        public List<int> AvailableYears { get; set; } = new();
        public int SelectedYear { get; set; }
    }
}
