IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateImportExcelInq]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateImportExcelInq]
GO

	-- =============================================  
-- Author:  Tejashree Patil  
-- Create date: 5 March 2013  
-- Description: Update content of inquiries imported from excel sheet.  
-- =============================================  
CREATE PROCEDURE  [dbo].[TC_UpdateImportExcelInq]  
 -- Add the parameters for the stored procedure here  
 @Name VARCHAR(50),  
 @Mobile VARCHAR(50),  
 @Email VARCHAR(50),  
 @City VARCHAR(50),  
 @Make VARCHAR(50),  
 @Model VARCHAR(50),  
 @BranchId BIGINT,  
 @Id BIGINT,
 --@InqSourceId TINYINT,
 --@UserId BIGINT,
 @ValidInqIds VARCHAR(MAX) = NULL,
 @Status TINYINT OUTPUT 
AS  
BEGIN  
	SET @Status = 0  
  
	IF(@ValidInqIds IS NULL)
	BEGIN
		
		DECLARE @PrevCity VARCHAR(50),@PrevMake VARCHAR(50), @PrevModel VARCHAR(50) 
		 
		SELECT	@PrevCity=City,@PrevMake=CarMake,@PrevModel=CarModel
		FROM	TC_ExcelInquiries 
		WHERE	BranchId=@BranchId 
				AND Id=@Id
				AND (IsValid IS NULL OR IsValid=0)
		
		IF(@PrevCity IS NULL  OR @PrevCity<>@City)
		BEGIN 
			IF NOT EXISTS (SELECT Id FROM Cities WHERE Name=@PrevCity)
			BEGIN 			
				IF (@PrevCity IS NULL)
				BEGIN				
					UPDATE	TC_ExcelInquiries  
					SET		City=@City
					WHERE	BranchId=@BranchId   
							AND Id =@Id
							AND (IsValid IS NULL OR IsValid=0)
				END
				ELSE
				BEGIN
					UPDATE	TC_ExcelInquiries  
					SET		City=@City
					WHERE	BranchId=@BranchId   
							AND TC_NewCarInquiriesId IS NULL
							AND (IsValid IS NULL OR IsValid=0)
							AND ( ISNULL(City,1)=ISNULL(@PrevCity,1))
				END
			END
			ELSE
			BEGIN
			
				IF NOT EXISTS (	SELECT TD.CityId 
								FROM TC_DealerCities AS TD 
								INNER JOIN Cities AS C  on TD.CityID = C.ID         
								WHERE TD.DealerId = @BranchId AND TD.IsActive = 1 AND C.Name=@PrevCity)
				BEGIN 		
					UPDATE	TC_ExcelInquiries  
					SET		City=@City
					WHERE	BranchId=@BranchId   
							AND TC_NewCarInquiriesId IS NULL
							AND (IsValid IS NULL OR IsValid=0)
							AND ( ISNULL(City,1)=ISNULL(@PrevCity,1))
				END
				ELSE
				BEGIN
					UPDATE	TC_ExcelInquiries  
					SET		City=@City
					WHERE	BranchId=@BranchId   
							AND Id=@Id
				END
			END
		END
		
		IF(@PrevModel<>@Model)
		BEGIN
			IF NOT EXISTS (SELECT Id FROM CarModels WHERE Name=@PrevModel)
			BEGIN 
				UPDATE	TC_ExcelInquiries  
				SET		CarModel=@Model,CarMake=@Make
				WHERE	BranchId=@BranchId   
						AND TC_NewCarInquiriesId IS NULL
						AND (IsValid IS NULL OR IsValid=0)
						AND ( ISNULL(CarModel,1)=ISNULL(@PrevModel,1))
			END
			ELSE
			BEGIN
				UPDATE	TC_ExcelInquiries  
				SET		CarModel=@Model,CarMake=@Make
				WHERE	BranchId=@BranchId   
						AND Id=@Id
			END
		END
		
		-- Insert statements for procedure here  
		UPDATE	TC_ExcelInquiries  
		SET		Name=@Name,Mobile=@Mobile,Email=@Email,IsValid=NULL--,City=@City,CarMake=@Make,CarModel=@Model
				--,UserId = @UserId, TC_InquirySourceId=@InqSourceId
		WHERE	BranchId=@BranchId 
				AND Id=@Id 
		
		SET @Status = 1 
	END
    ELSE
    BEGIN
		UPDATE	TC_ExcelInquiries  
		SET		IsValid=1
		WHERE	BranchId=@BranchId 
				AND Id IN (SELECT ListMember FROM fnSplitCSV(@ValidInqIds))
		
		SET @Status = 2 
    END
END
