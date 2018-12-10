IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddBuyerInqWithoutStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddBuyerInqWithoutStock]
GO

	
-- Created By:	Surendra
-- Create date: 6 Jan 2012
-- Description:	Adding Buyer's Inquiry without Stock
-- Modification: By Surendra on 26th July,include customerId so this SP is used in followup also
-- =============================================
CREATE PROCEDURE [dbo].[TC_AddBuyerInqWithoutStock]          
@BranchId BIGINT,
-- TC_CustomerDetails's related param
@CustomerName VARCHAR(100)=NULL,
@Email VARCHAR(100)=NULL,
@Mobile VARCHAR(15)=NULL,
@Location VARCHAR(50)=NULL,
@Buytime VARCHAR(20)=NULL,
@CustomerComments VARCHAR(400)=NULL,

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
@Comments VARCHAR(500)=NULL,
@InquiryStatus SMALLINT=NULL,
@NextFollowup DATETIME=NULL,
@AssignedTo BIGINT=NULL,
@InquirySource SMALLINT,
@BodyTypeNames VARCHAR(200),
@FuelTypeNames VARCHAR(100),
@CustomerId BIGINT=NULL
AS           

BEGIN	
	BEGIN TRY
		BEGIN TRANSACTION ProcessBuyerInquiries
			--getting versionid from tc_stock table	
			
			DECLARE @TC_BuyerInqWithoutStockId BIGINT =NULL

			IF(@CustomerId IS NOT NULL)
			BEGIN
				--DECLARE @CustomerName VARCHAR(50),@Email VARCHAR(100),@Mobile VARCHAR(15),@Location VARCHAR(50),@Buytime VARCHAR(20),@CustomerComments VARCHAR(400)				
				SELECT @CustomerName=C.CustomerName,@Email =C.Email,@Mobile =C.Mobile,@Location =C.Location,@Buytime =C.Buytime,@CustomerComments = C.Comments 
				FROM TC_CustomerDetails C WHERE C.Id=@CustomerId AND BranchId=@BranchId
				
				SELECT @TC_BuyerInqWithoutStockId =TC_BuyerInqWithoutStockId FROM TC_BuyerInqWithoutStock B INNER JOIN TC_Inquiries I
						ON B.TC_InquiriesId=I.TC_InquiriesId AND I.InquiryType=1
					WHERE I.TC_CustomerId=@CustomerId AND I.VersionId IS NULL
			END			
			
			IF(@TC_BuyerInqWithoutStockId IS NULL)
			BEGIN	
				DECLARE @TC_InquiriesId BIGINT							
				EXECUTE TC_AddTCInquiries @CustomerName,@Email,@Mobile,@Location,@Buytime,@CustomerComments,@BranchId,NULL,@Comments,@InquiryStatus,@NextFollowup,@AssignedTo,1,@InquirySource,@UserId,@TC_InquiriesId OUTPUT,1		
							
				-- Insert record in TC_AddBuyerInquiriesWithoutStock
				IF (@TC_InquiriesId IS NOT NULL)
				BEGIN
					IF NOT EXISTS( SELECT TOP 1 TC_BuyerInqWithoutStockId FROM TC_BuyerInqWithoutStock WHERE TC_InquiriesId=@TC_InquiriesId)
					BEGIN
						INSERT INTO TC_BuyerInqWithoutStock(TC_InquiriesId ,MinPrice,MaxPrice,FromMakeYear,ToMakeYear,BodyType,FuelType,ModelNames,ModelIds,CreatedBy, BodyTypeName,FuelTypeName)  
						VALUES(@TC_InquiriesId,@MinPrice,@MaxPrice,@FromMakeYear,@ToMAkeYear,@BodyType,@FuelType,@ModelNames,@ModelIds,@UserId,@BodyTypeNames,@FuelTypeNames)  
					END
				END
			END
			ELSE
			BEGIN
				UPDATE TC_BuyerInqWithoutStock SET MinPrice=@MinPrice,MaxPrice=@MaxPrice,FromMakeYear=@FromMakeYear,ToMakeYear=@ToMAkeYear,BodyType=@BodyType,
					FuelType=@FuelType,ModelNames=@ModelNames,ModelIds=@ModelIds,CreatedBy=@UserId,BodyTypeName=@BodyTypeNames,FuelTypeName=@FuelTypeNames  
					WHERE TC_BuyerInqWithoutStockId=@TC_BuyerInqWithoutStockId
			END
									
			-- Finnally inserting or updating record in TC_InquiriesLead table	
		COMMIT TRANSACTION ProcessBuyerInquiries
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN ProcessBuyerInquiries
		 INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
         VALUES('TC_AddBuyerInqWithoutStock',ERROR_MESSAGE(),GETDATE())
	END CATCH;
END


