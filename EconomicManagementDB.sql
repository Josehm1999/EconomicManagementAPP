USE [master]
GO
/****** Object:  Database [EconomicManagementDBv2]    Script Date: 24/08/2022 09:04:02 ******/
CREATE DATABASE [EconomicManagementDBv2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EconomicManagementDBv2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EconomicManagementDBv2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EconomicManagementDBv2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EconomicManagementDBv2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [EconomicManagementDBv2] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EconomicManagementDBv2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EconomicManagementDBv2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET ARITHABORT OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EconomicManagementDBv2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EconomicManagementDBv2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EconomicManagementDBv2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EconomicManagementDBv2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET RECOVERY FULL 
GO
ALTER DATABASE [EconomicManagementDBv2] SET  MULTI_USER 
GO
ALTER DATABASE [EconomicManagementDBv2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EconomicManagementDBv2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EconomicManagementDBv2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EconomicManagementDBv2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EconomicManagementDBv2] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EconomicManagementDBv2] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'EconomicManagementDBv2', N'ON'
GO
ALTER DATABASE [EconomicManagementDBv2] SET QUERY_STORE = OFF
GO
USE [EconomicManagementDBv2]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountTypes]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderAccount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[OperationTypeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationTypes]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[AccountId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[StandarEmail] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_AccounTypes_Cascade_Delete] FOREIGN KEY([AccountTypeId])
REFERENCES [dbo].[AccountTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_AccounTypes_Cascade_Delete]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_AccountType] FOREIGN KEY([AccountTypeId])
REFERENCES [dbo].[AccountTypes] ([Id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_AccountType]
GO
ALTER TABLE [dbo].[AccountTypes]  WITH CHECK ADD  CONSTRAINT [FK_AccountTypes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[AccountTypes] CHECK CONSTRAINT [FK_AccountTypes_Users]
GO
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_Operations] FOREIGN KEY([OperationTypeId])
REFERENCES [dbo].[OperationTypes] ([Id])
GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_Operations]
GO
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_Users]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Account_Cascade_Delete] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Account_Cascade_Delete]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Categories_Cascade_Delete] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Categories_Cascade_Delete]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Users]
GO
/****** Object:  StoredProcedure [dbo].[AccountTypes_Insert]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AccountTypes_Insert]
	@Name nvarchar(50),
	@UserId int
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @OrderAccount int;
	SELECT @OrderAccount = COALESCE(MAX(OrderAccount),0)+1
	FROM AccountTypes
	WHERE UserId = @UserId

	INSERT INTO AccountTypes(Name, UserId, OrderAccount)
	VALUES (@Name, @UserId, @OrderAccount);

	SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Transaction_Delete]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Transaction_Delete]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Total decimal(18,2);
	DECLARE @AccountId int;
	DECLARE @OperationTypeId int;

	SELECT @Total = Total, @AccountId = AccountId, @OperationTypeId = cate.OperationTypeId
	FROM Transactions
	INNER JOIN Categories cate
	ON cate.Id = Transactions.CategoryId
	WHERE Transactions.Id = @Id;

	DECLARE @MultiplicativeFactor int = 1;

	IF (@OperationTypeId = 2)
		SET @MultiplicativeFactor = -1;

	SET @Total = @Total * @MultiplicativeFactor;

	UPDATE Accounts
	SET Balance -= @Total
	WHERE Id = @AccountId;

	DELETE Transactions
	WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Transactions_Insert]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Transactions_Insert]
	@UserId int,
	@TransactionDate date,
	@Total decimal(18,2),
  @CategoryId int,
  @AccountId int,
	@Description nvarchar(1000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Transactions(UserId, TransactionDate, Total, CategoryId, AccountId, Description)
	VALUES(@UserId, @TransactionDate, ABS(@Total), @CategoryId, @AccountId, @Description)

  UPDATE Accounts
  SET Balance += @Total
  WHERE Id = @AccountId;

  SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Transactions_Update]    Script Date: 24/08/2022 09:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Transactions_Update]
	@Id int,
	@TransactionDate datetime,
	@Total decimal(18,2),
	@PreviousTotal decimal(18,2),
	@AccountId int,
	@PreviousAccountId int,
	@CategoryId int,
	@Description nvarchar(1000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Reverse previous transaction
	UPDATE Accounts
	SET Balance -= @PreviousTotal
	WHERE Id = @PreviousAccountId;

	-- Do a new transaction
	UPDATE Accounts
	SET Balance += @Total
	WHERE Id = @AccountId;

	UPDATE Transactions
	SET Total = ABS(@Total), TransactionDate = @TransactionDate,
	CategoryId = @CategoryId, AccountId = @AccountId, Description = @Description
	WHERE Id = @Id;
END
GO
USE [master]
GO
ALTER DATABASE [EconomicManagementDBv2] SET  READ_WRITE 
GO
