CREATE TABLE [dbo].[CRM_CarBookingLog] (
    [Id]                      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CBDId]                   NUMERIC (18)   NOT NULL,
    [IsBookingRequested]      BIT            NULL,
    [IsBookingCompleted]      BIT            NULL,
    [IsBookingNotPossible]    BIT            NULL,
    [BookingRequestDate]      DATETIME       NULL,
    [BookingCompleteDate]     DATETIME       NULL,
    [BookingNPDispositionId]  NUMERIC (18)   NULL,
    [IsPriorBooking]          BIT            NULL,
    [CreatedOn]               DATETIME       CONSTRAINT [DF_CRM_CarBookingLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]               NUMERIC (18)   NOT NULL,
    [UpdatedOn]               DATETIME       NULL,
    [UpdatedBy]               NUMERIC (18)   NULL,
    [BookingExpectedDate]     DATETIME       NULL,
    [BookingCompletedEventBy] NUMERIC (18)   NULL,
    [BookingCompletedEventOn] DATETIME       NULL,
    [Color]                   VARCHAR (100)  NULL,
    [Comments]                VARCHAR (1000) NULL,
    [RegisterPersonName]      VARCHAR (200)  NULL,
    [NIFeedback]              BIT            NULL,
    [NoFeedbackContact]       BIT            NULL,
    CONSTRAINT [PK_CRM_CarBookingLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarBookingLog_IsBookingCompleted]
    ON [dbo].[CRM_CarBookingLog]([IsBookingCompleted] ASC)
    INCLUDE([CBDId]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarBookingLog_IsBookingNotPossible]
    ON [dbo].[CRM_CarBookingLog]([IsBookingNotPossible] ASC)
    INCLUDE([CBDId]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarBookingLog_CBDId]
    ON [dbo].[CRM_CarBookingLog]([CBDId] ASC)
    INCLUDE([IsBookingRequested], [IsBookingCompleted]);


GO

-- ==========================================================
-- Author: AMIT KUMAR
-- Create date: 30 th OCT 2012
-- Description: Set BookingCompletedEventBy AND  BookingCompletedEventOn after altering the IsBookingCompleted or  IsBookingNotPossible or IsPriorBooking to any value
-- ===========================================================

CREATE TRIGGER [dbo].[TRG_AIU_CRM_CarBookingLog] ON [dbo].[CRM_CarBookingLog]
FOR INSERT,UPDATE
AS
IF(UPDATE(IsBookingCompleted) OR UPDATE(IsBookingNotPossible) OR UPDATE(IsPriorBooking))
BEGIN
	DECLARE @UpdateID  NUMERIC(18,0)
	DECLARE @UpdatedBy  NUMERIC(18,0)


	DECLARE @IsBookingCompletedI  INT
	DECLARE @IsBookingNotPossibleI  INT
	DECLARE @IsPriorBookingI	INT

	DECLARE @IsBookingCompletedD  INT
	DECLARE @IsBookingNotPossibleD  INT
	DECLARE @IsPriorBookingD	INT

	SELECT @IsBookingCompletedI = I.IsBookingCompleted, @IsBookingNotPossibleI = I.IsBookingNotPossible,@IsPriorBookingI=I.IsPriorBooking, @UpdateID  = I.Id, @UpdatedBy= I.UpdatedBy  FROM INSERTED I

	SELECT @IsBookingCompletedD = D.IsBookingCompleted ,@IsBookingNotPossibleD = D.IsBookingNotPossible, @IsPriorBookingD=D.IsPriorBooking FROM DELETED D

		IF ((@IsBookingCompletedD != @IsBookingCompletedI) OR (@IsBookingNotPossibleD != @IsBookingNotPossibleI) OR (@IsPriorBookingD != @IsPriorBookingI) OR (@IsBookingCompletedD IS NULL AND  @IsBookingCompletedI IS NOT NULL ) OR (@IsBookingNotPossibleD IS NULL AND @IsBookingNotPossibleI IS NOT NULL) OR (@IsPriorBookingD IS NULL AND @IsPriorBookingI IS NOT NULL))
		BEGIN
			 UPDATE CRM_CarBookingLog SET BookingCompletedEventBy = @UpdatedBy , BookingCompletedEventOn = GETDATE() 
			 WHERE Id = @UpdateID 
		END 
END
