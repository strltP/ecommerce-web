-------Tạo Database-------------
CREATE DATABASE OnlineShopTEST1
GO
--------------
USE OnlineShopTEST1
GO

---- Tạo bảng Banner (admin dùng để tạo banner tự động)
CREATE TABLE Banner (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200),
    SubTitle NVARCHAR(1000),
    ImageName NVARCHAR(50),
    Priority SMALLINT,
    Link NVARCHAR(100),
    Position NVARCHAR(50)
);
-----Tạo bảng Category----------
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

-- Tạo bảng Products
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200),
    Description NVARCHAR(500),
    FullDesc NVARCHAR(4000),
    Price MONEY,
    Discount MONEY,
    ImageName NVARCHAR(50),
    Qty INT,
    Tags NVARCHAR(1000),
    VideoUrl NVARCHAR(300),
	CategoryId INT,
	FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

-- Tạo bảng Comment
CREATE TABLE Comment (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50) NOT NULL,
    CommentText NVARCHAR(1000) NOT NULL,
    ProductId INT NOT NULL,
    CreateDate DATETIME NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- Tạo bảng Coupon
CREATE TABLE Coupon (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Code NVARCHAR(50) NOT NULL,
    Discount MONEY NOT NULL
);

-- Tạo bảng Menus (admin dùng để thêm menu tự động)
CREATE TABLE Menus (
    ID INT PRIMARY KEY IDENTITY(1,1),
    MenuTitle NVARCHAR(50),
    Link NVARCHAR(300),
    Type NVARCHAR(20)
);
-- Tạo bảng [User]
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(50) NOT NULL,
    FullName NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    IsAdmin BIT NOT NULL,
    RegisterDate DATETIME,
    RecoveryCode INT
);
-- Tạo bảng [Order]
CREATE TABLE [Order] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    CompanyName NVARCHAR(50),
    Country NVARCHAR(50) NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
    Comment NVARCHAR(200),
    CouponCode NVARCHAR(50),
    CouponDiscount MONEY,
    Shipping MONEY,
    SubTotal MONEY,
    Total MONEY,
    CreateDate DATETIME,
    TransId NVARCHAR(200),
    Status NVARCHAR(50),
    FOREIGN KEY (UserId) REFERENCES [User](Id)
);

-- Tạo bảng OrderDetails
CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductTitle NVARCHAR(200) NOT NULL,
    ProductPrice MONEY NOT NULL,
    Count INT NOT NULL,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES [Order](Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- Tạo bảng ProductGallery
CREATE TABLE ProductGallery (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    ImageName NVARCHAR(50),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);


-- Tạo bảng Settings (admin dùng)
CREATE TABLE Settings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Shipping MONEY,
    Title NVARCHAR(100),
    Address NVARCHAR(500),
    Email NVARCHAR(50),
    Phone NVARCHAR(50),
    CopyRight NVARCHAR(100),
    Instagram NVARCHAR(100),
    FaceBook NVARCHAR(100),
    GooglePlus NVARCHAR(100),
    Youtube NVARCHAR(100),
    Twitter NVARCHAR(100),
    Logo NVARCHAR(50)
);
GO
------------------------Tạo Views BestSellingTemp-------------------

CREATE VIEW BestSellingTemp AS
SELECT 
    dbo.OrderDetails.ProductId,
    dbo.OrderDetails.Count,
    dbo.Products.Title,
    dbo.Products.Price,
    dbo.Products.Discount,
    dbo.Products.ImageName,
    dbo.Products.Qty,
    dbo.OrderDetails.OrderId,
    dbo.[Order].Status
FROM dbo.OrderDetails
INNER JOIN dbo.Products ON dbo.Products.Id = dbo.OrderDetails.ProductId
LEFT OUTER JOIN dbo.[Order] ON dbo.OrderDetails.OrderId = dbo.[Order].Id
WHERE dbo.[Order].Status = N'approved';

----------------------
GO
---------------------------Tạo Views BestSellingFinal-------------------
CREATE VIEW dbo.BestSellingFinal AS
SELECT 
    ProductId,
    SUM(Count) AS TotalSum,
    Title,
    Price,
    Discount,
    ImageName,
    Qty,
    Status
FROM dbo.BestSellingTemp
GROUP BY 
    ProductId, 
    Title, 
    Price, 
    Discount, 
    ImageName, 
    Qty, 
    Status;


----------------------------------------------------ADD DATA-------------------------------
---Menus---------
-- Chèn dữ liệu vào bảng Menus
INSERT INTO Menus (MenuTitle, Link, Type) VALUES 
(N'Home', N'/', N'Top'),
(N'Shop', N'/products', N'Top'),
(N'Home', N'index.html', N'Bottom'),
(N'Shopping Cart', N'/Cart', N'Top'),
(N'Shop', N'/products', N'Bottom'),
(N'Checkout', N'/Cart/Checkout', N'Top');
------------Products------------------------
INSERT INTO Products 
(Title, Description, FullDesc, Price, Discount, ImageName, Qty, Tags, VideoUrl)
VALUES
(N'Áo Thun Nam Cổ Tròn', 
 N'Áo thun nam chất liệu cotton thoáng mát.', 
 N'Áo thun nam cổ tròn, chất vải cotton co giãn, thấm hút mồ hôi tốt. Phù hợp mặc hằng ngày hoặc đi chơi.', 
 199000, 50000, N'n', 100, N'áo, nam, cotton, casual', N'n'),

(N'Quần Jeans Nữ Lưng Cao', 
 N'Quần jeans nữ form ôm, co giãn nhẹ.', 
 N'Quần jeans nữ lưng cao tôn dáng, chất denim bền đẹp. Phù hợp đi học, đi chơi.', 
 399000, 70000, N'n', 80, N'quần, nữ, jeans, denim', N'n'),

(N'Áo Sơ Mi Nam Dài Tay Trơn', 
 N'Sơ mi nam chất vải cao cấp, kiểu dáng basic.', 
 N'Áo sơ mi nam dài tay, form slim fit, phù hợp môi trường công sở và đi tiệc nhẹ.', 
 299000, 30000, N'n', 60, N'áo, sơ mi, nam, công sở', N'n'),

(N'Chân Váy Chữ A Nữ', 
 N'Chân váy chữ A dễ phối đồ.', 
 N'Chân váy chất liệu kaki nhẹ, độ dài trên gối. Thiết kế trẻ trung phù hợp mọi vóc dáng.', 
 259000, 20000, N'n', 50, N'chân váy, nữ, thời trang', N'n'),

(N'Áo Khoác Jean Nam', 
 N'Áo khoác jean cá tính, thời trang.', 
 N'Áo khoác jean nam chất denim cao cấp, form rộng, phù hợp mặc đi chơi hoặc dạo phố.', 
 499000, 100000, N'n', 40, N'áo khoác, nam, jean, streetwear', N'n'),

(N'Đầm Maxi Nữ Dự Tiệc', 
 N'Đầm maxi nữ dài sang trọng.', 
 N'Váy đầm maxi chất voan lụa mềm mại, thiết kế nữ tính, phù hợp đi tiệc, sự kiện.', 
 599000, 150000, N'n', 30, N'đầm, maxi, nữ, dự tiệc', N'n');

 ---------------Coupon-------------------------
 INSERT INTO Coupon (Code, Discount)
VALUES
(N'SUMMER50K', 50000),
(N'FREESHIP', 30000),
(N'WELCOME100', 100000),
(N'TET2025', 250000),
(N'SALE10K', 10000),
(N'NEWUSER', 70000);
-----------------------
---add settings data----


---ADD DATA CATE----
INSERT INTO Categories (Name, Description)
VALUES 
('Áo sơ mi', 'Các loại áo sơ mi nam nữ'),
('Quần jeans', 'Quần jeans đủ size, kiểu dáng'),
('Giày dép', 'Giày thể thao, sandal, cao gót'),
('Phụ kiện', 'Túi xách, kính mát, mũ nón');
