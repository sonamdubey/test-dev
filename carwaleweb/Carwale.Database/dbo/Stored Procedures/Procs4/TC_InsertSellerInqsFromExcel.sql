IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertSellerInqsFromExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertSellerInqsFromExcel]
GO

	
-- =============================================
-- Author:		Tejashree Patil
-- Create date: 21 March 2013
-- Description:	Insert all seller inquiires from excel sheet.
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertSellerInqsFromExcel]
	@Name VARCHAR(100),  
	@Email VARCHAR(100),  
	@Mobile VARCHAR(15),
	@CarMake VARCHAR(50),  
	@CarModel VARCHAR(50),  
	@CarVersion VARCHAR(50),  
	@Address VARCHAR(100),
	@Location VARCHAR(50),
	@Price INT,
	@CarYear INT,   
	@IsValid BIT,
	@TC_InquirySourceId TINYINT ,   
	@TC_InquiryOtherSourceId TINYINT ,  
	@Eagerness TINYINT, 
	@CarMileage INT,  
	@CarColor VARCHAR(50), 
	@RegistrationNo VARCHAR(50), 
	@Comments VARCHAR(500),
	@UserId BIGINT,  
	@BranchId BIGINT , 
	@Id BIGINT,
	@ValidInqIds VARCHAR(MAX) = NULL ,
	@Status TINYINT OUTPUT 
AS
BEGIN
		
	SET @Status = 0 

	IF(@Id IS NULL AND @ValidInqIds IS NULL)
	BEGIN
		
		INSERT INTO TC_ImportSellerInquiries(Name,Email,Mobile,Address, Location,CarMake,CarModel,CarVersion,Price,RegistrationNo,CarColor, CarMileage, 
					IsValid,UserId,BranchId,TC_InquirySourceId,TC_InquiryOtherSourceId,Eagerness,EntryDate,CarYear, Comments)
		SELECT		@Name,@Email,@Mobile,@Address, @Location,@CarMake,@CarModel,@CarVersion,@Price,@RegistrationNo, @CarColor, @CarMileage,
					@IsValid,@UserId,@BranchId,@TC_InquirySourceId, @TC_InquiryOtherSourceId,@Eagerness,GETDATE(),@CarYear ,@Comments				
		WHERE NOT EXISTS (	SELECT	TOP 1 TC_ImportSellerInquiriesId 
							FROM	TC_ImportSellerInquiries 
							WHERE	Email =@Email AND Mobile =@Mobile 
									AND CarVersion=@CarVersion
									AND BranchId=@BranchId 
									AND IsDeleted=0
									AND TC_SellerInquiriesId IS NULL)
								
		SET @Status = 1 
	END
	ELSE
	BEGIN
		IF(@ValidInqIds IS NULL)
		BEGIN
			
			DECLARE @PrevMake VARCHAR(50), @PrevModel VARCHAR(50) 
			 
			SELECT	@PrevMake=CarMake,@PrevModel=CarModel
			FROM	TC_ImportSellerInquiries 
			WHERE	BranchId=@BranchId 
					AND TC_ImportSellerInquiriesId=@Id
					AND (IsValid IS NULL OR IsValid=0)
							
								
			IF( @PrevModel IS NULL OR @PrevModel<>@CarModel)
			BEGIN
				IF NOT EXISTS (SELECT Id FROM CarModels WHERE Name=@PrevModel)
				BEGIN 
				
					IF (@PrevModel IS NULL)
					BEGIN				
						UPDATE	TC_ImportSellerInquiries  
						SET		CarMake=@CarMake, CarModel=@CarModel
						WHERE	BranchId=@BranchId   
								AND TC_ImportSellerInquiriesId=@Id 
								AND (IsValid IS NULL OR IsValid=0)
					END
					ELSE
					BEGIN						
						UPDATE	TC_ImportSellerInquiries  
						SET		CarModel=@CarModel,CarMake=@CarMake
						WHERE	BranchId=@BranchId   
								AND TC_SellerInquiriesId IS NULL
								AND (IsValid IS NULL OR IsValid=0)
								AND ( ISNULL(CarModel,1)=ISNULL(@PrevModel,1))
					END
				END
				ELSE
				BEGIN
					UPDATE	TC_ImportSellerInquiries  
					SET		CarModel=@CarModel,CarMake=@CarMake
					WHERE	BranchId=@BranchId   
							AND TC_ImportSellerInquiriesId=@Id
				END
			END
			
			-- Insert statements for procedure here  
			UPDATE	TC_ImportSellerInquiries  
			SET		Name=@Name,Mobile=@Mobile,Email=@Email, Location=@Location, CarYear=@CarYear, Price=@Price , IsValid=NULL
			WHERE	BranchId=@BranchId 
					AND TC_ImportSellerInquiriesId=@Id 
			
			SET @Status = 1 
		END
		ELSE
		BEGIN
			UPDATE	TC_ImportSellerInquiries  
			SET		IsValid=1
			WHERE	BranchId=@BranchId 
					AND TC_ImportSellerInquiriesId IN (SELECT ListMember FROM fnSplitCSV(@ValidInqIds))
			
			SET @Status = 2 
		END
	END
END

