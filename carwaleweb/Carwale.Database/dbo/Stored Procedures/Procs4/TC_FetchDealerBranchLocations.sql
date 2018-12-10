IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchDealerBranchLocations]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchDealerBranchLocations]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 14-10-2014
-- Description:	Fetching Branch locations of a dealer
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchDealerBranchLocations]
@BranchId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT          A.Id   AS AreaId, 
	                A.Name AS Area 
	FROM	        TC_DealerBranchLocations DBL WITH(NOLOCK)
    INNER JOIN   
			        Areas A WITH(NOLOCK)
	ON				DBL.AreaId = A.ID AND IsDeleted = 0
	WHERE			DBL.BranchId = @BranchId

END