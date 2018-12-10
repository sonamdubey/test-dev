IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddBuyerInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddBuyerInquiries]
GO

	




-- Created By:	Surendra
-- Create date: 6th Feb 2012
-- Description:	Adding Buyer's Inquiry with or Without Stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_AddBuyerInquiries]          
@BranchId Numeric,
@StockId BIGINT,

-- TC_CustomerDetails's related param
@CustomerName VARCHAR(100),
@Email VARCHAR(100),
@Mobile VARCHAR(15),
@Location VARCHAR(50),
@Buytime VARCHAR(20),
@CustomerComments VARCHAR(400),

--- Followup related Param
@Comments VARCHAR(500),
@InquiryStatus SMALLINT,
@NextFollowup DATETIME,
@AssignedTo BIGINT,
@InquirySource SMALLINT,

----- Other Param
@UserId BIGINT,
@UsedCarPurchaseInquiryId BIGINT =NULL  --  Avishkar Modified 23 May 2012 to populate UsedCarPurchaseInquiryId
AS           

BEGIN		
	-- inserting record in main table (TC_inquiries) of inquiries
	BEGIN TRY
		BEGIN TRANSACTION ProcessBuyerInquiries
			--getting versionid from tc_stock table
			DECLARE @VersionId INT,@TC_InquiriesId BIGINT
			SET @TC_InquiriesId=NULL
			SELECT @VersionId=VersionId FROM TC_Stock WHERE Id=@StockId
			
			-- following common procedure will add/modified TC_customerDetails,TC_inquiries and TC_inquiriesLead tables
			EXECUTE TC_AddTCInquiries @CustomerName,@Email,@Mobile,@Location,@Buytime,@CustomerComments,@BranchId,@VersionId,@Comments,@InquiryStatus,@NextFollowup,@AssignedTo,1,@InquirySource,@UserId,@TC_InquiriesId OUTPUT,1
					
			-- Insert record in TC_AddBuyerInquiries
			IF(@TC_InquiriesId IS NOT NULL)
			BEGIN
				IF NOT EXISTS( SELECT TOP 1 TC_BuyerInquiriesId FROM TC_BuyerInquiries WHERE TC_InquiriesId=@TC_InquiriesId)
				BEGIN
					INSERT INTO TC_BuyerInquiries(TC_InquiriesId,StockId,UsedCarPurchaseInquiryId) -- Avishkar Modified 23 May 2012 to populate UsedCarPurchaseInquiryId
                                    VALUES(@TC_InquiriesId,@StockId,@UsedCarPurchaseInquiryId) 													
				
					If NOT EXISTS(SELECT StockId FROM TC_StockAnalysis WHERE StockId = @StockId )
					BEGIN
						INSERT INTO TC_StockAnalysis(StockId, CWResponseCount, TCResponseCount) VALUES(@StockId, 0, 0)
					END

					If (@InquirySource = 1 ) -- CarWale as source
						Begin
							-- Update CWResponseCount to Table.
							Update TC_StockAnalysis Set CWResponseCount = CWResponseCount + 1 Where StockId = @StockId
						End
					Else -- Any other source
						Begin
							-- Update TCResponseCount to Table.
							Update TC_StockAnalysis Set TCResponseCount = TCResponseCount + 1 Where StockId = @StockId
						End
				END
			END
			
	
		COMMIT TRANSACTION ProcessBuyerInquiries
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION ProcessBuyerInquiries
		 INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
         VALUES('TC_AddBuyerInquiries',ERROR_MESSAGE(),GETDATE())
		--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END




