IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateSalesStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateSalesStatus]
GO

	
--Author	:	Sachin Bharti(16th April 2013)
--Purpose	:	Update DCRM_SalesDealer if new package added or updating 
--				existing packages and also rcord log in DCRM_CPLog 
--Modifier	:	Sachin Bharti(30th April 2013)
--Purpose	:	First make new entry in DCRM_SalesMeeting table if user
--				Insert or Update the packages against the Dealer
--Modifier	:	Sachin Bharti(29th April 2013)
--Purpose	:	Make new entry in DCRM_ProductMeetingLog when new Meeting Id is 
--				crated against the all existing package for the Dealer 
--Modifier	:	Sachin Bharti(23rd June 2014)	
--Purpose	:	Added entry for Dicount,Product,Tax,TDS and final amount
--Modifier	:	Sachin Bharti(8th Aug 2014)
--Purpose	:	Capture PAN and TAN Number if TDS is given 
--Modifier	:	Sachin Bharti(22th Sep 2014)
--Purpose	:	Added package quantity for RSA packages.

CREATE PROCEDURE [dbo].[DCRM_UpdateSalesStatus]

	@SalesId		NUMERIC(18,0),
	@SalesMeetingId NUMERIC(18,0) = -1,
	@PitchProdId	VARCHAR(10),
	@OldPitchProdId	VARCHAR(10),
	@PitchDuratn	VARCHAR(10),
	@OldPitchDuratn	VARCHAR(10),
	@ClosngAmnt		VARCHAR(10),
	@OldClosngAmnt	VARCHAR(10),
	@UpdatedBy		INT,
	@InitiatedBy	INT,
	@OldClosngPrabability	VARCHAR(10),
	@NewClosngPrabability	VARCHAR(10),
	@LeadStatus		VARCHAR(10),
	@LostReasons	VARCHAR(10),
	@Type			VARCHAR(10),	--Used for new Insert and Update existing entry
	@IsCpLog		VARCHAR(10),	--For update record in DCRM_CpLog
	@IsPrdctLog		VARCHAR(10),	--For update recorf in DCRM_ProductLog
	@ExpClsngDate		DATETIME,
	@OldExpClsngDate	DATETIME,
	@OtherLostReason	VARCHAR(100),
	@NewSalesId		INT = -1 OUTPUT ,
	@NewMeetingId	INT = -1 OUTPUT ,
	@DiscountAmount	VARCHAR(15) = NULL,
	@ProductAmount	VARCHAR(15) = NULL,
	@TAX			VARCHAR(10) = NULL,
	@TotalAmount	VARCHAR(15) = NULL,
	@TDSAmount		VARCHAR(15) = NULL,
	@IsTDSGiven		VARCHAR(5)	= NULL,
	@DiscountDays	VARCHAR(10) = NULL,
	@MeetingModes	VARCHAR(5)	= NULL,
	@IsActionTaken	VARCHAR(5)	= NULL,
	@PANNumber		VARCHAR(10) = NULL,
	@TANNumber		VARCHAR(10) = NULL,
	@RSAPackageQunatity	INT	=	NULL,
	@ProductType	INT = NULL

AS
BEGIN

	DECLARE @TodayDate	DATETIME 
	DECLARE @DealerId	NUMERIC(18,0)
	DECLARE @SalesDealer TABLE(ID INT IDENTITY(1,1) ,SalesID NUMERIC(18,0))
	DECLARE @LoopCount	INT
	DECLARE @WhileLoopCount	INT
	DECLARE @SalesDealerId INT
	SET		@TodayDate = GETDATE()
	
	SELECT @DealerId = DealerId FROM DCRM_SalesDealer WHERE ID = @SalesId ORDER BY EntryDate DESC
	
	--If entry is exist then update the existing record
	IF @@RowCount <> 0 AND @Type = 1
		
		BEGIN		
		
			--Make new entry in Sales Meeting if no meeting is recorded
			IF @SalesMeetingId = -1 OR @SalesMeetingId IS NULL
		
				BEGIN
					INSERT INTO DCRM_SalesMeeting ( DealerId , SalesDealerId, ActionTakenOn,ActionTakenBy,DealerType,MeetingDate,Meetingmode,IsActionTaken) 
						VALUES( @DealerId, @SalesId,GETDATE(),@UpdatedBy,1,GETDATE(),@MeetingModes,@IsActionTaken)--Dealer type 1 for CarWale Dealers
					
					SET @NewMeetingId = SCOPE_IDENTITY()
					SET @SalesMeetingId = @NewMeetingId
					
					--Make new entries in DCRM_ProductMeetingLog for all the open Products exist against the Dealer
					IF @NewMeetingId <> -1 AND @NewMeetingId IS NOT NULL
						
						BEGIN
							INSERT INTO @SalesDealer(SalesID)
										SELECT DSD.ID FROM DCRM_SalesDealer DSD WHERE DSD.LeadStatus = 1 AND DSD.DealerId = @DealerId
							
							SELECT @WhileLoopCount = COUNT(ID) FROM @SalesDealer
							
							SET @LoopCount = 1
							
							WHILE @LoopCount <= @WhileLoopCount 
								BEGIN
									
									SELECT @SalesDealerId = SalesID FROM @SalesDealer 	WHERE ID = @LoopCount  
									
									INSERT INTO DCRM_ProductMeetingLog (SalesMeetingId,SalesDealerID) 
									VALUES (@NewMeetingId,@SalesDealerId) 
									
									SET @LoopCount = @LoopCount + 1
									
								END 
						END
				END
			--Updated entry in DCRM_SalesDealer				
			UPDATE DCRM_SalesDealer SET		UpdatedOn = @TodayDate, UpdatedBy = @UpdatedBy,PitchDuration = @PitchDuratn,
											ClosingProbability = @NewClosngPrabability,PitchingProduct = @PitchProdId, 
											ClosingAmount = @ClosngAmnt,ClosingDate = @ExpClsngDate,LeadStatus = @LeadStatus , 
											LostReason = @LostReasons,InitiatedBy = @InitiatedBy,FieldExecutive=@InitiatedBy,
											OtherResons = @OtherLostReason,DiscountAmount = @DiscountAmount,
											ProductAmount = @ProductAmount , ServiceTax = @TAX , TotalAmount = @TotalAmount , 
											TDSAmount = @TDSAmount,IsTDSGiven = @IsTDSGiven,DiscountDays = @DiscountDays,
											PANNumber = @PANNumber , TANNumber = @TANNumber ,Quantity = @RSAPackageQunatity,
											DealerType = @ProductType
					WHERE Id = @SalesId

			--If user Loss or Converted Dealer then upadte SalesDealer with closed SalesMeetingID
			IF @LeadStatus = 2 OR @LeadStatus = 3
				BEGIN
					UPDATE DCRM_SalesDealer SET ClosedMeetingID = @SalesMeetingId ,UpdatedOn = GETDATE() WHERE Id = @SalesId
				END
			
			IF @IsCpLog = 1 AND @SalesMeetingId IS NOT NULL
				BEGIN
					INSERT INTO DCRM_CPLog ( DealerId,SalesDealerId,UpdatedBy,UpdatedOn,OldValue,NewValue,SalesMeetingId )
								VALUES( @DealerId,@SalesId,@UpdatedBy,@TodayDate,@OldClosngPrabability,@NewClosngPrabability,@SalesMeetingId )
					
				END
				
			--Now log the record in DCRM_ProductLog for other chnages against Dealer in that SalesMeeting
			
			IF @IsPrdctLog = 1 AND @SalesMeetingId IS NOT NULL
				BEGIN
					INSERT INTO DCRM_ProductLog ( DealerId,SalesDealerId,UpdatedBy,UpdatedOn,OldPackage,NewPackage,OldClosingAmount,NewClosingAmount
											,OldClosingDate,NewClosingDate,OldDuration,NewDuration,SalesMeetingId)
								VALUES( @DealerId,@SalesId,@UpdatedBy,@TodayDate,@OldPitchProdId,@PitchProdId,@OldClosngAmnt,@ClosngAmnt,
											@OldExpClsngDate,@ExpClsngDate,@OldPitchDuratn,@PitchDuratn,@SalesMeetingId)
				END
		
			SET @NewSalesId = 1
		
		END
	
	--Enter new package against the Dealer
	ELSE IF @Type = 2
	
		BEGIN
			-- make new entry in Sales Meeting
			--When new package is added then @SalesId contains DealerId
			IF @SalesMeetingId = -1
				BEGIN
					INSERT INTO DCRM_SalesMeeting ( DealerId , SalesDealerId , ActionTakenOn,ActionTakenBy,DealerType,MeetingDate,Meetingmode,IsActionTaken) 
											VALUES( @SalesId, @NewSalesId ,@TodayDate,@UpdatedBy,1,GETDATE(),@MeetingModes,@IsActionTaken)--Dealer type 1 for CarWale Dealers
					
					SET @NewMeetingId = SCOPE_IDENTITY()
					SET @SalesMeetingId = SCOPE_IDENTITY()
					
					INSERT INTO DCRM_SalesDealer (	PitchingProduct,DealerId,PitchDuration,UpdatedBy,UpdatedOn,ClosingAmount,ClosingProbability,
													EntryDate,CreatedOn,DealerType,LeadSource,LeadStatus,ClosingDate,InitiatedBy,
													FieldExecutive,BOExecutive,OtherResons,StartMeetingId,Quantity)
										VALUES	(	@PitchProdId,@SalesId,@PitchDuratn,@UpdatedBy,@TodayDate,@ClosngAmnt,@NewClosngPrabability,
													@TodayDate,@TodayDate,@ProductType,31,@LeadStatus,@ExpClsngDate,@InitiatedBy,@InitiatedBy,
													-1,@OtherLostReason,@SalesMeetingId,@RSAPackageQunatity)
					
					SET @NewSalesId = SCOPE_IDENTITY()
					SET @DealerId = @SalesId

					--If new package added against the Dealer also make entry in DCRM_ProductMeetingLog table
					IF 	@SalesMeetingId <> -1 AND @SalesMeetingId IS NOT NULL
						BEGIN

							INSERT INTO @SalesDealer(SalesID)
							SELECT DSD.ID FROM DCRM_SalesDealer DSD WHERE DSD.LeadStatus = 1 AND DSD.DealerId = @DealerId

							SELECT @WhileLoopCount = COUNT(ID) FROM @SalesDealer

							SET @LoopCount = 1

							WHILE @LoopCount <= @WhileLoopCount 
								BEGIN
									--UPDATE DCRM_SalesDealer SET BOExecutive = 5	 WHERE StartMeetingId = @SalesMeetingId
									
									SELECT @SalesDealerId = SalesID FROM @SalesDealer 	WHERE ID = @LoopCount  
									
									INSERT INTO DCRM_ProductMeetingLog (SalesMeetingId,SalesDealerID) 
									VALUES (@SalesMeetingId,@SalesDealerId) 
									
									SET @LoopCount = @LoopCount + 1
									
								END 
						END
				END
			ELSE
				BEGIN
					INSERT INTO DCRM_SalesDealer(	PitchingProduct,DealerId,PitchDuration,UpdatedBy,UpdatedOn,ClosingAmount,ClosingProbability,
													EntryDate,CreatedOn,DealerType,LeadSource,LeadStatus,ClosingDate,InitiatedBy,
													FieldExecutive,BOExecutive,OtherResons,StartMeetingId,Quantity)
										VALUES	(	@PitchProdId,@SalesId,@PitchDuratn,@UpdatedBy,@TodayDate,@ClosngAmnt,@NewClosngPrabability,
													@TodayDate,@TodayDate,@ProductType,31,@LeadStatus,@ExpClsngDate,@InitiatedBy,@InitiatedBy,
													-1,@OtherLostReason,@SalesMeetingId,@RSAPackageQunatity)
					
					SET @NewSalesId = SCOPE_IDENTITY()

					IF @SalesMeetingId IS NOT NULL 
					BEGIN
						INSERT INTO DCRM_ProductMeetingLog (SalesMeetingId,SalesDealerID) 
								VALUES (@SalesMeetingId,@NewSalesId) 
					END
				END
		END
	PRINT @NewSalesId
END

