IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOrUpdateLocalTax]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOrUpdateLocalTax]
GO

	-- =============================================
-- Author:		<Prashant Vishe>
-- Create date: <16 Oct 2013>
-- Description:	<For Inserting and updating local tax data>
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrUpdateLocalTax]
	-- Add the parameters for the stored procedure here
	@CityId INT
	,@CategoryItemid INT
	,@Rate FLOAT
	,@Description VARCHAR(500)
	,@IsTaxOnTax BIT
	,@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @count INT

	SET @count = (
			SELECT count(cityId)
			FROM PriceQuote_LocalTax
			WHERE CityId = @CityId
			)

	-- Insert statements for procedure here
	IF @Id = - 1
	BEGIN
		IF @count = 0
		BEGIN
			INSERT INTO PriceQuote_LocalTax (
				CityId
				,CategoryItemid
				,Rate
				,Description
				,IsTaxOnTax
				)
			VALUES (
				@CityId
				,@CategoryItemid
				,@Rate
				,@Description
				,@IsTaxOnTax
				)
		END
		ELSE
		BEGIN
			UPDATE PriceQuote_LocalTax
			SET CategoryItemid = @CategoryItemid
				,rate = @Rate
				,Description = @Description
				,IsTaxOnTax = @IsTaxOnTax
			WHERE CityId = @CityId
		END
	END
	ELSE
	BEGIN
		UPDATE PriceQuote_LocalTax
		SET CityId = @CityId
			,CategoryItemid = @CategoryItemid
			,rate = @Rate
			,Description = @Description
			,IsTaxOnTax = @IsTaxOnTax
		WHERE Id = @Id
	END
END