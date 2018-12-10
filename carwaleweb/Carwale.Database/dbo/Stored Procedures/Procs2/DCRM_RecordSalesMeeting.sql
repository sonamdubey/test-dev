IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_RecordSalesMeeting]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_RecordSalesMeeting]
GO

	

--Author	:	Sachin Bharti(16th April 2013)
--Purpose	:	Make entry in DCRM_SalesMeeting for FieldExecutives meeting with Dealership
--Modified ON	:	Sachin Bharti(20th June 2013)
--Modified ON	:	Sachin Bharti(26th June 2013) Increase length of @MeetingPurpose
--Modified ON	:	Vinay Prajapati(13th Sept. 2013) added @DecisionMakerName
--Modified ON	:	Sachin Bharti(22th Dec. 2013)
--Modified ON	:	Sachin Bharti(24th Dec. 2013)
--Modified By   :   Komal Manjare(1-December-2015) Decision maker details added 
--Modified By   :   Kritika Choudhary on 18th Dec 2015, added one more parameter SourceId 
CREATE PROCEDURE [dbo].[DCRM_RecordSalesMeeting]

	@DealerID			NUMERIC(18,0),
	@DealerType			INT				=	NULL,--Used for distinguish between OEM Dealers and Carwale Dealers
	@SalesMeetingId		NUMERIC(18,0)	=	-1,
	@SalesDealerId		INT		=	NULL,
	@MeetingType		SMALLINT		=	NULL,
	@MeetDecisionMaker	BIT				=	NULL,
	@DecisionMakerName  VARCHAR(50)		=	NULL,	
	@Comments			VARCHAR(1000)	=	NULL,
	@UpdatedBy			INT,
	@MeetingPurpose		VARCHAR(50)		=	NULL,
	@OtherPurpose		VARCHAR(100)	=	NULL,
	@MeetingDate		DATETIME		=	NULL,
	@IsFromMobile		BIT = 0,
	@MeetingMode		SMALLINT		=	NULL,
	@CarsSoldFrom		DATETIME	=	NULL,
	@CarsSoldTo			DATETIME	=	NULL,
	@CarsSoldCarwale	INT	=	NULL,
	@CarsSoldOverall	INT	=	NULL,
	@NewMeetingId		INT =	NULL OUTPUT ,
	@DecisionMakerDesignation  VARCHAR(50)    =   NULL,
	@DecisionMakerPhoneNo      VARCHAR(20)    =NULL,
	@DecisionMakerEmail        VARCHAR(50)    =NULL,
	@SourceId           INT=1
AS
BEGIN

	DECLARE	@Separator_Position INT
	DECLARE	@MtngPrpsID	INT
	SET @NewMeetingId = -1

	--Make new entry for dealer meeting
	IF @SalesMeetingId = -1
		BEGIN
			INSERT INTO DCRM_SalesMeeting	
										(	DealerId,SalesDealerId,ActionComments,ActionTakenBy,IsActionTaken,ActionTakenOn,
											MeetingType,MeetDecisionMaker,DecisionMakerName,DecisionMakerDesignation,
											DecisionMakerPhoneNo,DecisionMakerEmail,MeetingDate,DealerType,IsFromMobile , 
											MeetingMode,CarsSoldOverall,CarsSoldThroughCarwale,FromCarsSoldBetween,ToCarsSoldBetween,SourceId)
								VALUES	(	@DealerID,@SalesDealerId,@Comments,@UpdatedBy,1,GETDATE(),
											@MeetingType,@MeetDecisionMaker,@DecisionMakerName,@DecisionMakerDesignation,
											@DecisionMakerPhoneNo,@DecisionMakerEmail,@MeetingDate,@DealerType,@IsFromMobile,
											@MeetingMode,@CarsSoldOverall,@CarsSoldCarwale,@CarsSoldFrom,@CarsSoldTo,@SourceId)
			SET @SalesMeetingId = SCOPE_IDENTITY()
			SET @NewMeetingId = @SalesMeetingId
		END
	--If user has recorded a meeting with Dealership then update it
	ELSE 
		BEGIN
			--First update the recorded meeting in DCRM_SalesMeeting
			UPDATE DCRM_SalesMeeting 
								SET		DealerId = @DealerID , SalesDealerId = @SalesDealerId , ActionComments = @Comments,
										ActionTakenBy = @UpdatedBy , IsActionTaken = 1 , ActionTakenOn = GETDATE(),
										MeetingType = @MeetingType ,MeetDecisionMaker = @MeetDecisionMaker,DecisionMakerName = @DecisionMakerName,
										DecisionMakerDesignation=@DecisionMakerDesignation,DecisionMakerPhoneNo=@DecisionMakerPhoneNo,DecisionMakerEmail=@DecisionMakerEmail,
										DealerType = @DealerType,MeetingDate = @MeetingDate,CarsSoldOverall = @CarsSoldOverall ,
										CarsSoldThroughCarwale = @CarsSoldCarwale , FromCarsSoldBetween = @CarsSoldFrom,ToCarsSoldBetween = @CarsSoldTo
								WHERE 
										ID = @SalesMeetingId
			SET @NewMeetingId = @SalesMeetingId
		END		


	--INSERT INTO DCRM_MeetinReason(MeetingId,ReasonId) VALUES (@Result ,)
	IF @MeetingPurpose <> '-1' AND @NewMeetingId <> -1 AND @NewMeetingId IS NOT NULL
		BEGIN
			WHILE @MeetingPurpose <> '' 
				BEGIN
					SET @Separator_Position = CHARINDEX(',', @MeetingPurpose) 
					SET @MtngPrpsID = LEFT(@MeetingPurpose, @Separator_Position-1)  
					SET @MeetingPurpose = RIGHT(@MeetingPurpose, LEN(@MeetingPurpose)- @Separator_Position)
					
					IF @Separator_Position > 0
						BEGIN
							INSERT INTO DCRM_MeetingReason(MeetingId,ReasonId,OtherReason) VALUES (@SalesMeetingId,@MtngPrpsID,@OtherPurpose)
						END
					
				END
		END

END


