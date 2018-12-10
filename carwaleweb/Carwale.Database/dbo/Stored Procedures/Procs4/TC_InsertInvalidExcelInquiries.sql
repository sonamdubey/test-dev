IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertInvalidExcelInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertInvalidExcelInquiries]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 17 May 2013
-- Description:	To insert imported invalid excel inquiry.
-- Modified By : Tejashre Patil on 24 Jun 2013, added UserId in seller and new car select query.
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertInvalidExcelInquiries] 
	@Name VARCHAR(100) ,
	@Email VARCHAR(100) ,
	@Mobile VARCHAR(15) ,
	@Location VARCHAR(50) ,
	@CityId INT,
	@Make VARCHAR(50) ,
	@Model VARCHAR(50) ,
	@Version VARCHAR(50) ,
	@VersionId INT,
	@CarYear VARCHAR(4) ,
	@CarDetails VARCHAR(1000) ,
	@Price VARCHAR(50) ,
	@Comments VARCHAR(500) ,
	@IsValid BIT ,
	@InquiryType TINYINT, 
	@UserId BIGINT, 
	@BranchId BIGINT,
	@MinPrice INT,
	@MaxPrice INT,
	@FromMakeYear SMALLINT,
	@ToMakeYear SMALLINT,
	@ExcelInquiryId BIGINT = NULL,
	@LeadOwnerId BIGINT=NULL
AS
BEGIN
	DECLARE @TC_CustomerId BIGINT, @AutoVerified BIT = 1, @TC_InquiryId BIGINT, @IsNewInq BIT
	DECLARE @InqStatus SMALLINT , @LeadDivertedTo VARCHAR(100), @LeadOwnId BIGINT, @CustomerId BIGINT, @LeadIdOutput BIGINT
	DECLARE @Color VARCHAR(50) ,@Address VARCHAR(100) ,@Eagerness SMALLINT ,@Mileage VARCHAR(4) ,@BuyingTime VARCHAR(20) ,@RegistrationNo VARCHAR(15) ,
			@Kilometer INT,@TC_InquirySourceId SMALLINT,@ImportInqCarDetails VARCHAR(1000),@ExcelSheetId BIGINT, @INQLeadIdOutput BIGINT=-1
	
	--Check inquiry is exists in respective Inquiry table
	
	IF (@InquiryType=1)
	BEGIN
		IF(@ExcelInquiryId IS NOT NULL)
		BEGIN
		
			--Update inquiry details in respective Import Inquiry table
			UPDATE	TC_ImportBuyerInquiries 
			SET		Name=@Name,Email=@Email,Mobile=@Mobile,Location=@Location,Price=@Price,IsValid=@IsValid,MaxPrice=@MaxPrice,MinPrice=@MinPrice,
					FromYear=@FromMakeYear,ToYear=@ToMakeYear,LeadOwnerId=@LeadOwnerId,CarYear=@CarYear,CarDetails=@CarDetails, Comments=@Comments, 
					ModifiedDate=GETDATE()
			WHERE	TC_ImportBuyerInquiriesId=@ExcelInquiryId
					AND BranchId=@BranchId 
					AND IsDeleted=0
					AND IsValid=0
					AND TC_BuyerInquiriesId IS NULL
					
			SELECT	@Name=B.Name,@Email=B.Email,@Mobile= B.Mobile,@Location= B.Location,@Price= B.Price,@IsValid= B.IsValid ,@UserId= B.UserId,
					@MaxPrice=B.MaxPrice,@MinPrice=B.MinPrice,@FromMakeYear=B.FromYear,@ToMakeYear=B.ToYear,@Eagerness=B.Eagerness,@BuyingTime=B.BuyingTime,
					@CarYear= B.CarYear,@CarDetails = B.CarDetails, @Comments = B.Comments, @TC_InquirySourceId= B.TC_InquirySourceId, @Make= B.CarMake,
					@Model= B.CarModel,@Version=B.CarVersion,@IsNewInq=B.IsNew
			FROM	TC_ImportBuyerInquiries B
			WHERE	BranchId=@BranchId AND IsDeleted=0
					AND TC_ImportBuyerInquiriesId=@ExcelInquiryId											
			
			IF(@IsValid=1)
			BEGIN
				--Insert inquiry in respective Inquiry table if it is valid and already lead exist
				EXEC TC_INQBuyerSave @AutoVerified ,@BranchId ,NULL,@Name,@Email,@Mobile,@Location,@BuyingTime,@Comments,@Eagerness,NULL,@TC_InquirySourceId ,
				@LeadOwnerId ,@UserId ,@MinPrice ,@MaxPrice ,@FromMakeYear ,@ToMakeYear ,NULL,NULL,NULL ,NULL ,NULL ,NULL ,NULL,@InqStatus OUTPUT,NULL ,NULL,
				@LeadDivertedTo  OUTPUT,@TC_InquiryId  OUTPUT ,@CarDetails ,@ExcelInquiryId  ,@TC_InquirySourceId,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
				@LeadIdOutput OUTPUT,@INQLeadIdOutput OUTPUT
			
			END	
			
		END							
							
	END		
	
	ELSE IF (@InquiryType=2)
	BEGIN
		IF(@ExcelInquiryId IS NOT NULL)
		BEGIN
			UPDATE	TC_ImportSellerInquiries
			SET		Name=@Name,Email=@Email,Mobile=@Mobile,Location=@Location,Price=@Price,IsValid=@IsValid,LeadOwnerId=@LeadOwnerId,
					CarYear=@CarYear,Comments=@Comments,CarMake=@Make,CarModel=@Model,CarVersion=@Version,VersionId=@VersionId, ModifiedDate=GETDATE()				
			WHERE	TC_ImportSellerInquiriesId=@ExcelInquiryId
					AND BranchId=@BranchId 
					AND IsDeleted=0
					AND IsValid=0
					AND TC_SellerInquiriesId IS NULL
			
			
			SELECT	@Name=S.Name,@Email=S.Email,@Mobile=S.Mobile,@Eagerness=S.Eagerness,@Address=S.Address, @Location=S.Location,@Make=S.CarMake,
					@Model=S.CarModel,@Version=S.CarVersion,@Price=S.Price,@RegistrationNo=S.RegistrationNo, @Color=S.CarColor, @Mileage=S.CarMileage,
					@IsValid=S.IsValid,@TC_InquirySourceId=S.TC_InquirySourceId,@CarYear=S.CarYear,@Comments=S.Comments,@VersionId=S.VersionId,
					@IsNewInq=S.IsNew, @UserId=S.UserId
			FROM	TC_ImportSellerInquiries S
			WHERE	BranchId=@BranchId AND IsDeleted=0
					AND TC_ImportSellerInquiriesId =@ExcelInquiryId
			
		
			SET @CarYear=CONVERT(INT,@CarYear)
			SET @Price=CONVERT(INT,@Price)
			
			IF(@IsValid=1)
			BEGIN
				--Insert inquiry in respective Inquiry table if it is valid and already lead exist
				EXEC TC_INQSellerSave NULL,@BranchId,@VersionId,@AutoVerified,@LeadOwnerId,@Name,@Email,@Mobile,@Location,@BuyingTime,@Eagerness,
				@TC_InquirySourceId ,@UserId ,@CarYear, @Price,NULL,@Color ,NULL ,@RegistrationNo ,NULL,NULL ,NULL ,NULL ,NULL ,NULL,@Mileage ,0 , 
				0 ,NULL ,NULL ,NULL ,NULL ,NULL , NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,
				@InqStatus  OUTPUT,@LeadDivertedTo  OUTPUT,@ExcelInquiryId ,@TC_InquirySourceId,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
				@LeadIdOutput OUTPUT,@INQLeadIdOutput OUTPUT
			END
			 
		END

	END
	
	ELSE IF (@InquiryType=3)
	BEGIN
		IF(@ExcelInquiryId IS NOT NULL)
		BEGIN
			UPDATE	TC_ExcelInquiries
			SET		Name=@Name,Email=@Email,Mobile=@Mobile,City=@Location,IsValid=@IsValid,LeadOwnerId=@LeadOwnerId,CityId=@CityId,
					Comments=@Comments,CarMake=@Make,CarModel=@Model,VersionId=@VersionId, ModifiedDate=GETDATE()
			WHERE	Id=@ExcelInquiryId
					AND BranchId=@BranchId 
					AND IsDeleted=0
					AND IsValid=0
					AND TC_NewCarInquiriesId IS NULL
			
			SELECT	@Name=E.Name,@Email=E.Email,@Mobile=E.Mobile,@IsValid=E.IsValid,@TC_InquirySourceId=E.TC_InquirySourceId,
					@CityId=E.CityId,@VersionId=E.VersionId,@IsNewInq=E.IsNew, @UserId=E.UserId
			FROM	TC_ExcelInquiries E
			WHERE	BranchId=@BranchId AND IsDeleted=0
					AND Id =@ExcelInquiryId						
			
			IF(@IsValid=1)
			BEGIN
				DECLARE @IsNew BIT
				DECLARE @PqRequestDate DATETIME = GETDATE()
				--Insert inquiry in respective Inquiry table if it is valid and already lead exist
				EXEC TC_INQNewCarBuyerSave @Name,@Email,@Mobile,@VersionId ,@CityId ,@BuyingTime ,@TC_InquirySourceId ,@Eagerness ,NULL ,@AutoVerified ,
				@BranchId ,@LeadOwnerId ,@UserId ,@InqStatus  OUTPUT, @PqRequestDate ,NULL ,NULL  ,NULL  ,NULL  ,NULL ,NULL ,@TC_InquiryId  OUTPUT,
				@LeadDivertedTo OUTPUT,	@IsNew OUTPUT,@TC_InquirySourceId,@ExcelInquiryId ,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
				@LeadIdOutput OUTPUT,@INQLeadIdOutput OUTPUT
			END
			
		END	
	END
	
	--To divert calls
	IF(@INQLeadIdOutput IS NOT NULL AND @INQLeadIdOutput >0)
	BEGIN
		EXEC TC_INQLeadAssignment @BranchId,@LeadOwnerId,@INQLeadIdOutput,@UserId
	END
						
END
