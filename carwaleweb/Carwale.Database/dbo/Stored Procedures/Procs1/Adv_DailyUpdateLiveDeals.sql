IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Adv_DailyUpdateLiveDeals]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Adv_DailyUpdateLiveDeals]
GO

	-- =============================================
-- Author:		Purohith Guguloth	
-- Create date: 13th Oct 2016
-- Description:	To update the liveDeals table if there is change in carmodels details
-- Modifier:    Saket on 8th Nov 2016, added update for RootId, SubsegmentId, HostURL, OriginalImgPath
-- =============================================
CREATE PROCEDURE [dbo].[Adv_DailyUpdateLiveDeals] 
AS
BEGIN

	SET NOCOUNT ON;
	
	CREATE TABLE #TempModifiedModels (ModelId INT)
	
	INSERT INTO #TempModifiedModels (ModelId)  
		(
		     SELECT DISTINCT AffectedId 
			 FROM CarWaleMasterDataLogs WITH(NOLOCK)
			 WHERE TableName='CarModels' 
			 AND   DATEADD(DAY, -1, GETDATE()) < CreatedOn 
			 AND   (ColumnName = 'MaskingName' OR ColumnName = 'Name' OR ColumnName = 'SubSegmentId' OR ColumnName = 'HostURL' OR ColumnName = 'OriginalImgPath' OR ColumnName = 'RootId')
			 AND   Remarks ='Record Updated'
			 AND   OldValue<>NewValue
		)
	     
	
	UPDATE LD
	SET
		LD.MaskingName = CM.MaskingName,
		LD.Model = CM.Name,
		LD.HostURL = CM.HostURL,
		LD.OriginalImgPath = CM.OriginalImgPath,
		LD.SubSegmentId = CM.SubSegmentId,
		LD.RootId = CM.RootId
	FROM
		LiveDeals AS LD With(NoLock)
		INNER JOIN #TempModifiedModels AS TMM With(NoLock) ON LD.ModelId = TMM.ModelId
		INNER JOIN CarModels AS CM With(NoLock) ON CM.ID = TMM.ModelId
	WHERE LD.ModelId = TMM.ModelId
	
	DROP TABLE #TempModifiedModels
END
