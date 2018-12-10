IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[UsedCarCustomersProfilesDailyAlert_Test]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[UsedCarCustomersProfilesDailyAlert_Test]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 29-12-2011
-- Description:	Populate Profiles matching with used Car customer search criteria
--  [UCAlert].[UsedCarCustomersProfilesDailyAlert_Test]  34256
-- =============================================
CREATE PROCEDURE [UCAlert].[UsedCarCustomersProfilesDailyAlert_Test] 
@UsedCarAlertId int
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	declare @budgetid varchar(100)
	declare @sql varchar(max)
	
	declare @budgetCondition varchar(MAX)	
	SELECT @budgetCondition=UCAlert.GetFUserCarAlertBudget(UsedCarAlert_Id)
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	
	declare @CarAgeCondition varchar(MAX)	
	SELECT @CarAgeCondition=UCAlert.GetUserCarAlertCarAge(UsedCarAlert_Id)
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	
	declare @CarKmCondition varchar(MAX)	
	SELECT @CarKmCondition=UCAlert.GetUserCarAlertCarKms(UsedCarAlert_Id)
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	
	declare @CarMakeCondition varchar(MAX)	
	SELECT @CarMakeCondition=' AND nc.makeid in ('+makeid+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	
	declare @CarModelCondition varchar(MAX)	
	SELECT @CarModelCondition=' AND nc.modelid in ('+[dbo].[SplitTextfortwodelimiters](Modelid,',','.')+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	and modelid is not null
	
	declare @CarBodyStyleCondition varchar(MAX)	
	SELECT @CarBodyStyleCondition=' AND nc.BodyStyleId in ('+bodystyleid+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	
	declare @CarSellerCondition varchar(MAX)	
	SELECT @CarSellerCondition=' AND nc.sellertype in ('+SellerId+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	
	declare @CarFuelTypeCondition varchar(MAX)	
	SELECT @CarFuelTypeCondition=' AND nc.FuelType in ('+FuelTypeId+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId
	
	declare @CarTransmissionCondition varchar(MAX)	
	SELECT @CarTransmissionCondition=' AND nc.TransmissionId in ('+TransmissionId+')'
	from UCAlert.UserCarAlerts 
	where UsedCarAlert_Id=@UsedCarAlertId

	
	set @sql='SELECT UsedCarAlert_Id,c.Email,nc.CityId,nc.ProfileId,nc.SellerType,nc.Seller,nc.Price,nc.MakeId,nc.Make,nc.ModelId,nc.Model,nc.City,nc.Kilometers, '+
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
	
	
	print @sql
	--EXEC(@sql)

   
END







