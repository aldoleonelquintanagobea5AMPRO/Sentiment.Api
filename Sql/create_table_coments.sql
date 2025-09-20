IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'sentiment_db')
BEGIN
	CREATE DATABASE sentiment_db;
END
GO

USE sentiment_db;
GO

IF OBJECT_ID(N'dbo.Comments', N'U') IS NULL
BEGIN
	CREATE TABLE Comments
	(
	    id INT IDENTITY(1,1) PRIMARY KEY,
		product_id INT NOT NULL,
		user_id INT NOT NULL,
		comment_text NVARCHAR(MAX) NOT NULL,
		sentiment NVARCHAR(20) NOT NULL,
		created_at DATETIME2 NOT NULL DEFAULT (SYSUTCDATETIME())
	);
END
GO