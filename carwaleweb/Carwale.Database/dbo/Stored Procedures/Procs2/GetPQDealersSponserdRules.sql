IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetPQDealersSponserdRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetPQDealersSponserdRules]
GO

	-- =============================================
-- Author: Chetan Thambad   
-- Create date:06/10/2015
-- Description:for Getting pq dealers data
-- EXEC GetPQDealersSponserdRules 4413,18
-- =============================================
CREATE PROCEDURE [dbo].[GetPQDealersSponserdRules] 

     @CampaignId int,
     @MakeId int=null,
     @ModelId VARCHAR(MAX)=null,
     @StateId int=null,
     @CityId VARCHAR(MAX)=null
     AS
             BEGIN
				SELECT PDS.ID AS PqId,DCM.ID AS Id,DCM.CityId,DCM.ModelId,DCM.ZoneId,S.ID AS StateId,CMK.ID AS MakeId,CMK.Name AS Makename, 
				CASE DCM.StateId when -1 THEN 'All States' ELSE s.Name END AS StateName,
				CASE DCM.CityId WHEN -1 THEN 'All Cities' ELSE C.Name END AS CityName,  
				CASE DCM.ModelId when -1 THEN 'All Models' ELSE CM.Name END AS ModelName, 
				CASE DCM.ZoneId when -1 THEN 'All Zones' ELSE CZ.ZoneName END AS ZoneName
				FROM PQ_DealerCitiesModels DCM WITH(NOLOCK)
				INNER JOIN PQ_DealerSponsored PDS WITH(NOLOCK) ON PDS.ID = DCM.CampaignId
				LEFT JOIN CarModels CM WITH (NOLOCK) ON CM.ID = DCM.ModelId 
				LEFT JOIN CarMakes CMK WITH(NOLOCK) ON CMK.ID = DCM.MakeId  
				LEFT JOIN Cities C WITH (NOLOCK) ON C.ID = DCM.CityId  
				LEFT JOIN States S WITH(NOLOCK) ON S.ID = DCM.StateId OR C.StateId = S.ID  
				LEFT JOIN CityZones CZ WITH (NOLOCK) ON CZ.Id = DCM.ZoneId 
				WHERE DCM.CampaignId = @CampaignId AND ((DCM.MakeId = @MakeId AND @MakeId IS NOT NULL) OR @MakeId IS NULL) 
				AND (((DCM.ModelId IN(SELECT items FROM [SplitText](@ModelId, ',')) AND @ModelId IS NOT NULL) OR @ModelId IS NULL) OR (DCM.ModelId = -1 and @MakeId != -1))
				AND ((((S.Id = @StateId AND @StateId IS NOT NULL) OR @StateId IS NULL) and DCM.StateId != -1) or DCM.StateId =-1)
				AND (((DCM.CityId IN(SELECT items FROM [SplitText](@CityId, ',')) AND @CityId IS NOT NULL) OR @CityId IS NULL)OR (DCM.CityId = -1 and @StateId != -1))  ORDER BY PDS.Id ;
				END

