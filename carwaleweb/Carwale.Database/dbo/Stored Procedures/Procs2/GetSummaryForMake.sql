IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSummaryForMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSummaryForMake]
GO

	-- =============================================
-- Author:		Shalini Nair
-- Create date: 26/11/14
-- Description:	Fetches the No.of Models and Segments of Models for a Car Make
-- =============================================
CREATE PROCEDURE [dbo].[GetSummaryForMake]
	-- Add the parameters for the stored procedure here
	@MakeId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) AS NoOfModels, UVW.Segment FROM
                         (SELECT	
                         DISTINCT CMO.Name, CS.Name AS Segment
                         FROM	
                         CarVersions CV  WITH(NOLOCK), CarModels CMO  WITH(NOLOCK), CarSegments CS  WITH(NOLOCK), NewCarSpecifications NS  WITH(NOLOCK)
                         WHERE
                         CMO.CarMakeId = @MakeId
                         AND CMO.IsDeleted = 0 AND CMO.New = 1
                         AND CV.CarModelId = CMO.ID
                         AND	CV.IsDeleted = 0 AND CV.New = 1
                         AND NS.CarVersionID=CV.ID 
                         AND CV.SegmentId = CS.ID) AS UVW
                         GROUP BY UVW.Segment
END

