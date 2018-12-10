IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ImportStockFormCW_Update]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ImportStockFormCW_Update]
GO

	CREATE Procedure [dbo].[TC_ImportStockFormCW_Update]
(
@SellInqId Int,
@Stock_Id Int
)
as
Begin
	Declare @Identity int-- For get last inserted value to Tc_stock table and update  tc_stockid colum in sellinquiry table
	Declare @CarversionId Numeric, @StatusId Numeric, @Kilometers Numeric, @ID Numeric
	Declare @Price Decimal
	Declare @Makeyear Datetime, @EntryDate Datetime, @LastUpdated Datetime
	Declare @Color Varchar(30), @RegNo Varchar(15)
	Declare @IsArchive bit,@IsSychronizedCW bit
	Declare @Certificationid smallint, @rowcount smallint
	Declare @comments varchar(500)
	
	/*************************** **************************************************/
				-- Update Car Basic details from CarWale to Trading Cars
	/*************************** **************************************************/	
	-- Here we are retrieving enquiry details from SellInquiries table
	SELECT  @ID= ID, @CarversionId=CarVersionId, @StatusId=StatusId, @Price=Price, @Kilometers=Kilometers, @Makeyear=MakeYear,
	@Color= isnull(Color,''), @RegNo=CarRegNo,@comments=Comments,
	@EntryDate=EntryDate,@LastUpdated= LastUpdated,  @Certificationid=CertificationId FROM SellInquiries where ID=@SellInqId 
	-- Updating details in TC_Stock from already fetch details in variable from SellInquiries table
	UPDATE TC_Stock SET VersionId=@CarversionId, StatusId=@StatusId, Price=@Price,Kms=@Kilometers,MakeYear=@Makeyear,
	Colour=@Color,RegNo=@RegNo,EntryDate=@EntryDate,LastUpdatedDate=@LastUpdated,CertificationId=@Certificationid WHERE ID=@Stock_Id	
	
	
	/*************************** **************************************************/
				-- Update Other details of the car from CarWale to Trading Cars
	/*************************** **************************************************/	
	--importing other car details from SellInquiriesDetails into TC_CarCondition
	Declare @Owners VARCHAR(50),@RegistrationPlace VARCHAR(50),@OneTimeTax VARCHAR(50),@Insurance VARCHAR(50),@InteriorColor VARCHAR(50),
	@InteriorColorCode VARCHAR(6),@CityMileage VARCHAR(50), @AdditionalFuel VARCHAR(50),@CarDriven VARCHAR(50),@Warranties VARCHAR(500),
	@Modifications VARCHAR(500),@ConditionComments VARCHAR(500),@ACCondition VARCHAR(50),@BatteryCondition VARCHAR(50),
	@BrakesCondition VARCHAR(50),@ElectricalsCondition VARCHAR(50), @EngineCondition VARCHAR(50), @ExteriorCondition VARCHAR(50),
	@InteriorCondition VARCHAR(50),@SeatsCondition VARCHAR(50), @SuspensionsCondition VARCHAR(50), @TyresCondition VARCHAR(50),
	@OverallCondition VARCHAR(50),@Features_SafetySecurity VARCHAR(200), @Features_Comfort VARCHAR(200), @Features_Others VARCHAR(200),
	@Accidental BIT, @FloodAffected BIT,
	@InsuranceExpiry DATETIME, @LastUpdatedDate DATETIME
	
	select  @Owners=Owners,@RegistrationPlace=RegistrationPlace,@OneTimeTax=OneTimeTax,@Insurance=Insurance,@InsuranceExpiry=InsuranceExpiry,
	@LastUpdatedDate=UpdateTimeStamp,
	@InteriorColor=InteriorColor,@InteriorColorCode=InteriorColorCode,@CityMileage=CityMileage,@AdditionalFuel=AdditionalFuel,
	@CarDriven=CarDriven,@Accidental=Accidental,@FloodAffected=FloodAffected,@Warranties=Warranties,@Modifications=Modifications,
	@ConditionComments=@comments,
	@ACCondition=ACCondition,@BatteryCondition=BatteryCondition,@BrakesCondition=BrakesCondition,@ElectricalsCondition=ElectricalsCondition,
	@EngineCondition=EngineCondition,@ExteriorCondition=ExteriorCondition,@InteriorCondition=InteriorCondition,
	@SeatsCondition=SeatsCondition,@SuspensionsCondition=SuspensionsCondition,@TyresCondition=TyresCondition,@OverallCondition=OverallCondition,
	@Features_SafetySecurity=Features_SafetySecurity,@Features_Comfort=Features_Comfort,@Features_Others=Features_Others
	FROM SellInquiriesDetails where SellInquiryId=@SellInqId
	
	UPDATE TC_CarCondition SET Owners= dbo.GetOwnerType(@Owners),RegistrationPlace=@RegistrationPlace,OneTimeTax=@OneTimeTax,Insurance=@Insurance,
	InsuranceExpiry=@InsuranceExpiry,LastUpdatedDate=@LastUpdatedDate,
	InteriorColor=@InteriorColor,InteriorColorCode=@InteriorColorCode,CityMileage=@CityMileage,AdditionalFuel=@AdditionalFuel,
	CarDriven=@CarDriven,Accidental=@Accidental,FloodAffected=@FloodAffected,Warranties=@Warranties,Modifications=@Modifications,
	Comments=@ConditionComments,
	ACCondition=@ACCondition,BatteryCondition=@BatteryCondition,BrakesCondition=@BrakesCondition,ElectricalsCondition=@ElectricalsCondition,
	EngineCondition=@EngineCondition,ExteriorCondition=@ExteriorCondition,InteriorCondition=@InteriorCondition,
	SeatsCondition=@SeatsCondition,SuspensionsCondition=@SuspensionsCondition,TyresCondition=@TyresCondition,OverallCondition=@OverallCondition,
	Features_SafetySecurity=@Features_SafetySecurity,Features_Comfort=@Features_Comfort,Features_Others=Features_Others
	WHERE StockId=@Stock_Id
	
	/*************************** **************************************************/
				-- Update all active photos from CarWale to Trading Cars
	/*************************** **************************************************/	
	Delete FROM TC_CarPhotos Where StockId = @Stock_Id
	
	insert into TC_CarPhotos(StockId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,DirectoryPath,IsMain,HostUrl,IsReplicated)
	select @Stock_Id,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,DirectoryPath,IsMain,HostURL, IsReplicated from CarPhotos 
	where InquiryId=@SellInqId and IsActive=1 and IsApproved=1
	-- End new code		
End
