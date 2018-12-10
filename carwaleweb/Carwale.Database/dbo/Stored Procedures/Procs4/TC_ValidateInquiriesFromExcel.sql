IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ValidateInquiriesFromExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ValidateInquiriesFromExcel]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 17 May 2013
-- Description:	To Validate imported excel inquiries.
-- =============================================
CREATE PROCEDURE [dbo].[TC_ValidateInquiriesFromExcel] 
	@Name VARCHAR(100) ,
	@Email VARCHAR(100) ,
	@Mobile VARCHAR(15) ,
	@Location VARCHAR(50) ,
	@CityId INT,
	@Address VARCHAR(100) ,
	@Make VARCHAR(50) ,
	@Model VARCHAR(50) ,
	@Version VARCHAR(50) ,
	@VersionId INT,
	@Color VARCHAR(50) ,
	@CarYear VARCHAR(4) ,
	@Mileage VARCHAR(4) ,
	@BuyingTime VARCHAR(20) ,
	@CarDetails VARCHAR(1000) ,
	@Eagerness SMALLINT ,
	@Price VARCHAR(50) ,
	@RegistrationNo VARCHAR(15) ,
	@Comments VARCHAR(500) ,
	@IsValid BIT ,
	@InquiryType TINYINT, 
	@UserId BIGINT, 
	@BranchId BIGINT,
	@TC_InquirySourceId SMALLINT,
	@MinPrice INT,
	@MaxPrice INT,
	@FromMakeYear SMALLINT,
	@ToMakeYear SMALLINT,
	@ExcelInquiryId BIGINT = NULL,
	@ExcelSheetId BIGINT,
	@IsNewInq BIT OUTPUT
AS
BEGIN
	IF (@InquiryType=1)
	BEGIN
		IF(@IsValid=0)
		BEGIN
			--Insert inquiry in respective Import Inquiry table
			UPDATE	TC_ImportBuyerInquiries
			SET		Name=@Name,Email=@Email,Mobile=@Mobile,Location=@Location,Price=@Price,
					CarYear=@CarYear,CarDetails=@CarDetails ,Comments= @Comments 
			WHERE	IsValid=0 
					AND IsDeleted=0 
					AND UserId=@UserId 
					AND BranchId=@BranchId 
					AND TC_BuyerInquiriesId IS NULL
					AND LeadOwnerId IS NULL 
		END
		ELSE
		BEGIN
			EXEC TC_INQFromExcelInsert @Name = @Name,@Email = @Email,@Mobile = @Mobile,@Location = @Location,@CityId = NULL,@Address = NULL,@Make = NULL,
				@Model = NULL,@Version = NULL,@VersionId = NULL,@Color = NULL,@CarYear = @CarYear,@Mileage = NULL,@BuyingTime = NULL,@CarDetails = @CarDetails,
				@Eagerness = NULL,@Price = @Price,@RegistrationNo = NULL,@Comments = @Comments,@IsValid = @IsValid,@InquiryType = @InquiryType,@UserId = @UserId,
				@BranchId = @BranchId,@TC_InquirySourceId = @TC_InquirySourceId,@MinPrice = @MinPrice,@MaxPrice = @MaxPrice,@FromMakeYear = @FromMakeYear,
				@ToMakeYear = @ToMakeYear, @ExcelInquiryId = @ExcelInquiryId,@ExcelSheetId = NULL,@IsNewInq = @IsNewInq OUTPUT
		END							
									
	END		
	
	ELSE IF (@InquiryType=2)
	BEGIN
		IF(@ExcelInquiryId IS NULL)
		BEGIN
			UPDATE	TC_ImportSellerInquiries 
			SET		Name=@Name,Email=@Email,Mobile=@Mobile,Location=@Location,Price=@Price,CarMake=@Make,CarModel=@Model,CarVersion=@Version,
					CarYear=@CarYear,Comments= @Comments--, IsValid=@IsValid 
			WHERE	IsValid=0 
					AND IsDeleted=0 
					AND UserId=@UserId 
					AND BranchId=@BranchId 
					AND TC_SellerInquiriesId IS NULL
					AND LeadOwnerId IS NULL 
		END
		ELSE
		BEGIN
			EXEC TC_INQFromExcelInsert @Name = @Name,@Email = @Email,@Mobile = @Mobile,@Location = @Location,@CityId = NULL,@Address = NULL,@Make = @Make,
				@Model = @Model,@Version = @Version,@VersionId = @VersionId,@Color = NULL,@CarYear = @CarYear,@Mileage = NULL,@BuyingTime = NULL,@CarDetails = @CarDetails,
				@Eagerness = NULL,@Price = @Price,@RegistrationNo = NULL,@Comments = @Comments,@IsValid = @IsValid,@InquiryType = @InquiryType,@UserId = @UserId,
				@BranchId = @BranchId,@TC_InquirySourceId = @TC_InquirySourceId,@MinPrice = @MinPrice,@MaxPrice = @MaxPrice,@FromMakeYear = @FromMakeYear,
				@ToMakeYear = @ToMakeYear, @ExcelInquiryId = @ExcelInquiryId,@ExcelSheetId = NULL,@IsNewInq = @IsNewInq OUTPUT
		END		

	END
	
	ELSE IF (@InquiryType=3)
	BEGIN
		IF(@ExcelInquiryId IS NULL)
		BEGIN
			UPDATE	TC_ExcelInquiries 
			SET		Name=@Name,Email=@Email,Mobile=@Mobile,CarMake=@Make,CarModel=@Model,City=@Location,Comments= @Comments--, IsValid=@IsValid 
			WHERE	IsValid=0 
					AND IsDeleted=0 
					AND UserId=@UserId 
					AND BranchId=@BranchId 
					AND TC_NewCarInquiriesId IS NULL
					AND LeadOwnerId IS NULL 
							
							
			SET @ExcelInquiryId = SCOPE_IDENTITY()
		END
		ELSE
		BEGIN
			EXEC TC_INQFromExcelInsert @Name = @Name,@Email = @Email,@Mobile = @Mobile,@Location = @Location,@CityId = @CityId,@Address = NULL,@Make = @Make,
				@Model = @Model,@Version = @Version,@VersionId = @VersionId,@Color = NULL,@CarYear = @CarYear,@Mileage = NULL,@BuyingTime = NULL,@CarDetails = @CarDetails,
				@Eagerness = NULL,@Price = @Price,@RegistrationNo = NULL,@Comments = @Comments,@IsValid = @IsValid,@InquiryType = @InquiryType,@UserId = @UserId,
				@BranchId = @BranchId,@TC_InquirySourceId = @TC_InquirySourceId,@MinPrice = @MinPrice,@MaxPrice = @MaxPrice,@FromMakeYear = @FromMakeYear,
				@ToMakeYear = @ToMakeYear, @ExcelInquiryId = @ExcelInquiryId,@ExcelSheetId = NULL,@IsNewInq = @IsNewInq OUTPUT
		END	
	END
END
