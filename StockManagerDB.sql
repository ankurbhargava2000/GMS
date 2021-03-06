USE [master]
GO
/****** Object:  Database [StockManager]    Script Date: 24-11-2017 09:25:25 ******/
CREATE DATABASE [StockManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StockManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS01\MSSQL\DATA\StockManager.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'StockManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS01\MSSQL\DATA\StockManager_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [StockManager] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StockManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StockManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StockManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StockManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StockManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StockManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [StockManager] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StockManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StockManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StockManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StockManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StockManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StockManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StockManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StockManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StockManager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StockManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StockManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StockManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StockManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StockManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StockManager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StockManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StockManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [StockManager] SET  MULTI_USER 
GO
ALTER DATABASE [StockManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StockManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StockManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StockManager] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StockManager] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [StockManager] SET QUERY_STORE = OFF
GO
USE [StockManager]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [StockManager]
GO
/****** Object:  Table [dbo].[ChalanType]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChalanType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChalanType] [varchar](50) NOT NULL,
	[Description] [varchar](200) NULL,
	[IsInput] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialYear]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialYear](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
 CONSTRAINT [PK_FinancialYear] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceDetails]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[invoice_id] [int] NULL,
	[product_id] [int] NULL,
	[sale_rate] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[discount] [float] NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceMaster]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[invoice_no] [int] NOT NULL,
	[customer_id] [int] NULL,
	[gross_amount] [float] NOT NULL,
	[net_amount] [float] NOT NULL,
	[created_on] [date] NULL,
	[created_by] [int] NULL,
	[financial_year] [int] NULL,
	[tenant_id] [int] NULL,
	[discount] [float] NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrinterChalan]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrinterChalan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NOT NULL,
	[ChalanDate] [datetime] NOT NULL,
	[Created] [datetime] NULL,
	[Updated] [datetime] NULL,
	[Description] [varchar](500) NULL,
	[chalan_number] [int] NULL,
	[dispatch_document_number] [varchar](100) NULL,
	[dispatched_through] [varchar](100) NULL,
	[bale_numbers] [varchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrinterChalanDetails]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrinterChalanDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChalanId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [numeric](18, 2) NULL,
	[Fold] [int] NULL,
	[ExpectedFold] [int] NULL,
	[NetQuantity] [numeric](18, 2) NULL,
	[Shrinkage] [numeric](18, 2) NULL,
	[Description] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrintJobWorkReceived]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrintJobWorkReceived](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NOT NULL,
	[ChalanDate] [datetime] NOT NULL,
	[Created] [datetime] NULL,
	[Updated] [datetime] NULL,
	[Description] [varchar](500) NULL,
	[chalan_number] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrintJobWorkReceivedDetails]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrintJobWorkReceivedDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChalanId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [numeric](18, 2) NULL,
	[Fold] [int] NULL,
	[Description] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductTypeId] [int] NULL,
	[ProductName] [varchar](100) NULL,
	[Description] [varchar](200) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductType] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseDetails]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [numeric](18, 2) NOT NULL,
	[Fold] [int] NOT NULL,
	[Rate] [numeric](18, 2) NULL,
	[Description] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrders]    Script Date: 24-11-2017 09:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NOT NULL,
	[InvoiceNumber] [int] NOT NULL,
	[InvoiceDate] [datetime] NOT NULL,
	[Created] [datetime] NULL,
	[Updated] [datetime] NULL,
	[Description] [varchar](200) NULL,
	[dispatch_document_number] [varchar](100) NULL,
	[dispatched_through] [varchar](100) NULL,
	[destination] [varchar](150) NULL,
	[bale_numbers] [varchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TailorChalan]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TailorChalan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NOT NULL,
	[ChalanDate] [datetime] NOT NULL,
	[ChalanNo] [varchar](10) NULL,
	[Created] [datetime] NULL,
	[Updated] [datetime] NULL,
	[Description] [varchar](1) NULL,
	[IsGivenToTailor] [bit] NOT NULL,
	[bill_number] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TailorChalanDetails]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TailorChalanDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChalanId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [numeric](18, 2) NULL,
	[LaborCost] [numeric](18, 2) NULL,
	[Description] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TailorMaterialDetails]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TailorMaterialDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TailorChalanDetailsId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [numeric](18, 2) NULL,
	[Description] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tenant]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Address] [varchar](200) NULL,
	[Phone] [varchar](50) NULL,
	[Mobile] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[CurrentFinYear] [int] NULL,
 CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [numeric](18, 2) NULL,
	[ChalanNumber] [varchar](50) NULL,
	[BillNumber] [varchar](20) NULL,
	[Created] [datetime] NULL,
	[Updated] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](50) NULL,
	[Email] [varchar](50) NOT NULL,
	[Mobile] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[TenantId] [int] NOT NULL,
	[password_reset_token] [varchar](max) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendor]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorTypeId] [int] NULL,
	[VendorName] [varchar](200) NULL,
	[PhoneNumber] [varchar](15) NULL,
	[Address] [varchar](300) NULL,
	[Description] [varchar](200) NULL,
	[IsActive] [bit] NULL,
	[email] [varchar](100) NULL,
	[pan_number] [varchar](30) NULL,
	[gst_number] [varchar](30) NULL,
	[mobile] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VendorType]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorType] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChalanType] ON 

INSERT [dbo].[ChalanType] ([Id], [ChalanType], [Description], [IsInput]) VALUES (2, N'PrinterJobWork', NULL, 0)
INSERT [dbo].[ChalanType] ([Id], [ChalanType], [Description], [IsInput]) VALUES (3, N'PrinterFinishedProduct', NULL, 1)
INSERT [dbo].[ChalanType] ([Id], [ChalanType], [Description], [IsInput]) VALUES (4, N'PrinterShrinkage', NULL, 1)
SET IDENTITY_INSERT [dbo].[ChalanType] OFF
SET IDENTITY_INSERT [dbo].[FinancialYear] ON 

INSERT [dbo].[FinancialYear] ([Id], [Name], [StartDate], [EndDate]) VALUES (2, N'TestYear', CAST(N'2017-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2017-12-31T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[FinancialYear] OFF
SET IDENTITY_INSERT [dbo].[PrintJobWorkReceived] ON 

INSERT [dbo].[PrintJobWorkReceived] ([Id], [VendorId], [ChalanDate], [Created], [Updated], [Description], [chalan_number]) VALUES (1, 4, CAST(N'2017-11-23T00:00:00.000' AS DateTime), NULL, CAST(N'2017-11-23T21:43:39.520' AS DateTime), N'desc', 15)
INSERT [dbo].[PrintJobWorkReceived] ([Id], [VendorId], [ChalanDate], [Created], [Updated], [Description], [chalan_number]) VALUES (2, 4, CAST(N'2017-11-24T00:00:00.000' AS DateTime), CAST(N'2017-11-23T21:20:14.240' AS DateTime), CAST(N'2017-11-23T21:20:14.240' AS DateTime), NULL, 16)
SET IDENTITY_INSERT [dbo].[PrintJobWorkReceived] OFF
SET IDENTITY_INSERT [dbo].[PrintJobWorkReceivedDetails] ON 

INSERT [dbo].[PrintJobWorkReceivedDetails] ([Id], [ChalanId], [ProductId], [Quantity], [Fold], [Description]) VALUES (1, 1, 1, CAST(100.00 AS Numeric(18, 2)), 98, NULL)
INSERT [dbo].[PrintJobWorkReceivedDetails] ([Id], [ChalanId], [ProductId], [Quantity], [Fold], [Description]) VALUES (2, 2, 2, CAST(50.00 AS Numeric(18, 2)), 97, NULL)
SET IDENTITY_INSERT [dbo].[PrintJobWorkReceivedDetails] OFF
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([Id], [ProductTypeId], [ProductName], [Description], [IsActive]) VALUES (1, 1, N'Cloth Raw Material Test', N'Test one', 0)
INSERT [dbo].[Product] ([Id], [ProductTypeId], [ProductName], [Description], [IsActive]) VALUES (2, 1, N'Cotton Slub', NULL, 1)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[ProductType] ON 

INSERT [dbo].[ProductType] ([Id], [ProductType], [Description], [IsActive]) VALUES (1, N'Cloth', NULL, 1)
SET IDENTITY_INSERT [dbo].[ProductType] OFF
SET IDENTITY_INSERT [dbo].[PurchaseDetails] ON 

INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2, 2, 1, CAST(1.00 AS Numeric(18, 2)), 3, CAST(1.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (4, 3, 1, CAST(1.00 AS Numeric(18, 2)), 3, CAST(2.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (5, 3, 1, CAST(4.00 AS Numeric(18, 2)), 6, CAST(5.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2011, 2003, 1, CAST(34.00 AS Numeric(18, 2)), 56, CAST(45.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2012, 2003, 1, CAST(54.00 AS Numeric(18, 2)), 3, CAST(232.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2013, 2003, 1, CAST(34.00 AS Numeric(18, 2)), 56, CAST(45.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2014, 2003, 1, CAST(54.00 AS Numeric(18, 2)), 3, CAST(232.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2015, 2004, 1, CAST(4.00 AS Numeric(18, 2)), 5, CAST(3.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2016, 2004, 1, CAST(7.00 AS Numeric(18, 2)), 5, CAST(6.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2019, 2004, 1, CAST(12.00 AS Numeric(18, 2)), 3, CAST(12.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (2020, 2004, 1, CAST(32.00 AS Numeric(18, 2)), 4, CAST(3.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3011, 3003, 1, CAST(435.00 AS Numeric(18, 2)), 43, CAST(65.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3012, 2, 1, CAST(1.00 AS Numeric(18, 2)), 3, CAST(2.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3013, 3005, 1, CAST(10.00 AS Numeric(18, 2)), 5, CAST(5.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3014, 3005, 2, CAST(45.00 AS Numeric(18, 2)), 6, CAST(5.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3015, 3005, 2, CAST(458.00 AS Numeric(18, 2)), 8, CAST(6.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3016, 3005, 2, CAST(852.00 AS Numeric(18, 2)), 5, CAST(45.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3017, 3005, 2, CAST(48.00 AS Numeric(18, 2)), 25, CAST(56.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[PurchaseDetails] ([Id], [PurchaseId], [ProductId], [Quantity], [Fold], [Rate], [Description]) VALUES (3018, 3006, 1, CAST(1.00 AS Numeric(18, 2)), 1, CAST(1.00 AS Numeric(18, 2)), NULL)
SET IDENTITY_INSERT [dbo].[PurchaseDetails] OFF
SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON 

INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (2, 1, 12345, CAST(N'2017-10-26T00:00:00.000' AS DateTime), NULL, CAST(N'2017-10-22T22:56:04.547' AS DateTime), NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (3, 1, 5345234, CAST(N'2017-10-13T00:00:00.000' AS DateTime), CAST(N'2017-10-13T14:00:40.440' AS DateTime), CAST(N'2017-10-13T14:00:40.440' AS DateTime), NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (2003, 1, 455443, CAST(N'2017-10-18T00:00:00.000' AS DateTime), CAST(N'2017-10-17T10:01:02.957' AS DateTime), CAST(N'2017-10-17T10:01:02.957' AS DateTime), NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (2004, 1, 564543, CAST(N'2017-10-16T00:00:00.000' AS DateTime), NULL, CAST(N'2017-10-20T16:58:09.437' AS DateTime), NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (3003, 1, 43333335, CAST(N'2017-10-19T00:00:00.000' AS DateTime), CAST(N'2017-10-20T16:59:05.417' AS DateTime), CAST(N'2017-10-20T16:59:05.417' AS DateTime), NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (3004, 1, 1545, CAST(N'2017-11-18T00:00:00.000' AS DateTime), CAST(N'2017-11-18T09:30:05.213' AS DateTime), CAST(N'2017-11-18T09:30:05.213' AS DateTime), NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (3005, 1, 4555, CAST(N'2017-11-18T00:00:00.000' AS DateTime), NULL, CAST(N'2017-11-18T17:58:09.753' AS DateTime), N'test', N'777888', N'Courier', N'Jaipur', N'785,789,756')
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (3006, 1, 1, CAST(N'2017-11-15T00:00:00.000' AS DateTime), CAST(N'2017-11-20T04:55:50.513' AS DateTime), CAST(N'2017-11-20T04:55:50.513' AS DateTime), NULL, N'123', N'123', N'123', N'123')
INSERT [dbo].[PurchaseOrders] ([Id], [VendorId], [InvoiceNumber], [InvoiceDate], [Created], [Updated], [Description], [dispatch_document_number], [dispatched_through], [destination], [bale_numbers]) VALUES (3007, 2, 3123, CAST(N'2017-11-23T00:00:00.000' AS DateTime), CAST(N'2017-11-20T04:56:37.217' AS DateTime), CAST(N'2017-11-20T04:56:37.217' AS DateTime), N'Me', N'Me', N'Me', N'Me', N'Me')
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
SET IDENTITY_INSERT [dbo].[TailorChalan] ON 

INSERT [dbo].[TailorChalan] ([Id], [VendorId], [ChalanDate], [ChalanNo], [Created], [Updated], [Description], [IsGivenToTailor], [bill_number]) VALUES (2, 1, CAST(N'2017-11-03T00:00:00.000' AS DateTime), N'wrer43', CAST(N'2017-11-03T13:26:30.940' AS DateTime), CAST(N'2017-11-03T13:26:30.940' AS DateTime), NULL, 0, NULL)
SET IDENTITY_INSERT [dbo].[TailorChalan] OFF
SET IDENTITY_INSERT [dbo].[TailorChalanDetails] ON 

INSERT [dbo].[TailorChalanDetails] ([Id], [ChalanId], [ProductId], [Quantity], [LaborCost], [Description]) VALUES (3, 2, 1, CAST(43.00 AS Numeric(18, 2)), CAST(54.00 AS Numeric(18, 2)), NULL)
SET IDENTITY_INSERT [dbo].[TailorChalanDetails] OFF
SET IDENTITY_INSERT [dbo].[TailorMaterialDetails] ON 

INSERT [dbo].[TailorMaterialDetails] ([Id], [TailorChalanDetailsId], [ProductId], [Quantity], [Description]) VALUES (3, 3, 1, CAST(5.00 AS Numeric(18, 2)), NULL)
INSERT [dbo].[TailorMaterialDetails] ([Id], [TailorChalanDetailsId], [ProductId], [Quantity], [Description]) VALUES (4, 3, 1, CAST(6.00 AS Numeric(18, 2)), NULL)
SET IDENTITY_INSERT [dbo].[TailorMaterialDetails] OFF
SET IDENTITY_INSERT [dbo].[Tenant] ON 

INSERT [dbo].[Tenant] ([Id], [Name], [Address], [Phone], [Mobile], [City], [State], [Country], [CurrentFinYear]) VALUES (1, N'test', NULL, NULL, NULL, NULL, NULL, NULL, 2)
SET IDENTITY_INSERT [dbo].[Tenant] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [UserName], [Password], [Email], [Mobile], [Phone], [TenantId], [password_reset_token]) VALUES (4, N'Waleed', N'7c4a8d09ca3762af61e59520943dc26494f8941b', N'devwaleed@gmail.com', NULL, NULL, 1, N'')
INSERT [dbo].[User] ([UserId], [UserName], [Password], [Email], [Mobile], [Phone], [TenantId], [password_reset_token]) VALUES (5, N'ankur', N'c0f3fc9217bf9ef9ed628c02747bea7b2b3c80e7', N'ankurbhargava2000@gmail.com', N'09829085000', N'9829085000', 1, NULL)
INSERT [dbo].[User] ([UserId], [UserName], [Password], [Email], [Mobile], [Phone], [TenantId], [password_reset_token]) VALUES (6, N'yashi', N'afa9ca3be149447780c2f7b524d61ed3d3db63f9', N'ankur_bhargava2000@yahoo.co.in', N'09829085000', N'9829085000', 1, NULL)
INSERT [dbo].[User] ([UserId], [UserName], [Password], [Email], [Mobile], [Phone], [TenantId], [password_reset_token]) VALUES (7, N'saksham', N'01c80ef7164e7528965713b7f5a9c13235156f16', N'ankur.bhargava@metacube.com', N'09829085000', N'9829085000', 1, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[Vendor] ON 

INSERT [dbo].[Vendor] ([Id], [VendorTypeId], [VendorName], [PhoneNumber], [Address], [Description], [IsActive], [email], [pan_number], [gst_number], [mobile]) VALUES (1, 1, N'Ankur Bhargava', N'9829085000', N'J-145, Adarsh Nagar', N'Desc', NULL, N'ankurbhargava2000@gmail.com', N'556678676767', N'676767676', N'8989899999')
INSERT [dbo].[Vendor] ([Id], [VendorTypeId], [VendorName], [PhoneNumber], [Address], [Description], [IsActive], [email], [pan_number], [gst_number], [mobile]) VALUES (2, 1, N'Cloth Vendor', N'45415515422', N'test', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Vendor] ([Id], [VendorTypeId], [VendorName], [PhoneNumber], [Address], [Description], [IsActive], [email], [pan_number], [gst_number], [mobile]) VALUES (3, 1, N'Ankur Bhargava', N'9829085000', N'J-145, Adarsh Nagar', N'Test Desc', NULL, N'ankurbhargava2000@gmail.com', N'ABF45454545', N'121212122', N'9829085000')
INSERT [dbo].[Vendor] ([Id], [VendorTypeId], [VendorName], [PhoneNumber], [Address], [Description], [IsActive], [email], [pan_number], [gst_number], [mobile]) VALUES (4, 2, N'Ankur Bhargava', N'9829085000', N'J-145, Adarsh Nagar', NULL, NULL, N'ankurbhargava2000@gmail.com', N'AFS4545455', N'77787878', N'9829085000')
SET IDENTITY_INSERT [dbo].[Vendor] OFF
SET IDENTITY_INSERT [dbo].[VendorType] ON 

INSERT [dbo].[VendorType] ([Id], [VendorType], [Description], [IsActive]) VALUES (1, N'Raw Matarial Vendor', N'Who supplies raw material to company', 1)
INSERT [dbo].[VendorType] ([Id], [VendorType], [Description], [IsActive]) VALUES (2, N'Printer', N'Who do the color processing of cloths', 1)
INSERT [dbo].[VendorType] ([Id], [VendorType], [Description], [IsActive]) VALUES (3, N'Fabricator', N'Who create product from cloths', 1)
SET IDENTITY_INSERT [dbo].[VendorType] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__User__A9D10534725FE159]    Script Date: 24-11-2017 09:25:26 ******/
ALTER TABLE [dbo].[User] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__User__C9F2845688DA1058]    Script Date: 24-11-2017 09:25:26 ******/
ALTER TABLE [dbo].[User] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[InvoiceDetails] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[InvoiceMaster] ADD  DEFAULT ((0)) FOR [discount]
GO
ALTER TABLE [dbo].[InvoiceMaster] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD FOREIGN KEY([invoice_id])
REFERENCES [dbo].[InvoiceMaster] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceMaster]  WITH CHECK ADD FOREIGN KEY([created_by])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceMaster]  WITH CHECK ADD FOREIGN KEY([customer_id])
REFERENCES [dbo].[Vendor] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceMaster]  WITH CHECK ADD FOREIGN KEY([financial_year])
REFERENCES [dbo].[FinancialYear] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceMaster]  WITH CHECK ADD FOREIGN KEY([tenant_id])
REFERENCES [dbo].[Tenant] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrinterChalan]  WITH CHECK ADD FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[PrinterChalanDetails]  WITH CHECK ADD FOREIGN KEY([ChalanId])
REFERENCES [dbo].[PrinterChalan] ([Id])
GO
ALTER TABLE [dbo].[PrinterChalanDetails]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[PrintJobWorkReceived]  WITH CHECK ADD FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[PrintJobWorkReceivedDetails]  WITH CHECK ADD FOREIGN KEY([ChalanId])
REFERENCES [dbo].[PrintJobWorkReceived] ([Id])
GO
ALTER TABLE [dbo].[PrintJobWorkReceivedDetails]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([ProductTypeId])
REFERENCES [dbo].[ProductType] ([Id])
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD FOREIGN KEY([PurchaseId])
REFERENCES [dbo].[PurchaseOrders] ([Id])
GO
ALTER TABLE [dbo].[PurchaseOrders]  WITH CHECK ADD FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[TailorChalan]  WITH CHECK ADD FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[TailorChalanDetails]  WITH CHECK ADD FOREIGN KEY([ChalanId])
REFERENCES [dbo].[TailorChalan] ([Id])
GO
ALTER TABLE [dbo].[TailorChalanDetails]  WITH CHECK ADD FOREIGN KEY([ChalanId])
REFERENCES [dbo].[TailorChalan] ([Id])
GO
ALTER TABLE [dbo].[TailorChalanDetails]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[TailorMaterialDetails]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[TailorMaterialDetails]  WITH CHECK ADD FOREIGN KEY([TailorChalanDetailsId])
REFERENCES [dbo].[TailorChalanDetails] ([Id])
GO
ALTER TABLE [dbo].[Tenant]  WITH CHECK ADD  CONSTRAINT [FK_Tenant_FinancialYear] FOREIGN KEY([CurrentFinYear])
REFERENCES [dbo].[FinancialYear] ([Id])
GO
ALTER TABLE [dbo].[Tenant] CHECK CONSTRAINT [FK_Tenant_FinancialYear]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Tenant] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Tenant]
GO
ALTER TABLE [dbo].[Vendor]  WITH CHECK ADD FOREIGN KEY([VendorTypeId])
REFERENCES [dbo].[VendorType] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[USP_ProductWiseStock]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_ProductWiseStock]
AS
BEGIN

select P.Id, P.ProductName, 

(Select ISNULL(SUM(Quantity), 0) from PurchaseDetails where ProductId = P.id) as TotalPurchase,
(select ISNULL(SUM(quantity), 0) from PrinterChalanDetails PCD inner join PrinterChalan PC on PC.Id = PCD.ChalanId where ProductId = P.id ) as TotalProductGivenForPrinting,
(select ISNULL(SUM(quantity), 0) from PrinterChalanDetails PCD inner join PrinterChalan PC on PC.Id = PCD.ChalanId where ProductId = P.id ) as TotalReceived, 
(Select ISNULL(SUM(Shrinkage), 0) from PrinterChalanDetails where ProductId = P.id) as TotalShrinkage

from Product P

END
GO
/****** Object:  StoredProcedure [dbo].[USP_VendorWiseStock]    Script Date: 24-11-2017 09:25:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_VendorWiseStock]
AS
BEGIN

select V.Id, VendorName,
(SELECT ISNULL(SUM(Quantity), 0) from PrinterChalan PC left join PrinterChalanDetails PCD on PC.id = PCD.ChalanId where PC.VendorId = V.Id ) as GivenForPrinting, 
(SELECT ISNULL(SUM(Quantity), 0) from PrinterChalan PC left join PrinterChalanDetails PCD on PC.id = PCD.ChalanId where PC.VendorId = V.Id ) as ReceivedAfterPrinting, 
ISNULL(SUM(PCD.NetQuantity), 0) TotalNetQuantity, 
ISNULL(SUM(Shrinkage), 0) as TotalShrinkage
from Vendor V inner join PrinterChalan PC on V.id = PC.VendorId left join PrinterChalanDetails PCD on PC.id = PCD.ChalanId Group by  V.Id, VendorName

END
GO
USE [master]
GO
ALTER DATABASE [StockManager] SET  READ_WRITE 
GO
