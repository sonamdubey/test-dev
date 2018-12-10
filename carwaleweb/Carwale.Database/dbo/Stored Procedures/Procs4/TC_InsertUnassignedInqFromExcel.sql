IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertUnassignedInqFromExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertUnassignedInqFromExcel]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 17 May 2013
-- Description:	To insert unassigned imported excel inquiry.
--DECLARE @LeadIdOutput BIGINT 
--EXEC [TC_InsertUnassignedInqFromExcel] 1,5,72,229,@LeadIdOutput OUTPUT
--SELECT @LeadIdOutput
-- Modified By : Tejashre Patil on 24 Jun 2013, added UserId in seller and new car select query.
-- Modified By : Tejashre Patil on 19 Feb 2014, Fetched userId or LeadOwnerId depending on isSpecialUser condition.
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertUnassignedInqFromExcel]
	@InquiryType TINYINT,
	@BranchId BIGINT,
	@ExcelInquiryId BIGINT,
	@LeadOwnerId BIGINT
AS
BEGIN
	DECLARE @TC_CustomerId BIGINT, @AutoVerified BIT = 1, @TC_InquiryId BIGINT,@LeadIdOutput BIGINT
	DECLARE @InqStatus SMALLINT , @LeadDivertedTo VARCHAR(100),	@LeadOwnId BIGINT,@CustomerId BIGINT
	
	DECLARE @Name VARCHAR(100) ,@Email VARCHAR(100) ,@Location VARCHAR(50) ,@CityId INT,@Address VARCHAR(100) ,@Make VARCHAR(50) ,@Model VARCHAR(50) ,
	@Version VARCHAR(50) ,@VersionId INT,@Color VARCHAR(50) ,@CarYear VARCHAR(4) ,@Mileage VARCHAR(4) ,@BuyingTime VARCHAR(20) ,@CarDetails VARCHAR(1000) ,
	@Eagerness SMALLINT ,@Price VARCHAR(50) ,@RegistrationNo VARCHAR(15) ,@Comments VARCHAR(500) ,@IsValid BIT ,@TC_InquirySourceId SMALLINT,@MinPrice INT,
	@MaxPrice INT,@FromMakeYear SMALLINT,@ToMakeYear SMALLINT,@IsNewInq BIT ,@UserId BIGINT,@Mobile VARCHAR(15),@INQLeadIdOutput BIGINT=-1,
	@LastName VARCHAR(100), @Salutation VARCHAR(10)

	BEGIN
		IF (@InquiryType=1)
		BEGIN
			
			IF(@ExcelInquiryId IS NOT NULL)
			BEGIN
				SELECT	@Name=B.Name,@Email=B.Email,@Mobile= B.Mobile,@Location= B.Location,@Price= B.Price,@IsValid= B.IsValid ,@UserId= B.UserId,
						@MaxPrice=B.MaxPrice,@MinPrice=B.MinPrice,@FromMakeYear=B.FromYear,@ToMakeYear=B.ToYear,@Eagerness=B.Eagerness,@BuyingTime=B.BuyingTime,
						@CarYear= B.CarYear,@CarDetails = B.CarDetails, @Comments = B.Comments, @TC_InquirySourceId= B.TC_InquirySourceId, @Make= B.CarMake,
						@Model= B.CarModel,@Version=B.CarVersion,@IsNewInq=B.IsNew
				FROM	TC_ImportBuyerInquiries B
				WHERE	BranchId=@BranchId AND IsDeleted=0
						AND TC_ImportBuyerInquiriesId=@ExcelInquiryId						
			END	
			
			EXEC TC_INQBuyerSave @AutoVerified ,@BranchId ,NULL,@Name,@Email,@Mobile,@Location,@BuyingTime,@Comments,@Eagerness,NULL,@TC_InquirySourceId ,
			@LeadOwnerId ,@UserId ,@MinPrice ,@MaxPrice ,@FromMakeYear ,@ToMakeYear ,NULL,NULL,NULL ,NULL ,NULL ,NULL ,NULL,@InqStatus OUTPUT,NULL ,NULL,
			@LeadDivertedTo  OUTPUT,@TC_InquiryId  OUTPUT ,@CarDetails ,@ExcelInquiryId  ,@TC_InquirySourceId,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
			@LeadIdOutput OUTPUT,@INQLeadIdOutput OUTPUT
			
		END		
		
		ELSE IF (@InquiryType=2)
		BEGIN
			
			IF(@ExcelInquiryId IS NOT NULL)
			BEGIN
				
				SELECT	@Name=S.Name,@Email=S.Email,@Mobile=S.Mobile,@Eagerness=S.Eagerness,@Address=S.Address, @Location=S.Location,@Make=S.CarMake,
						@Model=S.CarModel,@Version=S.CarVersion,@Price=S.Price,@RegistrationNo=S.RegistrationNo, @Color=S.CarColor, @Mileage=S.CarMileage,
						@IsValid=S.IsValid,@TC_InquirySourceId=S.TC_InquirySourceId,@CarYear=S.CarYear,@Comments=S.Comments,@VersionId=S.VersionId,
						@IsNewInq=S.IsNew, @UserId=S.UserId
				FROM	TC_ImportSellerInquiries S
				WHERE	BranchId=@BranchId AND IsDeleted=0
						AND TC_ImportSellerInquiriesId =@ExcelInquiryId
			END 
			
			SET @CarYear=CONVERT(INT,@CarYear)
			SET @Price=CONVERT(INT,@Price)
			
			EXEC TC_INQSellerSave NULL,@BranchId,@VersionId,@AutoVerified,@LeadOwnerId,@Name,@Email,@Mobile,@Location,@BuyingTime,@Eagerness,
			@TC_InquirySourceId ,@UserId ,@CarYear, @Price,NULL,@Color ,NULL ,@RegistrationNo ,NULL,NULL ,NULL ,NULL ,NULL ,NULL,@Mileage ,0 , 
			0 ,NULL ,NULL ,NULL ,NULL ,NULL , NULL ,NULL ,NULL ,NULL ,NULL ,NULL,NULL,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,NULL ,
			@InqStatus  OUTPUT,@LeadDivertedTo  OUTPUT,@ExcelInquiryId ,@TC_InquirySourceId,@LeadOwnId OUTPUT,@CustomerId OUTPUT,
			@LeadIdOutput OUTPUT,@INQLeadIdOutput OUTPUT
	
		END
		
		ELSE IF (@InquiryType=3)
		BEGIN
			
			IF(@ExcelInquiryId IS NOT NULL)
			BEGIN
				SELECT	@Name=E.Name,@Email=E.Email,@Mobile=E.Mobile,@IsValid=E.IsValid,@TC_InquirySourceId=E.TC_InquirySourceId,
						@CityId=E.CityId,@VersionId=E.VersionId,@IsNewInq=E.IsNew,
						-- Modified By : Tejashre Patil on 19 Feb 2014,In case of specialUser @UserId will be @LeadOwnerId or E.UserId 
						@LastName = E.LastName,@Salutation = E.Salutation, @UserId = CASE WHEN E.IsSpecialUser=1 THEN @LeadOwnerId ELSE E.UserId END
				FROM	TC_ExcelInquiries E
				WHERE	BranchId=@BranchId AND IsDeleted=0
						AND Id =@ExcelInquiryId	
			END
						
			DECLARE @IsNew BIT
			DECLARE @PqRequestDate DATETIME = GETDATE()
			
			EXEC TC_INQNewCarBuyerSave @Name,@Email,@Mobile,@VersionId ,@CityId ,@BuyingTime ,@TC_InquirySourceId ,@Eagerness ,NULL ,@AutoVerified, @BranchId ,@LeadOwnerId,
			@UserId ,@InqStatus  OUTPUT, @PqRequestDate ,NULL ,NULL  ,NULL  ,NULL  ,NULL ,NULL ,@TC_InquiryId  OUTPUT,	@LeadDivertedTo OUTPUT,	@IsNewInq OUTPUT,
			@TC_InquirySourceId,@ExcelInquiryId ,@LeadOwnId OUTPUT,@CustomerId OUTPUT,	@LeadIdOutput OUTPUT, @INQLeadIdOutput OUTPUT,@Comments,
			0, -- Modified By : Tejashre Patil on 19 Feb 2014
			NULL, @Address, NULL,NULL, NULL, NULL, NULL, NULL,@Salutation, @LastName, NULL, NULL, NULL, NULL, NULL, NULL -- Modified By : Tejashre Patil on 19 Feb 2014,
			
		END	
		
		--To divert calls
		IF(@INQLeadIdOutput IS NOT NULL AND @INQLeadIdOutput >0)
		BEGIN
			EXEC TC_INQLeadAssignment @BranchId,@LeadOwnerId,@INQLeadIdOutput,@UserId
		END
	END
						
END

SET ANSI_NULLS ON
