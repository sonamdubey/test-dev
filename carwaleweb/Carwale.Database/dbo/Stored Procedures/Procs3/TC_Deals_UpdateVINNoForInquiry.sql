IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_UpdateVINNoForInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_UpdateVINNoForInquiry]
GO
	-- =============================================
-- Created By: Upendra Kumar
-- Created Date:7 Jan ,2016
-- Description: when dealers/CW accepts the blocking then update inquiries with VinId and change the status
-- EXEC TC_Deals_UpdateVINNoForInquiry 13888, 5, 243
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_UpdateVINNoForInquiry]
	@NCDInquiryId INT,
	@DealsStockVINId INT,
	@UserId INT
AS
BEGIN
  DECLARE @ReturnValue INT
  DECLARE @IsUpdated BIT 
  DECLARE @TC_LeadId BIGINT
  DECLARE @DealsStockId BIGINT
  -- Update status of Selected VIN No and store  IsUpdated 

	SELECT @DealsStockId = TC_Deals_StockId From TC_Deals_StockVIN WHERE TC_DealsStockVINId = @DealsStockVINId
	EXEC TC_Deals_ChangeVINStatus NULL, @DealsStockVINId, 5,@UserId,@IsUpdated OUTPUT,@NCDInquiryId

  IF(@IsUpdated IS NOT NULL)             -- IF Updation is Successful then GO inside 
    BEGIN 
		 DECLARE @PreviousINQVinId INT

		-- Select VinID in Which VINNo INQUIRY is done and make it online block 
		 SELECT @PreviousINQVinId = TC_DealsStockVINId FROM TC_NewCarInquiries WITH(NOLOCK) WHERE  TC_NewCarInquiriesId = @NCDInquiryId

		 IF(@DealsStockVINId != @PreviousINQVinId)            -- if online block vin no is changed by CW/Dealers then Make previous Vin free 
			BEGIN
				EXEC TC_Deals_ChangeVINStatus NULL, @PreviousINQVinId, 2 ,@UserId,@IsUpdated OUTPUT,@NCDInquiryId
			END	 
			-- Update TC_NewCarInquiries Table And TC_NewCarBooking table if VInNO is Changed 
			UPDATE TC_NewCarInquiries 
			SET TC_DealsStockVINId = @DealsStockVINId , TC_Deals_StockId = @DealsStockId
			WHERE TC_NewCarInquiriesId = @NCDInquiryId 

			-- Get VinNo To update TC_NewCarBooking based on newlly assigned vinnoid
			DECLARE @VinNo VARCHAR(20)
			SELECT @VinNo = VINNo FROM TC_Deals_StockVIN  WITH(NOLOCK) WHERE TC_DealsStockVINId = @DealsStockVINId
			select @VinNo,@DealsStockVINId

			UPDATE TC_NewCarBooking   
			SET TC_Deals_StockVINId = @DealsStockVINId ,VinNo = @VinNo,TC_Deals_StockId = @DealsStockId
			WHERE TC_NewCarInquiriesId = @NCDInquiryId

			SET @ReturnValue = @DealsStockVINId
    END
  ELSE
     SET @ReturnValue =  -1
 
 RETURN @ReturnValue
END

