IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockUploadedToCWCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockUploadedToCWCount]
GO

	 --=============================================
-- Author:                SURENDRA
-- Create date: 22ND OCT,2012
-- Description:        THIS PROCEDURE CHECKING UPLOADED CAR COUNT
-- =============================================
CREATE PROCEDURE TC_StockUploadedToCWCount
(
@StockIds VARCHAR(MAX)
)
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

   -- Insert statements for procedure here
       SELECT COUNT(id) AS NewCarCount FROM TC_Stock S WITH(NOLOCK) 
       WHERE S.IsSychronizedCW=0 AND S.Id in (SELECT listmember FROM [dbo].[fnSplitCSV](@StockIds)) AND S.StatusId=1
END
