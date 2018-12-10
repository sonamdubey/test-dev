IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[UsedCarCustomersProfilesDailyAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[UsedCarCustomersProfilesDailyAlert]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 29-12-2011
-- Description:	Populate Profiles matching with used Car customer search criteria
--  [UCAlert].[UsedCarCustomersProfilesDailyAlert]  1
-- Modified By : Manish on 17-09-2013 added condition not like '%-%' for avoiding wrong data
-- Modified By: Manish on 19-08-2015 added insert statement in  try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[UsedCarCustomersProfilesDailyAlert] 
@UsedCarAlertId int
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	declare @budgetid varchar(max)
	declare @sql varchar(max)
	
	declare @budgetCondition varchar(MAX)	
	SELECT @budgetCondition=UCAlert.GetFUserCarAlertBudget(UsedCarAlert_Id)
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND BudgetId NOT LIKE '%-%'
	
	declare @CarAgeCondition varchar(MAX)	
	SELECT @CarAgeCondition=UCAlert.GetUserCarAlertCarAge(UsedCarAlert_Id)
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND yearid NOT LIKE '%-%'
	
	declare @CarKmCondition varchar(MAX)	
	SELECT @CarKmCondition=UCAlert.GetUserCarAlertCarKms(UsedCarAlert_Id)
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND KmsId NOT LIKE '%-%'
	
	declare @CarMakeCondition varchar(MAX)	
	SELECT @CarMakeCondition=' AND nc.makeid in ('+makeid+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND MakeId NOT LIKE '%-%'
	
	declare @CarModelCondition varchar(MAX)	
	SELECT @CarModelCondition=' AND nc.modelid in ('+[dbo].[SplitTextfortwodelimiters](Modelid,',','.')+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	and modelid is not null
	AND modelId NOT LIKE '%-%'
	
	declare @CarBodyStyleCondition varchar(MAX)	
	SELECT @CarBodyStyleCondition=' AND nc.BodyStyleId in ('+bodystyleid+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND BodyStyleId NOT LIKE '%-%'
	
	declare @CarSellerCondition varchar(MAX)	
	SELECT @CarSellerCondition=' AND nc.sellertype in ('+SellerId+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND SellerId NOT LIKE '%-%'
	
	declare @CarFuelTypeCondition varchar(MAX)	
	SELECT @CarFuelTypeCondition=' AND nc.FuelType in ('+FuelTypeId+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND FuelTypeId NOT LIKE '%-%'
	
	declare @CarTransmissionCondition varchar(MAX)	
	SELECT @CarTransmissionCondition=' AND nc.TransmissionId in ('+TransmissionId+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	AND TransmissionId NOT LIKE '%-%'

	
	set @sql='INSERT INTO UCAlert.DailyAlerts(UsedCarAlert_Id,CustomerAlert_Email,Car_CityId,ProfileId,Car_SellerId,Car_Seller,Car_Price,Car_MakeId,Car_Make,Car_ModelId,
Car_Model, '+
			'Car_City,Car_Kms,Car_Version,Car_Year,Car_Color,Car_HasPhoto,Car_LastUpdated,CustomerAlert_City,CustomerAlert_Budget,CustomerAlert_MakeYear,CustomerAlert_Kms,CustomerAlert_Make, '+
			'CustomerAlert_Model,CustomerAlert_FuelType,CustomerAlert_BodyStyle,CustomerAlert_Transmission,CustomerAlert_Seller,alertUrl) '+
			'SELECT UsedCarAlert_Id,c.Email,nc.CityId,nc.ProfileId,nc.SellerType,nc.Seller,nc.Price,nc.MakeId,nc.Make,nc.ModelId,nc.Model,nc.City,nc.Kilometers, '+
			'nc.Version,nc.MakeYear,nc.Color,nc.HasPhoto,nc.LastUpdated,c.City,c.Budget,c.MakeYear,c.Kms,c.Make,c.Model,c.FuelType,c.BodyStyle,c.Transmission,c.Seller,c.alertUrl   ' +
			'FROM UCAlert.UserCarAlerts as C WITH(NOLOCK) ,'+
			' UCAlert.NewlyAddedDailyCars   nc WITH(NOLOCK) '+
			' WHERE  UsedCarAlert_Id= '+cast(@UsedCarAlertId as varchar(1000))+
			' AND C.CityId=nc.cityid '+
			' AND c.IsActive=1 '+
			' AND c.AlertFrequency=2 '
	
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
	
	BEGIN TRY
	--print @sql
	EXEC(@sql)
	
	END TRY
	BEGIN CATCH
	--ROLLBACK TRAN
	SELECT ERROR_MESSAGE(),@sql;
	 INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Daily alert email',
									        'UCAlert.UsedCarCustomersProfilesDailyAlert',
											 ERROR_MESSAGE(),
											 'UCAlert.UserCarAlerts',
											 @UsedCarAlertId,
											 GETDATE()
                                            )
  
	END CATCH;

   
END



