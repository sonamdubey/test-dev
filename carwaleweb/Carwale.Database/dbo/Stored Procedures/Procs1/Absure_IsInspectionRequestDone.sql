IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_IsInspectionRequestDone]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_IsInspectionRequestDone]
GO

	


-- =============================================
-- Author:		Vinay Kumar
-- Create date: 11th Aug 2015
-- Description:	 Inspection request is done or not for specific stockId.. 
-- =============================================
CREATE PROCEDURE [dbo].[Absure_IsInspectionRequestDone]
	@StockId		    NUMERIC(18, 0),
	@Status				BIT = 1 OUTPUT
AS
BEGIN
	DECLARE @AbsureCarId BIGINT	=-1
	SET @Status = 1	
	SELECT @AbsureCarId =ISNULL(ACD.Id,-1) FROM AbSure_CarDetails AS ACD WITH(NOLOCK) WHERE StockId= @StockId 
	IF @AbsureCarId <> -1
		BEGIN		
			SET @Status = 1   -- inspection request already raised
		END
	ELSE
	    SET @Status = 0      --- No inspection request .. 
END

