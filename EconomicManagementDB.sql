CREATE DATABASE [EconomicManagementDBv2]
GO
USE [EconomicManagementDBv2]
GO

CREATE TABLE [Users](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[StandarEmail] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
)
GO

INSERT INTO Users (Email,StandarEmail,Password) VALUES ('test1@gmail.com', 'TEST1@GMAIL.COM', '12345');
CREATE TABLE [AccountTypes](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderAccount] [int] NOT NULL,
	CONSTRAINT [FK_AccountTypes_Users] FOREIGN KEY (UserId) REFERENCES Users(Id)
)
GO
INSERT INTO AccountTypes  (Name, UserId, OrderAccount) VALUES ('Credit', 1, 1);

CREATE TABLE [Accounts](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1000) NULL,
    CONSTRAINT [FK_AccountType] FOREIGN KEY (AccountTypeId) REFERENCES AccountTypes(Id)
)
GO
INSERT INTO Accounts (Name, AccountTypeId, Balance, Description) VALUES ('Credit 1', 1, 150.80, 'Account with $1500 worth of credit');

CREATE TABLE [OperationTypes](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
)
GO
INSERT INTO OperationTypes (Description) VALUES ('Income'), ('Expenses');

CREATE TABLE Categories(
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[OperationTypeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
  CONSTRAINT [FK_Categories_Operations] FOREIGN KEY (OperationTypeId) REFERENCES OperationTypes(Id),
	CONSTRAINT [FK_Categories_Users] FOREIGN KEY (UserId) REFERENCES Users(Id)
)
GO

CREATE TABLE [Transactions](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[AccountId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	CONSTRAINT [FK_Transactions_Users] FOREIGN KEY (UserId) REFERENCES Users(Id),
	CONSTRAINT [FK_Transactions_Account] FOREIGN KEY (AccountId) REFERENCES Accounts(Id),
	CONSTRAINT [FK_Transactions_Categories] FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
)
GO

CREATE PROCEDURE Transactions_Insert
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

CREATE PROCEDURE Transactions_Update
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

CREATE PROCEDURE Transaction_Delete
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

CREATE PROCEDURE AccountTypes_Insert
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

CREATE PROCEDURE Categorie_Delete
@id INT
AS
BEGIN
	SET NOCOUNT ON;
		IF EXISTS (SELECT 1 FROM Transactions WHERE CategoryId = @id )
		BEGIN
			DELETE FROM Transactions WHERE CategoryId = @id
		END
		DELETE FROM Categories WHERE Id = @id
END
GO

CREATE PROCEDURE Accounts_Delete
@id INT
AS
BEGIN
SET NOCOUNT ON;
		IF EXISTS (SELECT 1 FROM Transactions WHERE AccountId = @id )
		BEGIN
			DELETE FROM Transactions WHERE AccountId = @id
		END
		DELETE FROM Accounts WHERE Id = @id
END
GO

CREATE PROCEDURE AccountTypes_Delete
@id int
AS
BEGIN
SET NOCOUNT ON;
		IF EXISTS (SELECT 1 FROM Transactions t
			INNER JOIN Accounts a ON t.AccountId = a.Id
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			WHERE at.Id = @id )
		BEGIN
			DELETE Transactions
			FROM Transactions t
			INNER JOIN Accounts a ON t.AccountId = a.Id
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			WHERE at.Id = @id;
		END
		IF EXISTS (SELECT 1 FROM Accounts a
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			WHERE at.Id=@id)
		BEGIN
			DELETE Accounts FROM Accounts a
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			WHERE at.Id=@id
		END
		DELETE FROM AccountTypes
		WHERE Id = @id;
END
GO

CREATE PROCEDURE Users_Delete
@id int
AS
BEGIN
SET NOCOUNT ON;
		If exists (SELECT 1 FROM Transactions t
			INNER JOIN Accounts a ON t.AccountId = a.Id
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			INNER JOIN Users u ON u.Id = at.UserId
			WHERE u.Id = @id )
		BEGIN
			DELETE Transactions
			FROM Transactions t
			INNER JOIN Accounts a ON t.AccountId = a.Id
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			INNER JOIN Users u ON u.Id = at.UserId
			WHERE u.Id = @id;
		END
		IF EXISTS (SELECT 1 FROM Accounts a
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			INNER JOIN Users u ON u.Id = at.UserId
			WHERE u.Id = @id)
		BEGIN
			DELETE Accounts FROM Accounts a
			INNER JOIN AccountTypes at ON at.Id = a.AccountTypeId
			INNER JOIN Users u ON u.Id = at.UserId
			WHERE u.Id = @id;
		END
		IF EXISTS (SELECT 1 FROM AccountTypes at
			INNER JOIN Users u ON u.Id = at.UserId
			WHERE u.Id = @id)
		BEGIN
			DELETE FROM AccountTypes
			WHERE UserId = @id;
		END
		IF EXISTS (SELECT 1 FROM Categories c
				INNER JOIN Users u ON u.Id = c.UserId
				WHERE u.Id = @id)
		BEGIN
			DELETE FROM Categories
			WHERE UserId = @id;
		END
		DELETE FROM Users WHERE Id = @id
END
GO
