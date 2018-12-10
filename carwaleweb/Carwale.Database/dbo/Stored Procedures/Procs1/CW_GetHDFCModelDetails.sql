IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetHDFCModelDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetHDFCModelDetails]
GO

	CREATE PROCEDURE [dbo].[CW_GetHDFCModelDetails]
@MakeId AS Numeric(18,0)=NULL
AS
--Author: Rakesh Yadav On 02 Aug 2015
--Desc: fetch model details with segment and tier
BEGIN
	SELECT VM.Make+' '+VM.Model AS CarName,CMD.CarModelId AS ModelId,CMD.CW_CarSegmentId AS CarSegmentId,
	CS.Name+':'+CS.Segments AS CarSegment, CMD.CW_CarTierId AS CarTierId,CT.Name AS CarTier,VM.MakeId,CMD.IsActive	
	FROM 
	CW_CarModelDetails CMD
	JOIN CW_CarSegments CS ON CMD.CW_CarSegmentId=CS.Id
	JOIN CW_CarTiers CT ON CMD.CW_CarTierId=CT.Id
	JOIN vwMMV VM ON VM.ModelId=CMD.CarModelId
	WHERE @MakeId IS NULL OR VM.MakeId=@MakeId
	GROUP BY VM.Make,VM.Model ,CMD.CarModelId ,CMD.CW_CarSegmentId ,
	CS.Name,CS.Segments , CMD.CW_CarTierId ,CT.Name ,VM.MakeId,CMD.IsActive
END
