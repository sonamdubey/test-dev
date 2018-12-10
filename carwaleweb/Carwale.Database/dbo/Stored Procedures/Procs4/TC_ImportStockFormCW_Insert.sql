IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ImportStockFormCW_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ImportStockFormCW_Insert]
GO

	-- Modified By: Binumon George
-- Modified date: 28 mar 2012
-- Description: -- caling proc TC_AddBuyerInquiries instead TC_SaveInquiryDetails_SP

-- Modified By: Surendra
-- Modified date: 25 Jan 2012
-- Description: -- IsReplicated field need to update
-- =============================================
CREATE Procedure [dbo].[TC_ImportStockFormCW_Insert]
(
@SellInqId Int,
@Dealer_ID Numeric
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
		
	-- Here we are retrieving enquiry details from SellInquiries table
	SELECT  @ID= ID, @CarversionId=CarVersionId, @StatusId=StatusId, @Price=Price, @Kilometers=Kilometers, @Makeyear=MakeYear,
	@Color= isnull(Color,''), @RegNo=CarRegNo,@comments=Comments,
	@EntryDate=EntryDate,@LastUpdated= LastUpdated,  @Certificationid=CertificationId FROM SellInquiries where ID=@SellInqId 

	-- Inserting details in TC_Stock from already fetch details in variable from SellInquiries table
	INSERT INTO TC_Stock (BranchId, VersionId, StatusId, Price,Kms,MakeYear,Colour,RegNo,
	EntryDate,LastUpdatedDate, IsActive, IsSychronizedCW,CertificationId) values
	(@Dealer_ID,@CarversionId, @StatusId, @Price,@Kilometers, @Makeyear, @Color, @RegNo,@EntryDate,@LastUpdated, 1,1,@Certificationid)
	
	set @Identity=SCOPE_IDENTITY();
	
	-- modofied code by Surendra ,date 25/01/2011
	insert into TC_CarPhotos(StockId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,DirectoryPath,IsMain,HostUrl,IsReplicated)
	select @Identity,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,DirectoryPath,IsMain,HostURL,IsReplicated  from CarPhotos 
	where InquiryId=@SellInqId and IsActive=1 and IsApproved=1
	-- End new code
	
	--importing other car details from SellInquiriesDetails into TC_CarCondition
	INSERT INTO TC_CarCondition(StockId, Owners,RegistrationPlace,OneTimeTax,Insurance,InsuranceExpiry,LastUpdatedDate,
	InteriorColor,InteriorColorCode,CityMileage,AdditionalFuel,CarDriven,Accidental,FloodAffected,Warranties,Modifications,Comments,
	ACCondition,BatteryCondition,BrakesCondition,ElectricalsCondition,EngineCondition,ExteriorCondition,InteriorCondition,
	SeatsCondition,SuspensionsCondition,TyresCondition,OverallCondition,Features_SafetySecurity,Features_Comfort,Features_Others)
	select @Identity, dbo.GetOwnerType(Owners),RegistrationPlace,OneTimeTax,Insurance,InsuranceExpiry,UpdateTimeStamp,
	InteriorColor,InteriorColorCode,CityMileage,AdditionalFuel,CarDriven,Accidental,FloodAffected,Warranties,Modifications,@comments,
	ACCondition,BatteryCondition,BrakesCondition,ElectricalsCondition,EngineCondition,ExteriorCondition,InteriorCondition,
	SeatsCondition,SuspensionsCondition,TyresCondition,OverallCondition,Features_SafetySecurity,Features_Comfort,Features_Others
	FROM SellInquiriesDetails where SellInquiryId=@SellInqId

	--importing all purchase enquireis from UsedCarPurchaseInquiries into TC_PurchaseInquiries
	-- There can be multiple row for same Inquiry so here we are creating table variable so that we can loop through on it
	DECLARE @tl_purchase_temp TABLE -- 
	(
		serial int identity(1,1),
		pk numeric
	)
	
	insert into @tl_purchase_temp(pk) select ID  from UsedCarPurchaseInquiries where SellInquiryId=@SellInqId 
	select @rowcount= COUNT(*) from @tl_purchase_temp
			
	if(@rowcount > 0)--For checking sellinqiry data available for particular user
		Begin					
			Declare @count int = 1
			while(@rowcount >= @count)
				Begin
					Declare @pk int
						
					Declare @CustomerName VarChar(100),@CustomerMobile VARCHAR(15)
					Declare @CustomerEmail VarChar(100),@Pcomments VarChar(500),@InterestedIn VarChar(500)
					Declare @SourceId Int, @FollowUp DateTime,@StockId Int,@InquiryId Int,@RequestDateTime DateTime,@PId Numeric 
					Declare @CustomerID Bigint ,@FollowupDate DateTime
					
					set @FollowupDate=GETDATE()
					set @StockId=@Identity -- already inserted stock
					
					select @pk =pk from  @tl_purchase_temp where serial=@count
					
					--Getting all purchase enquiry details for particular primary key			
					select @CustomerID=CustomerID ,@StockId=@Identity,@Pcomments=Comments,@InterestedIn=CarModelNames,@SourceId=1,
					@RequestDateTime=RequestDateTime from UsedCarPurchaseInquiries where ID=@pk 
					
					--Getting Customer Details from Carwale's Customers
					select @CustomerName=Name,@CustomerEmail=Email,@CustomerMobile= Mobile from Customers where Id=@CustomerID
					
					-- Following procedure will insert record in TC_PurchaseInquiries and also check the Customer presence in TC_CustomerDetails
					-- otherwise insert new customer
					--execute TC_SaveInquiryDetails_SP @Dealer_ID,@CustomerName,@CustomerMobile,@CustomerEmail,@Pcomments,@InterestedIn,
					--NULL,@SourceId,@FollowupDate,@StockId,-1,@RequestDateTime,@PId output
					
					EXEC TC_AddBuyerInquiries @BranchId  =@Dealer_ID, @StockId =@StockId, @CustomerName =@CustomerName, @Email =@CustomerEmail,
					@Mobile = @CustomerMobile, @Location = NULL, @Buytime =NULL, @CustomerComments =@Pcomments, @Comments =NULL, @InquiryStatus =NULL,
					@NextFollowup =@FollowupDate, @AssignedTo =NULL,
					@InquirySource =@SourceId, @UserId =NULL 
					
					set @count =@count + 1										
				End
		End
	-- Finnaly updating SellInquiries with newly inserted Stock for TC
	update SellInquiries set TC_StockId= @Identity where ID =@ID
End
