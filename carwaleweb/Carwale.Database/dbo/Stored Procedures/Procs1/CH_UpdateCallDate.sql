IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_UpdateCallDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_UpdateCallDate]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: Sep 10, 2009
-- Description:	-- This SP will check if currently no call scheduled for this customer.
				-- And if call is already scheduled of past time then update it to the current time
				-- so that telecallrs will notify new purchase inquiries
-- =============================================
CREATE PROCEDURE [dbo].[CH_UpdateCallDate]
	-- Add the parameters for the stored procedure here
	@CustomerId		NUMERIC, -- Seller Id
	@EventId		NUMERIC,
	@IsCallExists	BIT OUTPUT
AS
BEGIN
	Declare @CallId  NUMERIC, @TbcDateTime	DateTime, @DaysDiff	Int
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @IsCallExists = 0

	-- Note: 
		-- CallType = 7 = CustomerPaymentReminder
		-- CallType = 15 = SellerRegularCall
	SELECT @CallId=CallId, @TbcDateTime=TBCDateTime FROM CH_ScheduledCalls WHERE TbcId = @CustomerId AND  EventId = @EventId AND ( CallType = 7 OR CallType = 15 )
	
	Set @DaysDiff = DATEDIFF(dd,@TbcDateTime,getdate())
	
	IF @CallId > 0 AND @DaysDiff > 0
		BEGIN
			-- if call is already scheduled update TBCDateTime to current datetime
			-- so that telecallers can call as the purchase inquiry comes for this seller
			UPDATE CH_ScheduledCalls SET TBCDateTime = DateAdd(dd,2,GetDate()) WHERE CallId = @CallId
			SET @IsCallExists = 1
		END
END


