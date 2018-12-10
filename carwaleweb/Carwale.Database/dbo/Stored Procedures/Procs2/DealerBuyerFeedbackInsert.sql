IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DealerBuyerFeedbackInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DealerBuyerFeedbackInsert]
GO

	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DealerBuyerFeedbackInsert]
	-- Add the parameters for the stored procedure here
	
	@DealerId			NUMERIC,
	@BuyerId			NUMERIC,
	@ReceivedCall		SMALLINT,
	@RcvComments		NVARCHAR(500),
	@IsHappy			SMALLINT,
	@IsVisited			SMALLINT,
	@VisitComments		NVARCHAR(500),
	@DealStatus			SMALLINT,
	@LikeOtherCar		SMALLINT,
	@WhoMetAtDealer		SMALLINT,
	@MeetComments		NVARCHAR(500),
	@StillLooking		SMALLINT,
	@WhenVisit			SMALLINT,
	@LookComment		NVARCHAR(500),	
	@CallSeller			SMALLINT,
	@SellerComments		NVARCHAR(500),
	@PurcaheseStatus	SMALLINT,
	@MeetSeller			SMALLINT,
	@SellerResponse		SMALLINT,
	@ResponseComment	NVARCHAR(500),
	@PlanToCall			SMALLINT,
	@PlanComment		NVARCHAR(500),
	@BuyerType			SMALLINT,
	@BuyCarRange		SMALLINT,
	@BuyerCity			SMALLINT,
	@BuyerCWComments	NVARCHAR(500),
	@ExecutiveId		NUMERIC,
	@EntryDate			DATETIME,
	@StatusId			NUMERIC OUTPUT
	
AS

BEGIN
	
	INSERT INTO DealerBuyerFBCall
		(DealerId,BuyerId, ReceivedCall, RcvComments, IsHappy, IsVisited, VisitComments,
		DealStatus, LikeOtherCar, WhoMetAtDealer, MeetComments, StillLooking, WhenVisit, 
		LookComment, CallSeller, SellerComments, PurcaheseStatus, MeetSeller, SellerResponse,
		ResponseComment, PlanToCall, PlanComment, BuyerType, BuyCarRange, BuyerCity, BuyerCWComments,
		ExecutiveId, EntryDate)
	VALUES
		(@DealerId, @BuyerId, @ReceivedCall, @RcvComments, @IsHappy, @IsVisited, @VisitComments,
		@DealStatus, @LikeOtherCar, @WhoMetAtDealer, @MeetComments, @StillLooking, @WhenVisit, 
		@LookComment, @CallSeller, @SellerComments, @PurcaheseStatus, @MeetSeller, @SellerResponse,
		@ResponseComment, @PlanToCall, @PlanComment, @BuyerType, @BuyCarRange, @BuyerCity, @BuyerCWComments,
		@ExecutiveId, @EntryDate)
		
		SET @StatusId	= SCOPE_IDENTITY()
	 
END

