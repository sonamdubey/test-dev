IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[NDUsedCarCustomersProfilesWeeklyAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[NDUsedCarCustomersProfilesWeeklyAlert]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 23-06-2014
-- Description:	Populate Profiles matching with used Car customer search criteria
-- Modified By : Manish on 24-09-2014 changes in dynamic query for resolution of the exceptions.\
-- Modified By: Manish on 20-10-2014 change alert frequency from weekly alert to twice in a week alert.
-- =============================================
CREATE PROCEDURE [UCAlert].[NDUsedCarCustomersProfilesWeeklyAlert]
@UsedCarAlertId INT
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
		DECLARE @Usercity VARCHAR(100),
            @Userbudget VARCHAR(50),
            @Usermakeyear VARCHAR(30),
            @Userkms VARCHAR(50),
            @UsermakeId VARCHAR(50),
			@Usermake VARCHAR(100),
            @Usermodel VARCHAR(100),
            @UserfueltypeId VARCHAR(30),
			@Userfueltype VARCHAR(100),
            @UserbodystyleId VARCHAR(30),
			@Userbodystyle VARCHAR(100),
            @UsertransmissionId VARCHAR(30),
			@Usertransmission VARCHAR(100),
            @UsersellerId VARCHAR(20),
			@Userseller VARCHAR(50)
       
	SELECT @Usercity=CT.Name,
	       @Userbudget= CONVERT(VARCHAR,C.MinBudget)+'-'+CONVERT(VARCHAR,C.MaxBudget),
		   @Usermakeyear=CONVERT(VARCHAR,C.MinCarAge)+'-'+CONVERT(VARCHAR,C.MaxCarAge) +' Year',
		   @Userkms=CONVERT(VARCHAR,C.Minkms)+'-'+CONVERT(VARCHAR,C.MaxKms) +' Kms',
		   @UsermakeId=c.MakeId,
		   @UserfueltypeId=c.FuelTypeId,
		   @UserbodystyleID=c.BodyStyleId,
		   @UsertransmissionID=c.TransmissionId,
		   @UsersellerId=c.SellerId
	FROM UCALERT.NDUsedCarAlertCustomerList AS C WITH(NOLOCK) 
	JOIN Cities AS CT WITH(NOLOCK) ON CT.ID=C.CityId
	WHERE  C.UsedCarAlert_Id=@UsedCarAlertId

	SELECT 
		@Usermake= COALESCE(@Usermake + ',', '') + CK.Name
		FROM CarMakes AS CK WITH (NOLOCK)
		JOIN fnSplitCSVValues(@Usermakeid) AS F  ON CK.ID=F.ListMember
	
	
	SELECT 
		@Userfueltype= COALESCE(@Userfueltype + ',', '') + CK.FuelType
		FROM CarFuelType AS CK WITH (NOLOCK)
		JOIN fnSplitCSVValues(@UserfueltypeId) AS F  ON CK.FuelTypeId=F.ListMember

	
	SELECT 
		@Userbodystyle= COALESCE(@Userbodystyle + ',', '') + CK.Name
		FROM CarBodyStyles AS CK WITH (NOLOCK)
		JOIN fnSplitCSVValues(@Userbodystyleid) AS F  ON CK.ID=F.ListMember
	
	
	SELECT 
		@Userbodystyle= COALESCE(@Userbodystyle + ',', '') + CK.Descr
		FROM CarTransmission AS CK WITH (NOLOCK)
		JOIN fnSplitCSVValues(@UsertransmissionID) AS F  ON CK.ID=F.ListMember

		
	SELECT 
		@Userseller= COALESCE(@Userseller + ',', '') + CK.Seller
		FROM UCALERT.Seller AS CK WITH (NOLOCK)
		JOIN fnSplitCSVValues(@UsersellerID) AS F  ON CK.ID=F.ListMember
	




	declare @budgetid varchar(100)
	declare @sql varchar(max)
	
	declare @budgetCondition varchar(MAX)	
	SELECT @budgetCondition=' AND Price BETWEEN '  +  CONVERT(VARCHAR,MinBudget) +' AND ' + CONVERT(VARCHAR,MaxBudget)
	from UCALERT.NDUsedCarAlertCustomerList
	where UsedCarAlert_Id=@UsedCarAlertId
	
	
	declare @CarAgeCondition varchar(MAX)	
	SELECT @CarAgeCondition=  '  AND ABS(DATEDIFF(MM,MakeYear,GETDATE()))/12.000   BETWEEN '  +  CONVERT(VARCHAR,MinCarAge) +' AND ' + CONVERT(VARCHAR,MaxCarAge)
	from UCALERT.NDUsedCarAlertCustomerList
	where UsedCarAlert_Id=@UsedCarAlertId

	
	declare @CarKmCondition varchar(MAX)	
	SELECT @CarKmCondition='  AND Kilometers  BETWEEN '  +  CONVERT(VARCHAR,MinKms) +' AND ' + CONVERT(VARCHAR,MaxKms)
	from UCALERT.NDUsedCarAlertCustomerList
	where UsedCarAlert_Id=@UsedCarAlertId
	
	
	declare @CarMakeCondition varchar(MAX)	
	SELECT @CarMakeCondition=' AND nc.makeid in ('+makeid+')'
	from UCALERT.NDUsedCarAlertCustomerList 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND MakeId NOT LIKE '%-%'
	
	declare @CarModelCondition varchar(MAX)	
	SELECT @CarModelCondition=' AND nc.RootId in ('+[dbo].[SplitTextfortwodelimiters](Modelid,',','.')+')'
	from UCALERT.NDUsedCarAlertCustomerList
	where UsedCarAlert_Id=@UsedCarAlertId
	and modelid is not null
	AND modelId NOT LIKE '%-%'
	
	declare @CarBodyStyleCondition varchar(MAX)	
	SELECT @CarBodyStyleCondition=' AND nc.BodyStyleId in ('+bodystyleid+')'
	from UCALERT.NDUsedCarAlertCustomerList
	where UsedCarAlert_Id=@UsedCarAlertId
	AND BodyStyleId NOT LIKE '%-%'
	
	declare @CarSellerCondition varchar(MAX)	
	SELECT @CarSellerCondition=' AND nc.sellertype in ('+SellerId+')'
	from UCALERT.NDUsedCarAlertCustomerList
	where UsedCarAlert_Id=@UsedCarAlertId
	AND SellerId NOT LIKE '%-%'
	
	declare @CarFuelTypeCondition varchar(MAX)	
	SELECT @CarFuelTypeCondition=' AND nc.FuelType in ('+FuelTypeId+')'
	from UCALERT.NDUsedCarAlertCustomerList 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND FuelTypeId NOT LIKE '%-%'
	
	declare @CarTransmissionCondition varchar(MAX)	
	SELECT @CarTransmissionCondition=' AND nc.TransmissionId in ('+TransmissionId+')'
	from UCALERT.NDUsedCarAlertCustomerList 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND TransmissionId NOT LIKE '%-%'


	declare @CarCertifiedCondition varchar(MAX)	
	SELECT @CarCertifiedCondition=CASE WHEN NeedOnlyCertifiedCars=1 THEN ' AND nc.CertificationId IS NOT NULL' END
	from UCALERT.NDUsedCarAlertCustomerList 
	where UsedCarAlert_Id=@UsedCarAlertId
	

	declare @OwnerTypeIdCondition varchar(MAX)	
	SELECT @OwnerTypeIdCondition=' AND nc.OwnerTypeId in ('+OwnerTypeId+')'
	from UCALERT.NDUsedCarAlertCustomerList 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND @OwnerTypeIdCondition NOT LIKE '%-%'
	
	
	set @sql='INSERT INTO UCAlert.WeeklyAlerts(UsedCarAlert_Id,CustomerAlert_Email,Car_CityId,ProfileId,Car_SellerId,Car_Seller,Car_Price,Car_MakeId,Car_Make,Car_ModelId,
Car_Model, '+
			'Car_City,Car_Kms,Car_Version,Car_Year,Car_Color,Car_HasPhoto,Car_LastUpdated,CustomerAlert_City,CustomerAlert_Budget,CustomerAlert_MakeYear,CustomerAlert_Kms,CustomerAlert_Make, '+
			'CustomerAlert_Model,CustomerAlert_FuelType,CustomerAlert_BodyStyle,CustomerAlert_Transmission,CustomerAlert_Seller,alertUrl,IsAlertFromNewDesign) '+
			'SELECT UsedCarAlert_Id,c.Email,nc.CityId,nc.ProfileId,nc.SellerType,nc.Seller,nc.Price,nc.MakeId,nc.Make,nc.ModelId,nc.Model,nc.City,nc.Kilometers, '+
			'nc.Version,nc.MakeYear,nc.Color,nc.HasPhoto,nc.LastUpdated,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL, c.alertUrl ,1  ' +
			'FROM UCALERT.NDUsedCarAlertCustomerList as C WITH(NOLOCK) ,'+
			' UCAlert.NewlyAddedWeeklyCars   nc WITH(NOLOCK) '+
			' WHERE  UsedCarAlert_Id= '+CONVERT(VARCHAR,@UsedCarAlertId)+
			' AND C.CityId=nc.cityid '+
			' AND c.IsActive=1 '+
			' AND c.AlertFrequency=3 '
	
	if @budgetCondition is not null	
	set @sql=@sql+@budgetCondition
	
	if @CarAgeCondition is not null	
	set @sql=@sql+@CarAgeCondition
	
	if @CarKmCondition is not null	
	set @sql=@sql+@CarKmCondition
	
	if @CarMakeCondition is not null	
	set @sql=@sql+@CarMakeCondition
	
	if @CarModelCondition is not null	
	set @sql=@sql+@CarModelCondition
	
	if @CarBodyStyleCondition is not null	
	set @sql=@sql+@CarBodyStyleCondition
	
	if @CarSellerCondition is not null	
	set @sql=@sql+@CarSellerCondition
	
	if @CarFuelTypeCondition is not null	
	set @sql=@sql+@CarFuelTypeCondition
	
	if @CarTransmissionCondition is not null	
	set @sql=@sql+@CarTransmissionCondition

    if @CarCertifiedCondition is not null	
	set @sql=@sql+@CarCertifiedCondition

	
	if @OwnerTypeIdCondition is not null	
	set @sql=@sql+@OwnerTypeIdCondition
	
	BEGIN TRY ---- TRY Block added by Manish on 30-12-2013 for handling the issue if any statement fails than should not affect on other insert statements
	
			--print @sql
			EXEC(@sql)
				UPDATE   UCAlert.WeeklyAlerts SET CustomerAlert_City=@UserCity,
	                                CustomerAlert_Budget=@UserBudget,
	                                CustomerAlert_MakeYear=@UserMakeYear,
									CustomerAlert_Kms=@UserKms,
									CustomerAlert_Make=@UserMake,
									CustomerAlert_FuelType=@UserFuelType,
									CustomerAlert_BodyStyle=@UserBodyStyle,
									CustomerAlert_Transmission=@UserTransmission,
									CustomerAlert_Seller=@UserSeller
WHERE UsedCarAlert_Id=@UsedCarAlertId
and IsAlertFromNewDesign=1

	END TRY

	BEGIN CATCH
	   INSERT INTO ScheduledJobExceptions (
			                                JobName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('UsedCarAlertWeeklyJob',
									        'NDUsedCarCustomersProfilesWeeklyAlert',
											 ERROR_MESSAGE(),
											 'UCAlert.UserCarAlerts',
											 @UsedCarAlertId,
											 GETDATE()
                                            )

	END CATCH;

   
END
