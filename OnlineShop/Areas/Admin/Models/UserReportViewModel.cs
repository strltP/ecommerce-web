namespace OnlineShop.Areas.Admin.Models
{
    public class UserReportViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalRegularUsers => TotalUsers - TotalAdmins;

        public Dictionary<string, int> UsersPerDay { get; set; } = new();      // 7 ngày gần nhất
        public Dictionary<string, int> UsersPerMonth { get; set; } = new();    // Theo tháng trong năm
        public Dictionary<int, int> UsersPerYear { get; set; } = new();        // Mỗi năm

        public int SelectedYear { get; set; }
        public List<int> AvailableYears { get; set; } = new();                // Dropdown chọn năm
    }
}
