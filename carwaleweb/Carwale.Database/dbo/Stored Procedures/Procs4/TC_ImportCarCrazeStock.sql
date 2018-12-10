IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ImportCarCrazeStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ImportCarCrazeStock]
GO

	-- Avishkar
-- Modified date: 28 Jan 2013
-- Description: --Import stock of CarCraze in TradingCar
-- exec TC_ImportCarCrazeStock 69576,	1550
-- =============================================
CREATE  Procedure [dbo].[TC_ImportCarCrazeStock]
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
	
	BEGIN TRY
		--BEGIN TRANSACTION TranStockInsert
		BEGIN TRANSACTION
		/*
		  Only pull  Unavailable cars and listing before 2011-04-30 
		*/		
		
	-- Here we are retrieving enquiry details from SellInquiries table
	SELECT  @ID= ID, @CarversionId=CarVersionId, 
			@StatusId=StatusId, 
			@Price=Price, @Kilometers=Kilometers, @Makeyear=MakeYear,
			@Color= isnull(Color,''), @RegNo=CarRegNo,@comments=Comments,
			@EntryDate=EntryDate,@LastUpdated= LastUpdated,  @Certificationid=CertificationId 
	FROM SellInquiries WITH(NOLOCK)
	where DealerId=@Dealer_ID
	and ID=@SellInqId
	and  StatusId=2
	and EntryDate<'2011-04-30 12:55:10.500'

	-- Inserting details in TC_Stock from already fetch details in variable from SellInquiries table
	/*
	IsActive =0  and  IsSychronizedCW=0	
	*/
	INSERT INTO TC_Stock (BranchId, VersionId, StatusId, Price,Kms,MakeYear,Colour,RegNo,
	EntryDate,LastUpdatedDate, IsActive, IsSychronizedCW,CertificationId) 
	values (@Dealer_ID,@CarversionId, @StatusId, @Price,@Kilometers, @Makeyear, @Color, @RegNo,@EntryDate,@LastUpdated, 1,0,@Certificationid)
	
	set @Identity=SCOPE_IDENTITY();
	
	
	insert into TC_CarPhotos(StockId,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,DirectoryPath,IsMain,HostUrl,IsReplicated)
	select @Identity,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,DirectoryPath,IsMain,HostURL,IsReplicated  
	from CarPhotos WITH(NOLOCK)
	where InquiryId=@SellInqId 
	and IsActive=1 
	and IsApproved=1
	and IsDealer=1
	
	
	--importing other car details from SellInquiriesDetails into TC_CarCondition
	INSERT INTO TC_CarCondition(StockId, Owners,RegistrationPlace,OneTimeTax,Insurance,InsuranceExpiry,LastUpdatedDate,
	InteriorColor,InteriorColorCode,CityMileage,AdditionalFuel,CarDriven,Accidental,FloodAffected,Warranties,Modifications,Comments,
	ACCondition,BatteryCondition,BrakesCondition,ElectricalsCondition,EngineCondition,ExteriorCondition,InteriorCondition,
	SeatsCondition,SuspensionsCondition,TyresCondition,OverallCondition,Features_SafetySecurity,Features_Comfort,Features_Others)
	select @Identity, dbo.GetOwnerType(Owners),RegistrationPlace,OneTimeTax,Insurance,InsuranceExpiry,UpdateTimeStamp,
	InteriorColor,InteriorColorCode,CityMileage,AdditionalFuel,CarDriven,Accidental,FloodAffected,Warranties,Modifications,@comments,
	ACCondition,BatteryCondition,BrakesCondition,ElectricalsCondition,EngineCondition,ExteriorCondition,InteriorCondition,
	SeatsCondition,SuspensionsCondition,TyresCondition,OverallCondition,Features_SafetySecurity,Features_Comfort,Features_Others
	FROM SellInquiriesDetails WITH(NOLOCK)
	where SellInquiryId=@SellInqId

	--importing all purchase enquireis from UsedCarPurchaseInquiries into TC_PurchaseInquiries
	-- There can be multiple row for same Inquiry so here we are creating table variable so that we can loop through on it
	DECLARE @tl_purchase_temp TABLE -- 
	(
		serial int identity(1,1),
		pk numeric
	)
	
	insert into @tl_purchase_temp(pk) 
	select ID  
	from UsedCarPurchaseInquiries 
	where SellInquiryId=@SellInqId 
	
	select @rowcount= COUNT(*) 
	from @tl_purchase_temp
			
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
					
					set @FollowupDate=NULL /* old stock */
					set @StockId=@Identity -- already inserted stock
					
					select @pk =pk 
					from  @tl_purchase_temp 
					where serial=@count
					
					--Getting all purchase enquiry details for particular primary key			
					select	@CustomerID=CustomerID ,@StockId=@Identity,@Pcomments=Comments,@InterestedIn=CarModelNames,@SourceId=1,
							@RequestDateTime=RequestDateTime 
					from UsedCarPurchaseInquiries 
					where ID=@pk 
					
					--Getting Customer Details from Carwale's Customers
					select @CustomerName=Name,@CustomerEmail=Email,@CustomerMobile= Mobile 
					from Customers 
					where Id=@CustomerID					
					
					
					EXEC TC_AddCarCrazeBuyerInquiries @BranchId  =@Dealer_ID, @StockId =@StockId, @CustomerName =@CustomerName, @Email =@CustomerEmail,
					@Mobile = @CustomerMobile, @Location = NULL, @Buytime =NULL, @CustomerComments =@Pcomments, @Comments =NULL, @InquiryStatus =NULL,
					@NextFollowup =@FollowupDate, @AssignedTo =NULL,
					@InquirySource =@SourceId, @UserId =NULL ,@RequestDateTime=@RequestDateTime
					
					set @count =@count + 1										
				End
		End
	-- Finnaly updating SellInquiries with newly inserted Stock for TC
	update SellInquiries 
	set TC_StockId= @Identity 
	where ID =@ID
	
	--COMMIT TRANSACTION TranStockInsert
	COMMIT TRANSACTION 
	END TRY
	
	BEGIN CATCH
		--ROLLBACK TRAN TranStockInsert
		IF @@trancount > 0 ROLLBACK TRAN 
		INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
	 VALUES('TC_ImportCarCrazeStock',ERROR_MESSAGE(),GETDATE())
	END CATCH;
End
