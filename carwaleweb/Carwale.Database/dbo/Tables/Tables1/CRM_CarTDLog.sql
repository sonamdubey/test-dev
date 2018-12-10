CREATE TABLE [dbo].[CRM_CarTDLog] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CBDId]              NUMERIC (18)  NOT NULL,
    [IsTDRequested]      BIT           NOT NULL,
    [IsTDCompleted]      BIT           NULL,
    [ISTDNotPossible]    BIT           NULL,
    [IsTDDirect]         BIT           NULL,
    [TDRequestDate]      DATETIME      NOT NULL,
    [TDCompleteDate]     DATETIME      NULL,
    [TDNPDispositionId]  NUMERIC (18)  NULL,
    [CreatedOn]          DATETIME      CONSTRAINT [DF_CRM_CarTDLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          NUMERIC (18)  NOT NULL,
    [UpdatedOn]          DATETIME      NULL,
    [UpdatedBy]          NUMERIC (18)  NULL,
    [TDComment]          VARCHAR (500) NULL,
    [TDCompletedEventBy] NUMERIC (18)  NULL,
    [TDCompletedEventOn] DATETIME      NULL,
    CONSTRAINT [PK_CRM_CarTDLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarTDLog__CBDId__IsTDCompleted__ISTDNotPossible__TDRequestDate]
    ON [dbo].[CRM_CarTDLog]([CBDId] ASC, [IsTDCompleted] ASC, [ISTDNotPossible] ASC, [TDRequestDate] ASC);


GO

CREATE TRIGGER [dbo].[TRG_CRM_CarTDLog]
ON [dbo].[CRM_CarTDLog]
AFTER INSERT , UPDATE 
AS
IF ( UPDATE (IsTDCompleted) OR UPDATE (ISTDNotPossible) )
	BEGIN
		DECLARE @LeadId BIGINT
		DECLARE @uIsTDCompleted BIT,@dIsTDCompleted BIT,
				@uISTDNotPossible BIT,@dISTDNotPossible BIT

		SELECT @uIsTDCompleted=ISNULL(I.IsTDCompleted,0) , @dIsTDCompleted=ISNULL(D.IsTDCompleted,0)
		FROM INSERTED AS I LEFT JOIN DELETED AS D ON I.ID=D.ID
  
		SELECT @uISTDNotPossible=ISNULL(i.ISTDNotPossible,0) , @dISTDNotPossible=ISNULL(d.ISTDNotPossible,0)
		FROM INSERTED AS I LEFT JOIN DELETED AS D ON I.ID=D.ID

		IF ((@uIsTDCompleted <>@dIsTDCompleted) or (@uISTDNotPossible <>@dISTDNotPossible))
			BEGIN
				SELECT top 1 @LeadId =  LeadId 
				FROM CRM_CarBasicData as CBD JOIN INSERTED AS I ON CBD.ID=I.CBDID
  
				EXEC CRM.LSUpdateLeadScore 5, @LeadId, -1
			END
	END;




GO
DISABLE TRIGGER [dbo].[TRG_CRM_CarTDLog]
    ON [dbo].[CRM_CarTDLog];


GO

-- ==========================================================
-- Author: AMIT KUMAR
-- Create date: 30 th OCT 2012
-- Description: Set TDCompletedEventBy AND  TDCompletedEventOn after altering the IsTDCompleted or  ISTDNotPossible or IsTDDirect to any value
-- ===========================================================

CREATE TRIGGER [dbo].[TRG_AIU_CRM_CarTDLog] ON [dbo].[CRM_CarTDLog]
FOR INSERT,UPDATE
AS
IF (UPDATE(IsTDCompleted) OR UPDATE(ISTDNotPossible) OR UPDATE(IsTDDirect))
BEGIN
	DECLARE @UpdateID  NUMERIC(18,0)
	DECLARE @UpdatedBy  NUMERIC(18,0)


	DECLARE @IsTDCompletedI  INT
	DECLARE @ISTDNotPossibleI  INT
	DECLARE @IsTDDirectI	INT

	DECLARE @IsTDCompletedD  INT
	DECLARE @ISTDNotPossibleD  INT
	DECLARE @IsTDDirectD	INT

	SELECT @IsTDCompletedI = I.IsTDCompleted, @ISTDNotPossibleI = I.ISTDNotPossible,@IsTDDirectI=I.IsTDDirect, @UpdateID  = I.Id, @UpdatedBy= I.UpdatedBy  FROM INSERTED I

	SELECT @IsTDCompletedD = D.IsTDCompleted ,@ISTDNotPossibleD = D.ISTDNotPossible, @IsTDDirectD=D.IsTDDirect FROM DELETED D

		IF ((@IsTDCompletedD != @IsTDCompletedI) OR (@ISTDNotPossibleD != @ISTDNotPossibleI) OR (@IsTDDirectD != @IsTDDirectI) OR (@IsTDCompletedD IS NULL AND  @IsTDCompletedI IS NOT NULL ) OR (@ISTDNotPossibleD IS NULL AND @ISTDNotPossibleI IS NOT NULL) OR (@IsTDDirectD IS NULL AND @IsTDDirectI IS NOT NULL))
		BEGIN
			 UPDATE CRM_CarTDLog SET TDCompletedEventBy = @UpdatedBy , TDCompletedEventOn = GETDATE() 
			 WHERE Id = @UpdateID 
		END 
END
