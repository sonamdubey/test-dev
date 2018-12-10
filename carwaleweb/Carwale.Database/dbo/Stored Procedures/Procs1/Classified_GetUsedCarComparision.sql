IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetUsedCarComparision]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetUsedCarComparision]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 20-12-2011
-- Description:	Used Car comparision feature and specification wise
-- [dbo].[Classified_GetUsedCarComparision] D9, S1
-- Modified By : Ashish G. Kamble on 11 Feb 2013
-- Added : CityName in select clause
-- =============================================
CREATE  PROCEDURE   [dbo].[Classified_GetUsedCarComparision]
	@Profile1 varchar(10),@Profile2 varchar(10),@Profile3 varchar(10) = null,@Profile4 varchar(10)=null
	-- This SP expects atleast 2 profile Ids for compaision
AS
BEGIN
	
	SET NOCOUNT ON;
    
    DECLARE @DealerId1 int,@DealerId2 int,@DealerId3 int,@DealerId4 int
    DECLARE @CustId1 int,@CustId2 int,@CustId3 int,@CustId4 int
    
	   -- Check if profile Id belongs to Dealer and if so get Dealer Profile Ids
	   IF   SUBSTRING(@profile1,1,1)='D'
			SET  @DealerId1=[dbo].[GetStringExceptFirst](@profile1)
	   ELSE SET  @DealerId1=0	
		
	  IF   SUBSTRING(@profile2,1,1)='D'
			SET  @DealerId2=[dbo].[GetStringExceptFirst](@profile2)
	   ELSE SET  @DealerId2=0
	   
	   IF   SUBSTRING(@profile3,1,1)='D'
			SET  @DealerId3=[dbo].[GetStringExceptFirst](@profile3)
	   ELSE SET  @DealerId3=0
	   
	   IF   SUBSTRING(@profile4,1,1)='D'
			SET  @DealerId4=[dbo].[GetStringExceptFirst](@profile4)
	   ELSE SET  @DealerId4=0
   
   --  Check if profile Id belongs to Individual Seller and if so get Individual Seller Profile Ids 
		IF   SUBSTRING(@profile1,1,1)='S'
			SET  @CustId1=[dbo].[GetStringExceptFirst](@profile1)
	   ELSE SET  @CustId1=0	
		
	  IF   SUBSTRING(@profile2,1,1)='S'
			SET  @CustId2=[dbo].[GetStringExceptFirst](@profile2)
	   ELSE SET  @CustId2=0
	   
	   IF   SUBSTRING(@profile3,1,1)='S'
			SET  @CustId3=[dbo].[GetStringExceptFirst](@profile3)
	   ELSE SET  @CustId3=0
	   
	   IF   SUBSTRING(@profile4,1,1)='S'
			SET  @CustId4=[dbo].[GetStringExceptFirst](@profile4)
	   ELSE SET  @CustId4=0
	
 SELECT  'D'+cast(id AS VARCHAR(10)) AS profileId,vw.Make,vw.Model,vw.Version,vw.VersionId, s.MakeYear,s.Price,s.Kilometers,NCS.FuelType,'Dealer' as Owner,
           cast (CSD.Owners as varchar(50)) as Owners,lower(s.Color) as Color,lower(CSD.RegistrationPlace) as RegistrationPlace,CSD.CityMileage,CSD.Insurance,CSD.InsuranceExpiry,CSD.OneTimeTax,NCS.NoOfCylinders,NCS.ValueMechanism,
           NCF.PassengerAirBags,NCF.DriverAirBags,NCF.ABS,NCF.Immobilizer,NCF.ChildSafetyLocks,NCF.TractionControl,NCF.PowerDoorLocks,
           NCF.PowerSeats, NCF.PowerSteering, NCF.PowerWindows,NCF.Defogger,NCF.CentralLocking,NCF.SteeringAdjustment,NCF.RemoteBootFuelLid,
           CSD.ACCondition,
           CSD.BatteryCondition,
           CSD.BrakesCondition,
           CSD.EngineCondition,
           CSD.ExteriorCondition,
           CSD.TyresCondition,
           S.Comments,
           CSD.InteriorCondition,CSD.SuspensionsCondition,
           LL.HostURL,
           LL.FrontImagePath,
           LL.CityId, LL.CityName
           
  FROM SellInquiries AS s 
       JOIN vwMMV as vw on vw.VersionId=s.CarVersionId
       JOIN LiveListings AS LL ON LL.Inquiryid = s.ID and LL.SellerType = 1
       LEFT JOIN SellInquiriesDetails as CSD on CSD.SellInquiryId=S.ID
       LEFT JOIN NewCarSpecifications as NCS on NCS.CarVersionId=s.CarVersionId
       LEFT JOIN NewCarStandardFeatures as NCF on NCF.CarVersionId=s.CarVersionId
  WHERE id in (@DealerId1,@DealerId2,@DealerId3,@DealerId4)	
  
  UNION ALL
  
   SELECT  'S'+cast(id AS VARCHAR(10)) AS profileId,vw.Make,vw.Model,vw.Version,vw.VersionId,s.MakeYear,s.Price,s.Kilometers,NCS.FuelType,'Individual' as Owner,
           cast (CSD.Owners as varchar(50)) as Owners,lower(s.Color) as Color,lower(CSD.RegistrationPlace) as RegistrationPlace,CSD.CityMileage,CSD.Insurance,CSD.InsuranceExpiry,CSD.Tax,NCS.NoOfCylinders,NCS.ValueMechanism,
           NCF.PassengerAirBags,NCF.DriverAirBags,NCF.ABS,NCF.Immobilizer,NCF.ChildSafetyLocks,NCF.TractionControl,NCF.PowerDoorLocks,
           NCF.PowerSeats, NCF.PowerSteering,NCF.PowerWindows,NCF.Defogger,NCF.CentralLocking,NCF.SteeringAdjustment,NCF.RemoteBootFuelLid,
           CSD.ACCondition,
           CSD.BatteryCondition,
           CSD.BrakesCondition,
           CSD.EngineCondition,
           CSD.ExteriorCondition,
           CSD.TyresCondition,
           S.Comments,
           CSD.InteriorCondition,CSD.SuspensionsCondition,
           LL.HostURL,
           LL.FrontImagePath,
           LL.CityId, LL.CityName
           
  FROM CustomerSellInquiries AS s 
     JOIN vwMMV as vw on vw.VersionId=s.CarVersionId
     JOIN LiveListings AS LL ON LL.Inquiryid = s.ID and LL.SellerType = 2
     LEFT JOIN CustomerSellInquiryDetails as CSD on CSD.InquiryId=S.ID
     LEFT JOIN NewCarSpecifications as NCS on NCS.CarVersionId=s.CarVersionId
     LEFT JOIN NewCarStandardFeatures as NCF on NCF.CarVersionId=s.CarVersionId
  WHERE id in (@CustId1,@CustId2,@CustId3,@CustId4)	
  
		
END
