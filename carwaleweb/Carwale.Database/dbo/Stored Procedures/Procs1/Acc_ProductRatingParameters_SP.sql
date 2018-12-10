IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_ProductRatingParameters_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_ProductRatingParameters_SP]
GO

	
CREATE PROCEDURE [dbo].[Acc_ProductRatingParameters_SP] 
	@ProductId		NUMERIC,
	@ParameterName	VARCHAR(100),
	@Status		NUMERIC OUTPUT
 AS
BEGIN
	INSERT INTO Acc_ProductRatingParameters(ProductId, ParameterName)  VALUES(@ProductId, @ParameterName)
	
	IF SCOPE_IDENTITY() > 0 Set  @Status = SCOPE_IDENTITY()
	ELSE SET @Status = 0
END
