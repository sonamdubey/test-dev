IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQFromExcelInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQFromExcelInsert]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 17 May 2013
-- Description:	To insert imported excel inquiry.
--DECLARE @IsNewInq BIT 
--EXEC TC_INQFromExcelInsert NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, 
--1, 5,NULL, 2278,NULL, NULL, 1, NULL, NULL,@IsNewInq OUTPUT
--SELECT @IsNewInq
-- Modified by Tejashree on 18-06-2013 Fetch CityId when location is null
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQFromExcelInsert] 
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
	--@Kilometer INT,
	@TC_InquirySourceId SMALLINT,
	@MinPrice INT,
	@MaxPrice INT,
	@FromMakeYear SMALLINT,
	@ToMakeYear SMALLINT,
	--@TC_InquiryId BIGINT OUTPUT ,
	--@ImportInqCarDetails VARCHAR(1000),
	@ExcelInquiryId BIGINT = NULL,
	@ExcelSheetId BIGINT,
	@LeadOwnerId BIGINT=NULL,
	@IsNewInq BIT OUTPUT,
	@IsUnassigned BIT=0 OUTPUT
AS
BEGIN
	DECLARE @TC_CustomerId BIGINT, @AutoVerified BIT = 0, @TC_InquiryId BIGINT
	DECLARE @InqStatus SMALLINT , @LeadDivertedTo VARCHAR(100), @LeadOwnId BIGINT, @CustomerId BIGINT, @LeadIdOutput BIGINT
	SET		@IsNewInq=1
	
	IF(@Location IS NULL)
	BEGIN
		SELECT  @Location=Ct.Name,@CityId=Ct.ID
		FROM	Dealers DB
				LEFT JOIN Cities Ct ON DB.CityId=Ct.ID
		WHERE	DB.IsTCDealer = 1 AND DB.ID=@BranchId
	END
	
	IF(@IsValid=1)
	BEGIN
		IF(@LeadOwnerId IS NULL AND @ExcelInquiryId IS NULL)--Indicate that excel imported 1st time.Request from import page.y
		BEGIN
			IF EXISTS(	SELECT 1 
					FROM TC_InquiriesLead AS TCIL
					JOIN TC_CustomerDetails AS C ON C.Id=TCIL.TC_CustomerId
					WHERE TCIL.TC_LeadInquiryTypeId=@InquiryType 
					AND TCIL.TC_LeadStageId<>3 
					AND TCIL.TC_UserId IS NOT NULL
					AND C.IsleadActive=1 
					AND C.Mobile=@Mobile
					AND C.BranchId=@BranchId)
							
			BEGIN
			
				--If exists then get LeadOwnerId	
	 			SELECT @LeadOwnerId=TCIL.TC_UserId FROM TC_InquiriesLead AS TCIL
				 JOIN TC_CustomerDetails AS C ON C.Id=TCIL.TC_CustomerId
				WHERE TCIL.TC_LeadInquiryTypeId=@InquiryType 
				AND TCIL.TC_LeadStageId<>3 
				AND TCIL.TC_UserId IS NOT NULL
				AND C.IsleadActive=1
				AND C.Mobile=@Mobile
				AND C.BranchId=@BranchId
				
				SET @IsNewInq=0
				SET @AutoVerified=1
			END
		END	
	END
	
	--Check inquiry is exists in respective Inquiry table
	
	BEGIN
		IF (@InquiryType=1)
		BEGIN
			IF(@ExcelInquiryId IS NULL)
			BEGIN
			
				--Insert inquiry in respective Import Inquiry table
				INSERT INTO TC_ImportBuyerInquiries (Name,Email,Mobile,Location,Price,IsValid,UserId,BranchId,MaxPrice,MinPrice,FromYear,ToYear,LeadOwnerId,
							Eagerness,BuyingTime,EntryDate,CarYear,CarDetails, Comments, TC_InquirySourceId,CarMake,CarModel,CarVersion, ExcelSheetId)
				SELECT	@Name,@Email,@Mobile,@Location,@Price,@IsValid,@UserId,@BranchId,@MaxPrice,@MinPrice,@FromMakeYear,@ToMakeYear,@LeadOwnerId,
						@Eagerness,@BuyingTime,GETDATE(),@CarYear,@CarDetails , @Comments , @TC_InquirySourceId, @Make, @Model, @Version, @ExcelSheetId 			 
				WHERE NOT EXISTS (	SELECT	1  
									FROM	TC_ImportBuyerInquiries 
									WHERE	Email =@Email AND Mobile =@Mobile 
											AND (CarDetails=@CarDetails )
											AND BranchId=@BranchId 
											AND IsDeleted=0
											AND TC_BuyerInquiriesId IS NULL)
											
				SET @ExcelInquiryId = SCOPE_IDENTITY()
				
			END
			--ELSE IF(@ExcelInquiryId IS NOT NULL)
			--BEGIN
			--	SELECT	@Name=B.Name,@Email=B.Email,@Mobile= B.Mobile,@Location= B.Location,@Price= B.Price,@IsValid= B.IsValid ,@UserId= B.UserId,
			--			@MaxPrice=B.MaxPrice,@MinPrice=B.MinPrice,@FromMakeYear=B.FromYear,@ToMakeYear=B.ToYear,@Eagerness=B.Eagerness,@BuyingTime=B.BuyingTime,
			--			@CarYear= B.CarYear,@CarDetails = B.CarDetails, @Comments = B.Comments, @TC_InquirySourceId= B.TC_InquirySourceId, @Make= B.CarMake,
			--			@Model= B.CarModel,@Version=B.CarVersion,@IsNewInq=B.IsNew
			--	FROM	TC_ImportBuyerInquiries B
			--	WHERE	BranchId=@BranchId AND IsDeleted=0
			--			AND TC_ImportBuyerInquiriesId=@ExcelInquiryId
						
			--	SET @IsNewInq=0
			--END
			
			IF(@IsNewInq=0 AND @IsValid=1)
			BEGIN
				
				--Insert inquiry in respective Inquiry table if it is valid and already lead exist
				EXEC TC_INQBuyerSave @AutoVerified ,@BranchId ,NULL,@Name,@Email,@Mobile,@Location,@BuyingTime,@Comments,@Eagerness,NULL,@TC_InquirySourceId ,
				@LeadOwnerId ,@UserId ,@MinPrice ,@MaxPrice ,@FromMakeYear ,@ToMakeYear ,NULL,NULL,NULL ,NULL ,NULL ,NULL ,NULL,@InqStatus OUTPUT,NULL ,NULL,
				@LeadDivertedTo  OUTPUT,@TC_InquiryId  OUTPUT ,@CarDetails ,@ExcelInquiryId  ,@TC_InquirySourceId,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
				@LeadIdOutput OUTPUT
				
				SET @IsUnassigned=0
			END
			ELSE IF( @IsValid=1 AND @IsNewInq=1)
			BEGIN
				SET @IsUnassigned=1
			END
			--ELSE
			--BEGIN
			--	SET @IsUnassigned=0
			--END					
		END		
		
		ELSE IF (@InquiryType=2)
		BEGIN
			IF(@ExcelInquiryId IS NULL)
			BEGIN
				INSERT INTO TC_ImportSellerInquiries(Name,Email,Mobile,Address, Location,CarMake,CarModel,CarVersion,Price,RegistrationNo,CarColor, CarMileage, 
						IsValid,UserId,BranchId,TC_InquirySourceId,Eagerness,EntryDate,CarYear, Comments, ExcelSheetId, VersionId,LeadOwnerId)
				SELECT	@Name,@Email,@Mobile,@Address, @Location,@Make,@Model,@Version,@Price,@RegistrationNo, @Color, @Mileage,
						@IsValid,@UserId,@BranchId,@TC_InquirySourceId,@Eagerness,GETDATE(),@CarYear ,@Comments, @ExcelSheetId, @VersionId,@LeadOwnerId
				WHERE NOT EXISTS (	SELECT	TOP 1 TC_ImportSellerInquiriesId 
									FROM	TC_ImportSellerInquiries 
									WHERE	Email =@Email AND Mobile =@Mobile 
											AND CarVersion=@Version
											AND BranchId=@BranchId 
											AND IsDeleted=0
											AND TC_SellerInquiriesId IS NULL)
		
				SET @ExcelInquiryId = SCOPE_IDENTITY()
			END
		/*	ELSE IF(@ExcelInquiryId IS NOT NULL)
			BEGIN
				
				SELECT	@Name=S.Name,@Email=S.Email,@Mobile=S.Mobile,@Eagerness=S.Eagerness,@Address=S.Address, @Location=S.Location,@Make=S.CarMake,
						@Model=S.CarModel,@Version=S.CarVersion,@Price=S.Price,@RegistrationNo=S.RegistrationNo, @Color=S.CarColor, @Mileage=S.CarMileage,
						@IsValid=S.IsValid,@TC_InquirySourceId=S.TC_InquirySourceId,@CarYear=S.CarYear,@Comments=S.Comments,@VersionId=S.VersionId,
						@IsNewInq=S.IsNew
				FROM	TC_ImportSellerInquiries S
				WHERE	BranchId=@BranchId AND IsDeleted=0
						AND TC_ImportSellerInquiriesId =@ExcelInquiryId
				
				SET @IsNewInq=0
			END */
				
			IF(@IsNewInq=0 AND @IsValid=1)
			BEGIN
				SET @CarYear=CONVERT(INT,@CarYear)
				SET @Price=CONVERT(INT,@Price)
				
				--Insert inquiry in respective Inquiry table if it is valid and already lead exist
				EXEC TC_INQSellerSave NULL,@BranchId,@VersionId,@AutoVerified,@LeadOwnerId,@Name,@Email,@Mobile,@Location,@BuyingTime,@Eagerness,
				@TC_InquirySourceId ,@UserId ,@CarYear, @Price,NULL,@Color ,NULL ,@RegistrationNo ,NULL,NULL ,NULL ,NULL ,NULL ,NULL,@Mileage ,0 , 
				0 ,NULL ,NULL ,NULL ,NULL ,NULL , NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,
				@InqStatus  OUTPUT,@LeadDivertedTo  OUTPUT,@ExcelInquiryId ,@TC_InquirySourceId,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
				@LeadIdOutput OUTPUT
				
				SET @IsUnassigned=0
			END
			ELSE IF( @IsValid=1 AND @IsNewInq=1)
			BEGIN
				SET @IsUnassigned=1
			END
			--ELSE
			--BEGIN
			--	SET @IsUnassigned=0
			--END	
		END
		
		ELSE IF (@InquiryType=3)
		BEGIN
			IF(@ExcelInquiryId IS NULL)
			BEGIN
				INSERT INTO TC_ExcelInquiries(Name,Email,Mobile,City,CarMake,CarModel,Isvalid,UserId,BranchId,TC_InquirySourceId,EntryDate, LeadOwnerId,
							ExcelSheetId,CityId,VersionId,Comments)
				SELECT	@Name,@Email,@Mobile,@Location,@Make,@Model,@IsValid,@UserId,@BranchId,@TC_InquirySourceId,GETDATE(),@LeadOwnerId,
							@ExcelSheetId,@CityId,@VersionId,@Comments
							  
				WHERE NOT EXISTS (
						SELECT	1 
						FROM	TC_ExcelInquiries 
						WHERE	Email =@Email AND Mobile =@Mobile AND CarModel=@Model AND  
								BranchId=@BranchId AND IsDeleted=0 AND TC_NewCarInquiriesId IS NULL)
								
								
				SET @ExcelInquiryId = SCOPE_IDENTITY()
			END
		/*	ELSE IF(@ExcelInquiryId IS NOT NULL)
			BEGIN
				SELECT	@Name=E.Name,@Email=E.Email,@Mobile=E.Mobile,@IsValid=E.IsValid,@TC_InquirySourceId=E.TC_InquirySourceId,
						@CityId=E.CityId,@VersionId=E.VersionId,@IsNewInq=E.IsNew
				FROM	TC_ExcelInquiries E
				WHERE	BranchId=@BranchId AND IsDeleted=0
						AND Id =@ExcelInquiryId						
						
				SET @IsNewInq=0
			END*/
			
			
			IF(@IsNewInq=0 AND @IsValid=1)
			BEGIN
				DECLARE @IsNew BIT
				DECLARE @PqRequestDate DATETIME = GETDATE()
				--Insert inquiry in respective Inquiry table if it is valid and already lead exist
				EXEC TC_INQNewCarBuyerSave @Name,@Email,@Mobile,@VersionId ,@CityId ,@BuyingTime ,@TC_InquirySourceId ,@Eagerness ,NULL ,@AutoVerified ,
				@BranchId ,@LeadOwnerId ,@UserId ,@InqStatus  OUTPUT, @PqRequestDate ,NULL ,NULL  ,NULL  ,NULL  ,NULL ,NULL ,@TC_InquiryId  OUTPUT,
				@LeadDivertedTo OUTPUT,	@IsNew OUTPUT,@TC_InquirySourceId,@ExcelInquiryId ,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
				@LeadIdOutput OUTPUT	
				
				SET @IsUnassigned=0			
			END	
			ELSE IF( @IsValid=1 AND @IsNewInq=1)
			BEGIN
				SET @IsUnassigned=1
			END
			--ELSE
			--BEGIN
			--	SET @IsUnassigned=0
			--END			
		END	
		
		--IF(@ExcelInquiryId IS NOT NULL AND @LeadIdOutput IS NOT NULL)
		--BEGIN
		--	EXEC TC_INQUnassignedLeadAssignment @BranchId =@BranchId,@UserID =@LeadOwnerId,@InqLeadIds=@LeadIdOutput ,@ModifiedBy=@UserId 			  			
		--END
		
	END
						
END
