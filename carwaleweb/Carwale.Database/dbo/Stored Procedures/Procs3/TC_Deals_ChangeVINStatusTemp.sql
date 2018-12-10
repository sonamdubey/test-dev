IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_ChangeVINStatusTemp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_ChangeVINStatusTemp]
GO

	--======================================================
-- Author		: Tejashree Patil
-- Created On	: 7 Jan 2016
-- Discription	: Change status of VIN number.
-- DECLARE @IsUpdated BIT EXEC [TC_Deals_ChangeVINStatus] NULL,193,2,123,@IsUpdated OUTPUT SELECT @IsUpdated
--======================================================
CREATE PROCEDURE [dbo].[TC_Deals_ChangeVINStatusTemp]
	@TC_Deals_StockId	VARCHAR(500) = NULL,	
	@VINId				INT = NULL,	
	@NextStatus			INT,
	@UserId				INT,
	@IsUpdated			BIT = NULL OUTPUT
AS
BEGIN
	--SELECT * FROM TC_Deals_StockVIN WHERE TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))
	--SELECT * FROM TC_Deals_Stock WHERE id IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))

	DECLARE @IsCarwaleUser BIT = 0, 
			@CurrentStatus INT = NULL,
			@UpdatedCount  INT = 0 ,
			@IsApproved BIT = 0,
			@DealsStockId BIGINT,
			@DummyStatus INT
	
	SET @IsUpdated = 0	

	-- Previous Status of Given VIN ststus
	IF (@VINId IS NOT NULL)
	BEGIN
		SELECT	@CurrentStatus = Status 
		FROM	TC_Deals_StockVIN WITH(NOLOCK)
		WHERE	TC_DealsStockVINId = @VINId

		SELECT	@IsApproved = ISNULL(S.IsApproved,0),
				@DealsStockId = S.Id
		FROM	TC_Deals_StockVIN AS SV WITH(NOLOCK)
				INNER JOIN TC_Deals_Stock AS S WITH(NOLOCK) ON S.Id = SV.TC_Deals_StockId
		WHERE	SV.TC_DealsStockVINId = @VINId
	END

	--Check for CarWale User
	SELECT	@IsCarwaleUser = IsCarwaleUser
	FROM	TC_Users U WITH(NOLOCK)
	WHERE	Id=@UserId


	--SELECT	@IsCarwaleUser '@IsCarwaleUser', @CurrentStatus '@CurrentStatus', @NextStatus '@NextStatus', @VINId '@VINId', @TC_Deals_StockId '@TC_Deals_StockId'
	
	IF (@CurrentStatus IN (5,6,8,9,11,12,14) AND @NextStatus = 2 AND @IsApproved = 0)
	BEGIN

		IF EXISTS(	
					SELECT	TC_DealsStockVINId 
					FROM	TC_Deals_StockVIN WITH(NOLOCK) 
					WHERE	TC_Deals_StockId = @DealsStockId 
							AND STATUS = 3
				 )
		BEGIN 
			SET @DummyStatus = 3
		END
		ELSE 
		BEGIN
			SET @DummyStatus = 1 
		END
	END

	IF (ISNULL(@CurrentStatus,0) = 1 AND (@IsCarwaleUser = 1 AND ISNULL(@NextStatus,0) IN(1,2,3)/*OR(@IsCarwaleUser = 0 AND ISNULL(@NextStatus,0)=12)*/))
		/*
			Unapporve (1)		:	No status change
			Approve	(2)			:	Active
			Rejected Stock (3)	:	Rejected Stock
		*/
		OR  
		 (ISNULL(@CurrentStatus,0) < 1 AND ISNULL(@NextStatus,0) IN(1,2,3,12))
		 /*
			Upapprove - 1	: SKU other parameters changed by Dealer then it will go to unapprove status (here VINId will be NULL and StockId will get in input)
		 */
		OR
		 ( @CurrentStatus = 2 AND ISNULL(@NextStatus,0) IN(1,4,5,11,12,13))
		 /*
			Refresh							:	No status change
			Unapproved - 1					:	If price,interior color or offers are changed, goes to Unapproved state - 1
			Dealer Blocked - 11				:	Block
			Unavailable - 12				:	Unavailable
			Block Online (Open) - 4			:	When customer blocks any VIN by maing payment online
			Blocked Online(Confirmed) - 5	:	When dealer assignes particular VIN to an online blocked(open) inquiry
		 */
		OR
		 ( @CurrentStatus = 3 AND (ISNULL(@NextStatus,0) IN (1,2))) 
		 /*
			Unapprove(1)	:	if price,interior color and offers are changed, goes to unapproved state - 1
			Active			:	if while editing SKU details, same SKU exists , transfer all VINs to that particular SKU and Change VIN status accordingly. 
								(VIN status= 1 for isapproved=0 and VIN status= 2 for isapproved=1)
		 */
		OR
		 ( @CurrentStatus = 4 AND ISNULL(@NextStatus,0) IN(2,5,12)) 
		 /*
																					Confirm	Blocked Online(Confirmed) - 5
																					Reject	Unavailable - 12
			When dealer changes randomly assigned VIN to Online blocked inquiry	:	Active - 2
		 
		 */
		OR
		 ( @CurrentStatus = 5 AND ISNULL(@NextStatus,0) IN(1,2,3,6,9)
			--AND ((@IsCarwaleUser = 1 AND ISNULL(@NextStatus,0) IN(2,6)) OR ( @IsCarwaleUser = 0 AND ISNULL(@NextStatus,0) IN(6,9)))
			)
			/*
				Confirm	Car Booked - 6			:						
				Active - 2						:	Cancel by CW Exec		
				Blocked Online(Cancelled) - 9	:	Cancel by Dealer		
			*/
		OR
		 ( @CurrentStatus = 6 AND ISNULL(@NextStatus,0) IN(2,7,8) 
			--AND ((@IsCarwaleUser = 1 AND ISNULL(@NextStatus,0) IN(2,7)) OR (@IsCarwaleUser = 1 AND ISNULL(@NextStatus,0) IN(7,8)))
			)
			/*
				Deliver	(7)		:										
				Active - 2		:	Cancel by CW Exec		
				Cancelled - 8	:	Cancel byDealer	Booking 	
			*/
		OR
		 ( @CurrentStatus = 8 AND @IsCarwaleUser = 1 AND ISNULL(@NextStatus,0) IN(2,5,6))
		 /*
			Active - 2												 :	Approve Cancellation 
			Blocked Online(Confirmed) - 5/, Car Booked-6 Accordingly :	Revert Cancellation	
		 */
		OR
		 ( @CurrentStatus = 9 AND @IsCarwaleUser = 1 AND ISNULL(@NextStatus,0) IN(1,2,3,5,6))
		  /*
			Active - 2												 :	Approve Cancellation 
			Blocked Online(Confirmed) - 5/, Car Booked-6 Accordingly :	Revert Cancellation	
		 */
		OR
		 ( @CurrentStatus = 11 AND ISNULL(@NextStatus,0) IN(2,14))
		 /*
			Dealer Booked - 14	:	Confirm Booking	
			Active - 2			:	Cancel Blocking	
		 */
		OR
		 ( @CurrentStatus IN (12,14) AND ISNULL(@NextStatus,0) IN(1,2,7,8))
		 /*	
			If price,interior color and offers are changed, goes to unapproved state - 1	:	Reupload with changes		
			If nothing changes, it goes to Active -2										:	Reupload without changes	
		 */
		
	BEGIN
	
		--SELECT 'Before Update'
		-- Update status of VIN
		UPDATE	SV 
		SET		SV.Status = CASE
								 WHEN	@CurrentStatus = 5 AND @NextStatus = 2 AND @IsCarwaleUser = 1 AND S.IsApproved = 1 THEN 2
								 WHEN	@CurrentStatus = 5 AND @NextStatus = 2 AND @IsCarwaleUser = 1 AND S.IsApproved = 0 THEN @DummyStatus
								 WHEN	@CurrentStatus = 5 AND @NextStatus = 9 AND @IsCarwaleUser = 0 THEN 9

								 WHEN	@CurrentStatus = 6 AND @NextStatus = 2 AND S.IsApproved  = 0 THEN @DummyStatus  --cw-user
								 WHEN	@CurrentStatus = 6 AND @NextStatus = 8 AND @IsCarwaleUser = 0 THEN 8   --dealer
							
								 WHEN   @CurrentStatus = 12 AND @NextStatus = 2 AND S.IsApproved  = 1 THEN 2
								 WHEN   @CurrentStatus = 12 AND @NextStatus = 2 AND S.IsApproved  = 0 THEN @DummyStatus --If Unavailable to reupload(Active) but IsApprove = 0 then we cant change to active(2), change it to unapprove(1)
								 WHEN   @CurrentStatus = 14 AND @NextStatus = 2 AND S.IsApproved  = 0 THEN @DummyStatus
								 WHEN	@CurrentStatus = 14 AND @NextStatus = 2 AND S.IsApproved  = 1 THEN 2

								 WHEN	@CurrentStatus = 9 AND @NextStatus = 2 AND S.IsApproved = 0 THEN @DummyStatus
								 WHEN	@CurrentStatus = 8 AND @NextStatus = 2 AND S.IsApproved = 0 THEN @DummyStatus
								 WHEN	@CurrentStatus = 1 AND @NextStatus = 3 AND S.IsApproved = 0 THEN @DummyStatus

								WHEN	@CurrentStatus = 11 AND @NextStatus = 2  AND S.IsApproved = 1 THEN 2
								 WHEN	@CurrentStatus = 11 AND @NextStatus = 2 AND S.IsApproved = 0 THEN @DummyStatus
                                 ELSE   @NextStatus
							END
		FROM	TC_Deals_StockVIN AS SV WITH(NOLOCK)
				INNER JOIN TC_Deals_Stock AS S WITH(NOLOCK) ON S.Id = SV.TC_Deals_StockId
		WHERE	((ISNULL(isApproved,0) = 1)--For Approved SKU all actions can be taken
				AND 
				(
					(@VINId IS NOT NULL AND TC_DealsStockVINId = @VINId)--update input status without any condition
					OR 
					(ISNULL(@CurrentStatus,4) = 4 AND @NextStatus = 12 AND SV.Status IN (1,2,3)
						AND (TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)) OR (@VINId IS NULL OR SV.TC_DealsStockVINId = @VINId)))--block online open - unavailable then update all vin
					OR 
					(@NextStatus IN (1,2,3) AND @VINId IS NULL 
						AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)) AND SV.Status IN (1,2))
						-- If status is Unapprove(1), Active(2) Update all VINs of stock
					)
				)
				OR
				((ISNULL(IsApproved, 0) = 0)
				AND 
				(
					(
					@NextStatus = 2 AND SV.Status IN (1,2,3) AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))--Unapprove to Active do for all VINs
					OR 
						(@NextStatus <> 2 AND SV.Status NOT IN (1,2,12) AND TC_DealsStockVINId = @VINId)--
					OR 
						(@NextStatus IN (1,2) AND @VINId IS NULL AND SV.Status IN (1,2,3) AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)) AND SV.Status IN (1,3))
							--Unavailable to Active/Unapprove, Reject to Unapprove
					OR 
						(@VINId IS NOT NULL AND TC_DealsStockVINId = @VINId))--update input status without any condition
					OR 
						(((@CurrentStatus IS NULL AND SV.Status IN (1,2,4)) OR (@CurrentStatus = 4  AND SV.Status IN (1,2,4))) AND @NextStatus = 12
							AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)))--block online open - unavailable then update all vin with status 1,2,3
					OR 
						(@NextStatus <> 2 AND SV.Status IN (1,2,12) AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)))
					--OR 
					--	(@NextStatus = 2 AND SV.Status IN (12) AND TC_DealsStockVINId = @VINId)
					)
				)-- For Unapproved SKU only Unapprove to Active can be done.


		SET @UpdatedCount = @@ROWCOUNT 
		
		IF  @UpdatedCount > 0
			SET		@IsUpdated = 1	

		--SELECT @UpdatedCount '@UpdatedCount', @IsUpdated '@IsUpdated111111111111111'

		-- Change TC_Deals_Stock table's IsApprove flag accordingly based on next status Active (2) or Unapprovd(1)
		IF((@VINId IS NULL) AND (@NextStatus IN (1,2)) /*AND @UpdatedCount > 0*/)
		UPDATE	TC_Deals_Stock
		SET		isApproved = CASE	
									WHEN @NextStatus = 1 THEN 0 
									WHEN @NextStatus = 2 THEN 1
							 END
		WHERE	Id IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))
		
		IF @@ROWCOUNT > 0
			SET		@IsUpdated = 1	
		
		--SELECT @IsUpdated '@IsUpdated222222222222222222'

		--Update booking and inquiries table vinid to blank
		IF(@CurrentStatus IN (5,6,8,9,11,14) AND @NextStatus IN (2) AND @IsUpdated > 0)
		BEGIN
			UPDATE	TC_NewCarBooking
			SET		TC_Deals_StockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_Deals_StockVINId = @VINId

			UPDATE	TC_NewCarInquiries
			SET		TC_DealsStockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_DealsStockVINId = @VINId
		END

		IF(@CurrentStatus IN (4) AND @NextStatus IN (12) AND @IsUpdated > 0)
		BEGIN
			UPDATE	TC_NewCarBooking
			SET		TC_Deals_StockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_Deals_StockVINId = @VINId

			UPDATE	TC_NewCarInquiries
			SET		TC_DealsStockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_DealsStockVINId = @VINId
		END
	END

	-- Status = 17 (refresh) then update related columns
	IF(@NextStatus = 17)
	BEGIN
		--SELECT 'Refresh'
		UPDATE	SV 
		SET		SV.LastRefreshedOn = GETDATE(), SV.LastRefreshedBy = @UserId
		FROM	TC_Deals_StockVIN AS SV WITH(NOLOCK)
				INNER JOIN TC_Deals_Stock AS S WITH(NOLOCK) ON S.Id = SV.TC_Deals_StockId
		WHERE	ISNULL(isApproved,0) = 1
				AND (@VINId IS NOT NULL AND TC_DealsStockVINId = @VINId)
				

		IF @@ROWCOUNT > 0
			SET		@IsUpdated = 1	

		--SELECT @IsUpdated '@IsUpdated777777777777777'
	END
END

SELECT	* 
FROM	TC_Deals_StockVIN 
WHERE	TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))

SELECT	* 
FROM	TC_Deals_Stock 
WHERE	Id IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))
	
