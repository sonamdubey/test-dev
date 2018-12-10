IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Impact_SearchDealerAdvertisement]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Impact_SearchDealerAdvertisement]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 19th Sept 2014
-- Description : Search Result From Impact_Slot for advertisement 
-- =============================================

CREATE PROCEDURE  [dbo].[Impact_SearchDealerAdvertisement]
    (
	@MakeId              INT,
	@CityId              INT
	)
 AS

   BEGIN
		SELECT  ICN.Impact_CampaignId AS ID, CM.Name AS CarMake , C.Name AS City ,DNC.Name AS DealerName,P.Name AS Package ,
		    ISNULL(ICN.StartDate,'') AS StartDate,ISNULL(ICN.EndDate,'') AS EndDate ,ICN.IsActive
		FROM Impact_Campaign AS ICN WITH(NOLOCK) 
		    LEFT JOIN Impact_Slot AS IPS WITH(NOLOCK) ON IPS.Impact_CampaignId=ICN.Impact_CampaignId
			LEFT JOIN  CarMakes AS CM WITH(NOLOCK) ON CM.ID=ICN.MakeId
			LEFT JOIN Cities AS C WITH(NOLOCK) ON C.ID=ICN.CityId
			LEFT JOIN Dealer_NewCar AS DNC WITH(NOLOCK) ON DNC.Id=ICN.DealerId
			LEFT JOIN Packages AS P WITH(NOLOCK) ON P.Id=ICN.PackageTypeId
		WHERE ICN.MakeId=@MakeId AND  ICN.CityId=@CityId ORDER BY ICN.IsActive DESC
   END

