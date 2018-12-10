IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetConversionCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetConversionCode]
GO

	
-- =============================================
-- Author:		Vikas
-- Create date: 26/10/2012
-- Description:	To get the conversion values for a selected model for the purpose of tracking.
-- =============================================
CREATE PROCEDURE [dbo].[GetConversionCode] 
	-- Add the parameters for the stored procedure here
	@ModelId NUMERIC, 
	@BuyTimeDays INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @iModelId INT
    SELECT @iModelId = ModelId From PQConversionCodes Where ModelId = @ModelId
    
	DECLARE @BuyCode CHAR(2)
	SELECT @BuyCode = CASE @BuyTimeDays WHEN 90 THEN 'JR' ELSE 'PQ' END 
	
	IF (ISNULL(@iModelId,0)=0)
		BEGIN	
			SELECT ConversionId, Label, Value FROM PQConversionCodes WHERE ModelId = -1 and BuyCode = @BuyCode 
		END
	ELSE
		BEGIN
			SELECT ConversionId, Label, Value FROM PQConversionCodes WHERE ModelId = @ModelId and BuyCode = @BuyCode		
		END
END

