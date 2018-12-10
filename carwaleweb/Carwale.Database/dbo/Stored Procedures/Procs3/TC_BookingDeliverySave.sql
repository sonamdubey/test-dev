IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BookingDeliverySave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BookingDeliverySave]
GO

	

-- Modified By:	Surendra
-- Create date: 4 Nov 2011
-- Description:	Updating TC_Stock and Tc_carbooking for car booked
-- Modified   : Tejashree Patil on Date-16th Oct 2012 ,Added parameter @DealerId while executing TC_StockStatusChange SP
-- Modified By : Nilesh Utture on 14th February 2013, EXEC TC_changeInquiryDisposition When buyerInquiry is marked as sold
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE"
-- Modified by: Nilesh Utture on 24th April,2013 passed extra parameter @UsersId to SP TC_changeInquiryDisposition and added it newly to this sp
-- =============================================
CREATE PROCEDURE [dbo].[TC_BookingDeliverySave]
(
@StockId INT ,
@DealerId INT,
@TC_BookingDelivery_Id INT=NULL, 
@IsDocDelivered BIT =NULL, 
@IsCarChecked BIT=NULL,
@IsAccessoriesGiven BIT =NULL,
@TC_BookingWarranties_Id INT=NULL,
@TC_BookingServices_Id INT=NULL,
@IsCarDelivered BIT=NULL,
@DeliveryDate DATE =NULL,
@Comments VARCHAR(400)=NULL,
@UserId INT = NULL, -- Modified by: Nilesh Utture on 24th April,2013
@Status INT OUTPUT
)	
AS
BEGIN

	DECLARE @BuyerInqId BIGINT
	DECLARE @InquiriesLeadId BIGINT
	DECLARE @LeadId BIGINT
	DECLARE @CustomerId BIGINT
	--DECLARE @UserId BIGINT


	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status=0
	IF EXISTS(SELECT Id	FROM TC_Stock WHERE Id=@StockId AND BranchId=@DealerId)--checking valid stock id and dealerid
		BEGIN
			DECLARE @TC_CarBooking_Id INT
			SELECT @TC_CarBooking_Id=TC_CarBookingId, @CustomerId = CustomerId FROM TC_Carbooking WHERE stockId=@StockId--taking bookingid as per stockid
			IF(@TC_BookingDelivery_Id IS NULL AND @TC_CarBooking_Id IS NOT NULL)--checking @TC_BookingDelivery_Id null or not.if null insert
				BEGIN
					INSERT INTO TC_BookingDelivery(TC_CarBooking_Id, IsDocDelivered, IsCarChecked, IsAccessoriesGiven, TC_BookingWarranties_Id, TC_BookingServices_Id, IsCarDelivered, DeliveryDate, Comments)
					VALUES(@TC_CarBooking_Id, @IsDocDelivered, @IsCarChecked, @IsAccessoriesGiven, @TC_BookingWarranties_Id, @TC_BookingServices_Id, @IsCarDelivered, @DeliveryDate, @Comments)		
					
					-- Making car as sold
					EXECUTE TC_StockStatusChange @StockId,3,@DealerId -- Modified   : Tejashree Patil on Date-16th Oct 2012 
					UPDATE TC_CarBooking SET IsCompleted=1 WHERE TC_CarBookingId=@TC_CarBooking_Id									
					SET @Status=SCOPE_IDENTITY()
					
					-- Modified By : Nilesh Utture on 14th February 2013
					SELECT @BuyerInqId = B.TC_BuyerInquiriesId FROM TC_BuyerInquiries B 
					INNER JOIN TC_InquiriesLead L ON B.TC_InquiriesLeadId = L.TC_InquiriesLeadId
					WHERE B.StockId = @StockId AND L.TC_CustomerId = @CustomerId AND L.IsActive = 1
					
					UPDATE TC_BuyerInquiries SET TC_LeadDispositionId = 4, BookingDate = GETDATE(), BookingStatus = 42 
					WHERE TC_BuyerInquiriesId = @BuyerInqId
					EXEC TC_changeInquiryDisposition @BuyerInqId,4,1, @UserId -- EXEC TC_changeInquiryDisposition InqId, LeadDispositionId, InquiryType -- Modified by: Nilesh Utture on 24th April,2013
				END
				ELSE
				BEGIN --if @TC_BookingDelivery_Id not null update row
					IF(@TC_CarBooking_Id IS NOT NULL)--If booking id not null
						BEGIN
							UPDATE TC_BookingDelivery SET TC_CarBooking_Id=@TC_CarBooking_Id, IsDocDelivered=@IsDocDelivered, IsCarChecked=@IsCarChecked, IsAccessoriesGiven=@IsAccessoriesGiven, 
							TC_BookingWarranties_Id=@TC_BookingWarranties_Id, TC_BookingServices_Id=@TC_BookingServices_Id, IsCarDelivered=@IsCarDelivered, DeliveryDate=@DeliveryDate, Comments=@Comments 
							WHERE TC_BookingDelivery_Id=@TC_BookingDelivery_Id
							 
							EXECUTE TC_StockStatusChange @StockId,3	,@DealerId -- Modified   : Tejashree Patil on Date-16th Oct 2012 
							UPDATE TC_CarBooking SET IsCompleted=1 WHERE TC_CarBookingId=@TC_CarBooking_Id	
							
							SET @Status=@TC_BookingDelivery_Id
							 
						END
						ELSE
						BEGIN
							SET @Status=-1
						END
				END
		END
END





/****** Object:  StoredProcedure [dbo].[TC_CustomerDetails_SP]    Script Date: 02/20/2013 15:55:07 ******/
SET ANSI_NULLS ON

