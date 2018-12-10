IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOrUpdatePQItems]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOrUpdatePQItems]
GO
	
-- ============================================= 
-- Author: Kumar Vikram    
-- Create date:30/10/2013
-- Description:for inserting and updating pq items data...   
-- EXEC [InsertorUpdatePQItems] -1,5,-1,'Insurance'
-- Modified by Kumar Vikram on <2/10/2014> : Changed the logic according to category wise
-- ============================================= 
CREATE PROCEDURE [dbo].[InsertOrUpdatePQItems]
	-- Add the parameters for the stored procedure here 
	@Id INT
	,@CategoryId TINYINT
	,@MakeId INT
	,@ItemName VARCHAR(200)
AS
BEGIN
	DECLARE @ItemId INT
		,@CategoryitemId INT
		,@CategoryId_Insurance INT = 5
		,@CategoryId_Dealers INT = 6
		,@CategoryId_LocalTax INT = 2

	-- SET NOCOUNT ON added to prevent extra result sets from 
	-- interfering with SELECT statements. 
	SET NOCOUNT ON;

	-- Insert and Update statements for procedure here 
	SELECT @ItemId = Id
	FROM pq_categoryitems with (nolock)
	WHERE CategoryName = @ItemName
		AND CategoryId = @CategoryId

	IF @Id <= 0
	BEGIN
		IF (@ItemId IS NULL)
		BEGIN
			INSERT INTO pq_categoryitems
			VALUES (@CategoryId,@ItemName)

			SET @CategoryitemId = @@IDENTITY

			IF (@CategoryId = @CategoryId_Insurance AND @MakeId <> - 1)
			BEGIN
				INSERT INTO PriceQuote_Insurances
				VALUES (@MakeId,@CategoryitemId,1,0)
			END
			ELSE
				IF (@CategoryId = @CategoryId_Insurance AND @MakeId = - 1)
				BEGIN
					INSERT INTO PriceQuote_Insurances
					VALUES (@MakeId,@CategoryitemId,1,1)
				END
				ELSE
					IF (@CategoryId = @CategoryId_Dealers)
					BEGIN
						INSERT INTO PriceQuote_DealerCharges
						VALUES (@CategoryitemId,1)
					END
		END
	END
	ELSE
	BEGIN
		UPDATE pq_categoryitems
		SET categoryname = @ItemName
		WHERE (
				SELECT COUNT(*)
				FROM PQ_CategoryItems
				WHERE categoryname = @ItemName
				) = 0
			AND id = @Id
	END
END
/****** Object:  StoredProcedure [dbo].[NC_AddDealer]    Script Date: 2/25/2014 6:10:05 PM ******/
SET ANSI_NULLS OFF
