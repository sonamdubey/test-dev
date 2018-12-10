IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerProperties_Api]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerProperties_Api]
GO

	CREATE PROCEDURE [dbo].[GetDealerProperties_Api] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT [Impact_SlotId]
      ,I.[MakeId]
      ,I.[CityId]
      ,D.ID as DealerId
      ,[PackageTypeId]
      ,[Impact_CampaignId] as ImpactCampaignId
	  ,D.FirstName
	  ,D.Organization AS DealerName
	  ,D.LastName
	  ,D.PhoneNo as ActiveMaskingNumber
	  ,D.MobileNo
	  ,D.AreaId
	  ,A.Name as AreaName
	  ,D.Lattitude as Latitude --Should be 'NCD.Lattitude as Latitude' on live and staging
	  ,D.Longitude --SHOULD BE 'NCD.Longitude' on live and staging
	  ,C.Name As CityName
	  
	  FROM
	  [dbo].[Impact_Slot] I WITH (NOLOCK)
	  --INNER JOIN Dealer_NewCar DN WITH (NOLOCK) on I.DealerId=DN.Id
	  
	  INNER JOIN Dealers D WITH (NOLOCK) on I.DealerId=D.ID
	  --10/15/2014
	  --INNER JOIN NCD_Dealers NCDD WITH (NOLOCK) on NCDD.DealerId = I.DealerId //COMMENTED OUT ONLY ON LOCAL DUE TO DATA INCONSISTENCY
	  --SHOULD NOT BE COMMENTED OUT ON STAGING OR LIVE
	  INNER JOIN Areas A WITH (NOLOCK) on D.AreaId=A.ID
	  INNER JOIN Cities C WITH (NOLOCK) ON I.CityId=C.Id
WHERE	I.IsActive=1
ORDER BY PackageTypeId ASC
END
