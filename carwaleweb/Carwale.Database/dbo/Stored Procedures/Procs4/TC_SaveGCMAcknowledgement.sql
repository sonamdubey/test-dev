IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveGCMAcknowledgement]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveGCMAcknowledgement]
GO

	-- =============================================
-- Author:	Nilesh Utture
-- Create date: Create Date, 16th May, 2013
-- Description:	Description,save response from GCM server
-- Modified By : Afrose on 07-12-2015, Added @Type, changed condition to insert multiple values
--EXEC TC_SaveGCMAcknowledgement 'Ok','89865,655478,965124,1024','Data',2
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveGCMAcknowledgement]
	-- Add the parameters for the stored procedure here
	@Message VARCHAR(500),
	@InquiryId VARCHAR(100),
	@Postdata VARCHAR(2000),
	@Type TINYINT=1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	/*DECLARE  @TblStockList  TABLE (TblStockListId INT IDENTITY(1,1) ,TC_StockId INT)

	INSERT INTO @TblStockList  (TC_StockId)
	SELECT ListMember FROM [dbo].[fnSplitCSV] (@InquiryId)*/
	
	--INSERT INTO TC_GCMAcknownledgements(InqId,Message,SentDate,PostData,Type) 
	--SELECT  ListMember, @Message, GETDATE(), @Postdata,@Type 
 --   FROM [dbo].[fnSplitCSV] (@InquiryId);
        
	--DECLARE  @TotalWhileLoop INT
	--DECLARE @WhileLoopControl INT =1
	--DECLARE @Id INT

	--SELECT @TotalWhileLoop=count (*) FROM @TblStockList
	--WHILE @WhileLoopControl<=@TotalWhileLoop
	--BEGIN
		
	--	SELECT @Id= TC_StockId FROM @TblStockList WHERE TblStockListId=@WhileLoopControl
	--	INSERT INTO TC_GCMAcknownledgements(InqId,Message,SentDate,PostData,Type) VALUES (@Id, @Message, GETDATE(), @Postdata,@Type)

	--	SET @WhileLoopControl= @WhileLoopControl+1
	--END

	
END

