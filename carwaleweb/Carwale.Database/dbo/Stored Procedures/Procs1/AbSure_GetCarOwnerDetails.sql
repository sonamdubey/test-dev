IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarOwnerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarOwnerDetails]
GO

	-- =============================================
-- Author:		Vinay Kumar Prajapati
-- Create date:  16th Feb 2015
-- Description:	Fetch Car Owner Details 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCarOwnerDetails] --610694
(
@StockId INT 
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     SELECT D.FirstName+ ' ' + D.LastName AS Name , D.MobileNo AS ContactNo, ISNULL(D.Address1,D.Address2) AS Address ,ISNULL(D.CityId,-1) AS  CityId,ISNULL(D.AreaId,-1) AS  AreaId
	 FROM  TC_Stock  AS ST WITH(NOLOCK) 
	 INNER JOIN Dealers AS D WITH(NOLOCK)  ON  D.ID=ST.BranchId
	 WHERE  ST.Id=@Stockid
END