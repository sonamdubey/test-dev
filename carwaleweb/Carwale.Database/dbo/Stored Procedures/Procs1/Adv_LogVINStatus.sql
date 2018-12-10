IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Adv_LogVINStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Adv_LogVINStatus]
GO

	
-- =============================================
-- Author:		Saket Thapliyal
-- Create date: 11th Aug 2016
-- Description:	Log VIN status in TC_Deals_StockVINLog
-- =============================================
CREATE PROCEDURE [dbo].[Adv_LogVINStatus] 
	@LogString VARCHAR(MAX),
	@FirstDelimit char(1),
	@SecondDelimit char(1),
	@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO TC_Deals_StockVINLog
	(TC_Deals_StockVINId, TC_Deals_StockStatusId, VINNo , PreviousStatus, ModifiedOn, ModifiedBy)
	(
		SELECT *,GETDATE(),@UserId 
		FROM [dbo].[SplitTextByTwoDelimitersWithFourColumns](@LogString, @FirstDelimit, @SecondDelimit)
	)

END

