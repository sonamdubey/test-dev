IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_FetchNewCarRoi_16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_FetchNewCarRoi_16_9_1]
GO

	
-- =============================================
-- Author:		Piyush Sahu
-- Create date: 9/16/2016
-- Description:	Removed CityCategoryId parameter
-- =============================================

CREATE Procedure [dbo].[CW_FetchNewCarRoi_16_9_1]
@CarSegmentId INT
AS
BEGIN
SELECT DISTINCT [1] AS ROI1,[2] AS ROI2,[3] AS ROI3,[4] AS ROI4,[5] AS ROI5,[6] AS ROI6,[7] AS ROI7,IsActive 
FROM
 (SELECT ROI,Tenor,IsActive FROM CW_NewCarROI WITH( NOLOCK) WHERE CW_CarSegmentId=@CarSegmentId
  )A PIVOT (Max(ROI) FOR Tenor  IN ([1],[2],[3],[4],[5],[6],[7]) 
 )
  AS pvt
END

