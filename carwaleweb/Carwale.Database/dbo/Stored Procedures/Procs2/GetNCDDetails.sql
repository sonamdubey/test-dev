IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCDDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCDDetails]
GO

	-- =============================================          
-- Author:  <Vinayak>          
-- Description: <Get the dealer detail based on dealerId>
-- Modified By Vikas J on 12/03/15 added order by clause
-- =============================================          
CREATE PROCEDURE [dbo].[GetNCDDetails](
@DealerId int,
--OUTPUT--
@MakeId int output,
@CityId int output,
@Name varchar(200) output,
@Address varchar(400) output,
@Pincode varchar(20) output,
@ContactNo varchar(100) output,
@FaxNo varchar(30) output,
@EMailId varchar(100) output,
@WebSite varchar(100) output,
@WorkingHours varchar(50) output,
@HostURL varchar(100) output,
@ContactPerson varchar(100) output,
@DealerMobileNo varchar(100) output,
@Mobile varchar(100) output,
@ShowroomStartTime varchar(50) output,
@ShowroomEndTime varchar(50) output,
@PrimaryMobileNo varchar(50) output,
@SecondaryMobileNo varchar(50) output,
@LandLineNo varchar(50) output,
@DealerArea varchar(400) output,
@Latitude float output,
@Longitude float output,
@CityName varchar(50) output,
@StateName varchar(30) output
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT @MakeId=ds.MakeId,@CityId=ds.CityId,@Name=ds.Name,@Address=ds.Address,@Pincode=ds.Pincode,@ContactNo=ds.ContactNo,
	@FaxNo=ds.FaxNo,@EMailId=ds.EMailId,@WebSite=ds.WebSite,@WorkingHours=ds.WorkingHours,@HostURL=ds.HostURL,@ContactPerson=ds.ContactPerson
	,@DealerMobileNo=ds.DealerMobileNo,@Mobile=ds.Mobile,@ShowroomStartTime=ds.ShowroomStartTime,@ShowroomEndTime=ds.ShowroomEndTime
	,@PrimaryMobileNo=ds.PrimaryMobileNo,@SecondaryMobileNo=ds.SecondaryMobileNo,@LandLineNo=ds.LandLineNo,@DealerArea=ds.DealerArea
	,@Latitude=ds.Latitude,@Longitude=ds.Longitude,@CityName=ci.Name,@StateName=s.Name
	FROM Dealer_NewCar ds WITH (NOLOCK)
	inner join cities ci WITH (NOLOCK) on ci.id=ds.CityId
	inner join states s WITH (NOLOCK) on ci.StateId=s.ID
	WHERE ds.TcDealerId=@DealerId AND ds.IsActive=1 AND ds.IsNewDealer=1
	ORDER BY ISNULL(IsPremium,0) DESC
	--AND CONVERT(date,GETDATE()) BETWEEN  CONVERT(date,ds.PackageStartDate) and CONVERT(date,ds.PackageEndDate)
END