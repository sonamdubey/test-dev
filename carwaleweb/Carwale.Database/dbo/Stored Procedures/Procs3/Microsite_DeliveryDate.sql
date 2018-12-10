IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DeliveryDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DeliveryDate]
GO

	CREATE PROCEDURE [dbo].[Microsite_DeliveryDate]
@VersionId numeric(18,0),
@DealerId numeric(18,0),
@CityId numeric(18,0) = null
--Author:Rakesh Yadav
--Date Created: 22 April 2015
--Desc: Fetch expected delivery date for version in city for perticullar 
AS 
BEGIN

SELECT BookingAmount,VersionId,DeliveryTime,DealerId,CityId 
FROM 
Microsite_DeliveryTime WITH(NOLOCK)
WHERE DealerId=@DealerId AND VersionId=@VersionId 
AND (@CityId IS NULL OR CityId=@CityId)

END