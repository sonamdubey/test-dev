IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertBuyerInqsFromExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertBuyerInqsFromExcel]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 19 March 2013
-- Description:	Insert all buyer inquiires from excel sheet.
-- Modified By : Tejashree Patil on 1 April 2013, Removed make , model, version and added CarDetails and Comments fields.
-- =============================================
CREATE PROCEDURE  [dbo].[TC_InsertBuyerInqsFromExcel]
	@Name VARCHAR(100),
	@Email VARCHAR(100),
	@Mobile VARCHAR(15),
	@Location VARCHAR(50),
	@Price VARCHAR(50),
	@CarYear VARCHAR(50),
	@Eagerness TINYINT,
	@BuyingTime VARCHAR(50),
	@IsValid BIT,
	@UserId BIGINT,
	@BranchId BIGINT,
	@TC_InquirySourceId TINYINT,
	@TC_InquiryOtherSourceId TINYINT,
	@Make VARCHAR(50),
	@Model VARCHAR(50),
	@Version VARCHAR(50),
	@CarDetails VARCHAR(100),
	@Comments VARCHAR(100)
AS
BEGIN

	INSERT INTO TC_ImportBuyerInquiries(Name,Email,Mobile,Location,Price,IsValid,UserId,BranchId,TC_InquiryOtherSourceId,
				Eagerness,BuyingTime,EntryDate,CarYear,CarDetails, Comments, TC_InquirySourceId,CarMake,CarModel,CarVersion)
    SELECT		@Name,@Email,@Mobile,@Location,@Price,@IsValid,@UserId,@BranchId,@TC_InquiryOtherSourceId,
				@Eagerness,@BuyingTime,GETDATE(),@CarYear,@CarDetails , @Comments , @TC_InquirySourceId, @Make, @Model, @Version 
	WHERE NOT EXISTS (	SELECT	1  
						FROM	TC_ImportBuyerInquiries 
						WHERE	Email =@Email AND Mobile =@Mobile 
								AND (CarDetails=@CarDetails )
								AND BranchId=@BranchId 
								AND IsDeleted=0
								AND TC_BuyerInquiriesId IS NULL)
END
