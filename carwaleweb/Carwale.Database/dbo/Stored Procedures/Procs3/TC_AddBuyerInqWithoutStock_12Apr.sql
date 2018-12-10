IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddBuyerInqWithoutStock_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddBuyerInqWithoutStock_12Apr]
GO

	
-- Created By:	Surendra
-- Create date: 6 Jan 2012
-- Description:	Adding Buyer's Inquiry without Stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_AddBuyerInqWithoutStock_12Apr]          
@BranchId BIGINT,
-- TC_CustomerDetails's related param
@CustomerName VARCHAR(100),
@Email VARCHAR(100),
@Mobile VARCHAR(15),
@Location VARCHAR(50),
@Buytime VARCHAR(20),
@CustomerComments VARCHAR(400),

-- TC_BuyerInqWithoutStock related
@MinPrice BIGINT,
@MaxPrice BIGINT,
@FromMakeYear SMALLINT,
@ToMAkeYear SMALLINT,
@ModelIds VARCHAR(400),
@ModelNames VARCHAR(400),
@BodyType VARCHAR(200),
@FuelType VARCHAR(20),
@UserId BIGINT,

--- TC_inquiriesLead and TC_InquiriesFollowup related Param
@Comments VARCHAR(500),
@InquiryStatus SMALLINT,
@NextFollowup DATETIME,
@AssignedTo BIGINT,
@InquirySource SMALLINT

AS           

BEGIN	
	BEGIN TRY
		BEGIN TRANSACTION
			--getting versionid from tc_stock table	
			DECLARE @TC_InquiriesId BIGINT
			
			EXECUTE TC_AddTCInquiries @CustomerName,@Email,@Mobile,@Location,@Buytime,@CustomerComments,@BranchId,NULL,@Comments,@InquiryStatus,@NextFollowup,@AssignedTo,1,@InquirySource,@UserId,@TC_InquiriesId OUTPUT		
						
			-- Insert record in TC_AddBuyerInquiriesWithoutStock
			IF (@TC_InquiriesId IS NOT NULL)
			BEGIN
				IF NOT EXISTS( SELECT TOP 1 TC_BuyerInqWithoutStockId FROM TC_BuyerInqWithoutStock WHERE TC_InquiriesId=@TC_InquiriesId)
				BEGIN
					INSERT INTO TC_BuyerInqWithoutStock(TC_InquiriesId ,MinPrice,MaxPrice,FromMakeYear,ToMakeYear,BodyType,FuelType,ModelNames,ModelIds,CreatedBy)  
					VALUES(@TC_InquiriesId,@MinPrice,@MaxPrice,@FromMakeYear,@ToMAkeYear,@BodyType,@FuelType,@ModelNames,@ModelIds,@UserId)  
				END
			END
									
			-- Finnally inserting or updating record in TC_InquiriesLead table	
		COMMIT TRANSACTION
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN
		--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END



