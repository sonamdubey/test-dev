IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_ChangeVINStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_ChangeVINStatus]
GO

	
--======================================================
-- Author		: Tejashree Patil
-- Created On	: 7 Jan 2016
-- Discription	: Change status of VIN number.
-- Modified By  : Ruchira Patil on 22/01/2016 (Changed the datatype of @VINId to varchar(in case of bulk refresh))
-- DECLARE @IsUpdated BIT EXEC [TC_Deals_ChangeVINStatus] NULL,430,5,243,@IsUpdated OUTPUT,14882 SELECT @IsUpdated
-- Modified By : Ashwini Dhamankar on Feb 23,2016 (Added condition to change booking status on dealer booking cancellation)
-- Modified By : Khushaboo Patil on 31 March 2016 (Added condition to verify user on dealer booking cancellation.added exec TC_CallScheduling)
--Modified By : Ashwini Dhamankar on April 22,2016 (Update BookingCancelDate of TC_NewCarInquiries if bookingstatus=31)
--Modified by : Anchal gupta on 12th July, 2016 (Corrected Typecasting of varchar into int using fnSplitCSVValues)
-- Modifier		: Saket on 5th Oct, 2016 added Query for Executing Adv_UpdateLiveDeals
--Modifier		: Saket on 26th Oct, 2016 added distinct to fetch distinct stockids
--Modifier		: Saket on 2nd Nov, 2016 increased the size of Stocks
--======================================================
CREATE PROCEDURE [dbo].[TC_Deals_ChangeVINStatus] 
	@TC_Deals_StockId	VARCHAR(500) = NULL,	
	@VINId				VARCHAR(500) = NULL,	
	@NextStatus			INT,
	@UserId				INT,
	@IsUpdated			BIT = NULL OUTPUT,
	@TC_NewCarInquiriesId BIGINT = NULL
AS
BEGIN
	--SELECT * FROM TC_Deals_StockVIN WHERE TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))
	--SELECT * FROM TC_Deals_Stock WHERE id IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId))
	DECLARE @singleVinId INT
BEGIN TRY
	IF(SELECT COUNT(LISTMEMBER) FROM fnSplitCSVValues(@VINId)) = 1
	BEGIN
		SELECT @singleVinId = LISTMEMBER FROM fnSplitCSVValues(@VINId)
	END
	ELSE
		SET @VINId = @VINId

	DECLARE @IsCarwaleUser BIT = 0, 
			@CurrentStatus INT = NULL,
			@UpdatedCount  INT = 0 ,
			@IsApproved BIT = 0,
			@DealsStockId BIGINT,
			@DummyStatus INT,
			@TC_LeadId BIGINT = NULL,
			@BookingStatus INT,
			@NextCallTo TINYINT = NULL,
			@LeadOwnerId INT = NULL

	SELECT	@TC_LeadId = L.TC_LeadId,@LeadOwnerId = IL.TC_UserId 
	FROM	TC_NewCarInquiries NC WITH(NOLOCK)
			INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON NC.TC_InquiriesLeadId = IL.TC_InquiriesLeadId 
			INNER JOIN TC_Lead L WITH(NOLOCK)ON IL.TC_LeadId = L.TC_LeadId
	WHERE	NC.TC_NewCarInquiriesId = @TC_NewCarInquiriesId
	
	SET @IsUpdated = 0	

	-- Previous Status of Given VIN ststus
	IF (@singleVinId IS NOT NULL)
	BEGIN
		SELECT	@CurrentStatus = Status 
		FROM	TC_Deals_StockVIN WITH(NOLOCK)
		WHERE	TC_DealsStockVINId = @singleVinId

		SELECT	@IsApproved = ISNULL(S.IsApproved,0),
				@DealsStockId = S.Id
		FROM	TC_Deals_StockVIN AS SV WITH(NOLOCK)
				INNER JOIN TC_Deals_Stock AS S WITH(NOLOCK) ON S.Id = SV.TC_Deals_StockId
		WHERE	SV.TC_DealsStockVINId = @singleVinId
	END
	--Check for CarWale User
	SELECT	@IsCarwaleUser = IsCarwaleUser
	FROM	TC_Users U WITH(NOLOCK)
	WHERE	Id=@UserId


	--SELECT	@IsCarwaleUser '@IsCarwaleUser', @CurrentStatus '@CurrentStatus', @NextStatus '@NextStatus', @singleVinId '@VINId', @TC_Deals_StockId '@TC_Deals_StockId'
	
	IF (@CurrentStatus IN (5,12,14,6,8,9,11) AND @NextStatus = 2 AND @IsApproved = 0)
	--If nextStatus is ACtive then check other VINs of the same stock are rejected(3) then input vin is also get rejected(3) else ACtive(1) for these 5,12,14,6,8,9,11 status
	BEGIN

		IF EXISTS(	
					SELECT	TC_DealsStockVINId 
					FROM	TC_Deals_StockVIN WITH(NOLOCK) 
					WHERE	TC_Deals_StockId = @DealsStockId 
							AND STATUS = 3
				 )
			SET @DummyStatus = 3
		ELSE 
			SET @DummyStatus = 1
	END

	IF (@CurrentStatus = 14 AND @NextStatus = 8 AND @IsApproved = 0)
	--If nextStatus is ACtive then check other VINs of the same stock are rejected(3) then input vin is also get rejected(3) else ACtive(1) for these 5,12,14,6,8,9,11 status
	BEGIN

		IF EXISTS(	
					SELECT	TC_DealsStockVINId 
					FROM	TC_Deals_StockVIN WITH(NOLOCK) 
					WHERE	TC_Deals_StockId = @DealsStockId 
							AND STATUS = 3
				 )
			SET @DummyStatus = 3
		ELSE 
			SET @DummyStatus = 1
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
		 ( @CurrentStatus = 2 AND ISNULL(@NextStatus,0) IN(1,4,5,11,12,13,14))  --Modified By : Ashwini Dhamankar on Feb 23,2016  added 14 (for offline booking from MyTask)
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
								 WHEN	@CurrentStatus = 5 AND @NextStatus = 2 AND @IsCarwaleUser = 1 AND S.IsApproved = 0 THEN @DummyStatus	--if any other vin is in (1 or 3)
								 WHEN	@CurrentStatus = 5 AND @NextStatus = 9 AND @IsCarwaleUser = 0 THEN 9	--dealer cancels blocking

								 WHEN	@CurrentStatus = 6 AND @NextStatus = 2 AND S.IsApproved  = 0  THEN @DummyStatus  --cw-user --if any other vin is in (1 or 3)
								 WHEN	@CurrentStatus = 6 AND @NextStatus = 8 AND @IsCarwaleUser = 0 THEN 8   --dealer
							
								 WHEN   @CurrentStatus = 12 AND @NextStatus = 2 AND S.IsApproved  = 1 THEN 2
								 WHEN   @CurrentStatus = 12 AND @NextStatus = 2 AND S.IsApproved  = 0 THEN @DummyStatus		--If Unavailable to reupload(Active) but IsApprove = 0 then we cant change to active(2), change it to unapprove(1) or rejected(3)

								 WHEN   @CurrentStatus = 14 AND @NextStatus IN (2,8) AND S.IsApproved  = 0 THEN @DummyStatus		--if any other vin is in (1 or 3)
								 WHEN	@CurrentStatus = 14 AND @NextStatus IN(2, 8) AND S.IsApproved  = 1 THEN 2


								 WHEN	@CurrentStatus = 9 AND @NextStatus = 2 AND S.IsApproved = 0 THEN @DummyStatus	--if any other vin is in (1 or 3)
								 WHEN	@CurrentStatus = 8 AND @NextStatus = 2 AND S.IsApproved = 0 THEN @DummyStatus	--if any other vin is in (1 or 3)
								 WHEN	@CurrentStatus = 1 AND @NextStatus = 3 AND S.IsApproved = 0 THEN @DummyStatus	--if any other vin is in (1 or 3)

								 WHEN	@CurrentStatus = 11 AND @NextStatus = 2  AND S.IsApproved = 1 THEN 2
								 WHEN	@CurrentStatus = 11 AND @NextStatus = 2  AND S.IsApproved = 0 THEN @DummyStatus		--if any other vin is in (1 or 3)
                                 ELSE   @NextStatus
							END
		FROM	TC_Deals_StockVIN AS SV WITH(NOLOCK)
				INNER JOIN TC_Deals_Stock AS S WITH(NOLOCK) ON S.Id = SV.TC_Deals_StockId
		WHERE	((ISNULL(isApproved,0) = 1)--For Approved SKU all actions can be taken
				AND 
				(
					(@VINId IS NOT NULL AND TC_DealsStockVINId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@VINId)))--update input status without any condition  -- Modified by Anchal gupta on 12th July, 2016 (Removed typecasting of varchar into int)
					OR 
					(ISNULL(@CurrentStatus,4) = 4 AND @NextStatus = 12 AND SV.Status IN (1,2,3)
						AND (TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)) OR (@VINId IS NULL OR SV.TC_DealsStockVINId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@VINId)) )))--block online open - unavailable then update all vin  -- Modified by Anchal gupta on 12th July, 2016 (Removed typecasting of varchar into int)
					OR 
					(@NextStatus IN (1,2,3) AND @singleVinId IS NULL 
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
						(@NextStatus <> 2 AND SV.Status NOT IN (1,2,12) AND TC_DealsStockVINId = @singleVinId)--
					OR 
						(@NextStatus IN (1,2) AND @singleVinId IS NULL AND SV.Status IN (1,2,3) AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)) AND SV.Status IN (1,3))
							--Unavailable to Active/Unapprove, Reject to Unapprove
					OR 
						(@singleVinId IS NOT NULL AND TC_DealsStockVINId = @singleVinId))--update input status without any condition
					OR 
						(((@CurrentStatus IS NULL AND SV.Status IN (1,2,4)) OR (@CurrentStatus = 4  AND SV.Status IN (1,2,4))) AND @NextStatus = 12
							AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)))--block online open - unavailable then update all vin with status 1,2,3
					OR 
						(@NextStatus <> 2 AND SV.Status IN (1,2,12) AND TC_Deals_StockId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)))
					--OR 
					--	(@NextStatus = 2 AND SV.Status IN (12) AND TC_DealsStockVINId = @singleVinId)
					)
				)-- For Unapproved SKU only Unapprove to Active can be done.


		SET @UpdatedCount = @@ROWCOUNT 
		
		IF  @UpdatedCount > 0
			SET		@IsUpdated = 1	

		-- Change TC_Deals_Stock table's IsApprove flag accordingly based on next status Active (2) or Unapprovd(1)
		IF((@singleVinId IS NULL) AND (@NextStatus IN (1,2)) /*AND @UpdatedCount > 0*/)
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
		IF(@CurrentStatus IN (4,5,6,8,9,11,14) AND @NextStatus IN (2,8,12) AND @IsUpdated > 0)   
		BEGIN
		IF(@NextStatus <> 8) --for currentstatus = 6 and nextStatus = 8 update query should not execute
		BEGIN
			UPDATE	TC_NewCarBooking
			SET		TC_Deals_StockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_Deals_StockVINId = @singleVinId

			UPDATE	TC_NewCarInquiries
			SET		TC_DealsStockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_DealsStockVINId = @singleVinId
		END
		ELSE IF(@CurrentStatus = 14 AND @NextStatus = 8)
		BEGIN
			UPDATE	TC_NewCarBooking
			SET		TC_Deals_StockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_Deals_StockVINId = @singleVinId

			UPDATE	TC_NewCarInquiries
			SET		TC_DealsStockVINId = NULL, TC_Deals_StockId = NULL
			WHERE	TC_DealsStockVINId = @singleVinId
		END
		END
		--Added By : Ashwini Dhamankar on Jan 21,2016
		IF(@CurrentStatus IN (2,4,5,6,8,9,14) AND @NextStatus IN (2,5,6,8,9,12) AND @IsUpdated > 0)
		BEGIN
			SET @BookingStatus  = CASE 
										WHEN @CurrentStatus IN (2,4) AND @NextStatus = 5 THEN 99
										WHEN @CurrentStatus = 4 AND @NextStatus = 12 THEN 98  --If reject then it becomes Unavailable --For Current status 4

										WHEN @CurrentStatus = 5 AND @NextStatus = 9 THEN 100   -- dealer cancels online blocking (req is pending)
										WHEN @CurrentStatus = 9 AND @NextStatus = 2 THEN 97 --CarwaleUser accepts cancellation
										WHEN @CurrentStatus = 9 AND @NextStatus = 5 THEN 99   --CarwaleUser rejects blocking cancellation req
										WHEN @CurrentStatus = 5 AND @NextStatus = 2 THEN 97   --cancel blocking by CarwaleUser

										WHEN @CurrentStatus = 6 AND @NextStatus = 8 THEN 101   --dealer cancels booking (req is pending)
										WHEN @CurrentStatus = 8 AND @NextStatus = 2 THEN 31   --Booking cancelled  --dealer action
										WHEN @CurrentStatus = 8 AND @NextStatus = 6 THEN 32   --CarwaleUser rejects booking cancellation req
										WHEN @CurrentStatus = 6 AND @NextStatus = 2 THEN 31   --booking cancel by CarwaleUser

										WHEN @CurrentStatus = 14 AND @NextStatus = 2 THEN 31    --added by Ashwini Dhamankar on Feb 23,2015 (dealer Booking cancellation)
										WHEN @CurrentStatus = 14 AND @NextStatus = 8 THEN 31    --added by Ashwini Dhamankar on Feb 23,2015 (dealer Booking cancellation)
								  END

			SET @NextCallTo =	CASE
										WHEN @CurrentStatus IN (2,4) AND @NextStatus = 5 THEN 2
										WHEN @CurrentStatus = 4 AND @NextStatus = 12 AND @IsCarwaleUser = 1 THEN 2
										WHEN @CurrentStatus = 4 AND @NextStatus = 12 AND @IsCarwaleUser = 0 THEN 1
										WHEN @CurrentStatus = 5 AND @NextStatus = 9 THEN 1 
										WHEN @CurrentStatus = 6 AND @NextStatus = 8 THEN 1
								END
			IF(@BookingStatus IS NOT NULL)
			BEGIN
				UPDATE	TC_NewCarInquiries
				SET		BookingStatus = @BookingStatus
				WHERE	TC_NewCarInquiriesId = @TC_NewCarInquiriesId

				UPDATE	TC_NewCarBooking
				SET		BookingStatus = @BookingStatus
				WHERE	TC_NewCarInquiriesId = @TC_NewCarInquiriesId
			END

			--Added by Ashwini Dhamankar on Feb 23,2015

			IF	(@BookingStatus IN (31,98,97))
			BEGIN
			
				IF EXISTS (SELECT TOP 1 TC_LeadId
						   FROM TC_Lead L WITH (NOLOCK)
						   WHERE L.TC_LeadId = @TC_LeadId
						   AND L.TC_LeadStageId = 1)
				BEGIN 
					EXEC TC_CallScheduling @TC_LeadId  = @TC_LeadId
							,@TC_UsersId = @UserId
							,@TC_CallActionId = 2
							,@Comment = 'Customer Verified'
							,@NextFolloupDate = NULL
							,@TC_LeadDispositionId =NULL
							,@TC_InqLeadOwnerId = @LeadOwnerId
							,@ActionTakenOn = NULL
							,@TC_NextActionId  = NULL
							,@InqStatusId  = NULL
							,@NextCallTo  = NULL
							,@BWLeadInqStatus  = NULL
				END

				EXECUTE TC_changeInquiryDisposition	 @TC_NewCarInquiriesId, @BookingStatus , 3 ,@UserId, NULL, NULL, NULL, NULL, NULL

				IF(@BookingStatus = 31)
				BEGIN
					UPDATE TC_NewCarInquiries 
					SET BookingCancelDate=GETDATE()  
					WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
				END
			END
			IF(@NextCallTo IS NOT NULL)
			BEGIN
				UPDATE TC_Calls SET NextCallTo = @NextCallTo WHERE TC_LeadId = @TC_LeadId
				UPDATE TC_ActiveCalls SET NextCallTo = @NextCallTo WHERE TC_LeadId = @TC_LeadId
			END
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
				AND (TC_DealsStockVINId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@VINId))) -- Modified By Ruchira Patil on 22/01/2016 (multiple vins in case of refresh)
				
		IF @@ROWCOUNT > 0
			SET		@IsUpdated = 1	

		--SELECT @IsUpdated '@IsUpdated777777777777777'
	END
	-- Insert into DCRM_DEALS_ModelStatus
	ELSE
	BEGIN
	IF(@IsUpdated = 1)
	BEGIN
		INSERT INTO DCRM_DEALS_ModelStatus(TC_ModelId)
		SELECT	DISTINCT VW.ModelId
		FROM	TC_Deals_StockVIN SV WITH(NOLOCK)
				INNER JOIN TC_Deals_Stock DS WITH(NOLOCK) ON SV.TC_Deals_StockId = DS.Id
				INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.VersionId = DS.CarVersionId AND ApplicationId = 1
		WHERE	(@TC_Deals_StockId IS NOT NULL AND DS.Id IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@TC_Deals_StockId)))
				OR (@VINId IS NOT NULL AND SV.TC_DealsStockVINId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@VINId)))
		
	END
	END
	--Update the LiveDeals Table
	IF @VINId IS NOT NULL
	BEGIN
		DECLARE @Stocks VARCHAR(500) 
						SELECT  @Stocks = COALESCE(@Stocks + ', ', '') + CAST(TC_Deals_StockId AS VARCHAR)
						FROM 
						(SELECT DISTINCT TC_Deals_StockId 
						FROM TC_Deals_StockVIN WITH(NOLOCK)
						WHERE TC_DealsStockVINId IN (SELECT LISTMEMBER FROM fnSplitCSVValues(@VINId))) Stocks
		EXEC Adv_UpdateLiveDeals @Stocks,NULL,1
	END
	ELSE
	BEGIN
	EXEC Adv_UpdateLiveDeals @TC_Deals_StockId,NULL,1
	END 
END TRY
	BEGIN CATCH
	INSERT INTO CarWaleWebsiteExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
	VALUES('Advantage Chnage VIN Status','dbo.TC_Deals_ChangeVINStatus',ERROR_MESSAGE(),'TC_Deals_StockVIN','0',GETDATE(), CAST(@NextStatus AS VARCHAR(20)) + CAST(@UserId AS VARCHAR(20))
																														 + CASE WHEN @TC_Deals_StockId IS NOT NULL THEN ' ;TC_Deals_StockId: ' +  @TC_Deals_StockId WHEN @TC_Deals_StockId IS NULL THEN '' END
	                                                                                                                     + CASE WHEN @VINId IS NOT NULL THEN ' ; VINId: ' +  @VINId WHEN @VINId IS NULL THEN '' END 
	                                                                                                                     + CASE WHEN @TC_NewCarInquiriesId IS NOT NULL THEN ' ; TC_Deals_StockId: ' +  @TC_NewCarInquiriesId WHEN @TC_NewCarInquiriesId IS NULL THEN '' END )
END CATCH
END

