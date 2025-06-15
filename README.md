# ecommerce-web
#1. mở project, vào Tools > NuGet Package Manager > Package Manager Console. sau đó gõ lệnh dotnet restore <br>
#2. mở mssql rồi tạo database qua file create_database_n_add_data_.sql <br>
#3.  scaffold database qua lệnh: Scaffold-DbContext “Server={server_name};Database={database_name};Trusted_Connection=True;TrustServerCertificate=true” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/Db -Context OnlineShopContext -force <br>
#4.------------------------ <br>
#5. tạo account paypal developer để lấy Key và Secret trong Apps &  và paste vào appsettings.json <br>
