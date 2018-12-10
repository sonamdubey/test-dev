IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerProperties]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerProperties]
GO

	-- =============================================
-- Author:		Rohan Sapkal	
-- Create date: 18-09-2014
-- Description:	Gets Properties and Current owner(s)
-- Modified By Rohan on 8-10-2014, Ordered by PackageTypeId and LAT LONG from NCD_Dealers
-- Exec [GetDealerProperties]
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerProperties] 
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
	  ,DN.Name AS DealerName
	  ,D.LastName
	  ,DN.Mobile as ActiveMaskingNumber
	  ,D.MobileNo
	  ,D.AreaId
	  ,A.Name as AreaName
	  ,NCDD.Lattitude AS Latitude
	  ,NCDD.Longitude
	  ,C.Name As CityName
	  
	  FROM
	  [dbo].[Impact_Slot] I WITH (NOLOCK)
	  INNER JOIN Dealer_NewCar DN WITH (NOLOCK) on I.DealerId=DN.Id
	  
	  INNER JOIN Dealers D WITH (NOLOCK) on DN.TcDealerId=D.ID
	  INNER JOIN NCD_Dealers NCDD WITH (NOLOCK) on NCDD.DealerId = I.DealerId
	  INNER JOIN Areas A WITH (NOLOCK) on D.AreaId=A.ID
	  INNER JOIN Cities C WITH (NOLOCK) ON I.CityId=C.Id
WHERE	I.IsActive=1
ORDER BY PackageTypeId ASC
END