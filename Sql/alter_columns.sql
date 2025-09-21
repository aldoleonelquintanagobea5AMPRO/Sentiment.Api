USE sentiment_db;
GO

ALTER TABLE comments ALTER COLUMN product_id NVARCHAR(100);
ALTER TABLE comments ALTER COLUMN user_id NVARCHAR(100);

SELECT * FROM Comments;