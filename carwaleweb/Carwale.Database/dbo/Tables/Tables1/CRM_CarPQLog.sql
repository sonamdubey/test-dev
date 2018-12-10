CREATE TABLE [dbo].[CRM_CarPQLog] (
    [Id]                 NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CBDId]              NUMERIC (18) NOT NULL,
    [IsPQRequested]      BIT          CONSTRAINT [DF_CRM_CarPQLog_IsPQRequested] DEFAULT ((1)) NOT NULL,
    [IsPQCompleted]      BIT          NULL,
    [IsPQNotRequired]    BIT          NULL,
    [PQRequestDate]      DATETIME     NOT NULL,
    [PQCompleteDate]     DATETIME     NULL,
    [PQNRDispositionId]  NUMERIC (18) NULL,
    [CreatedOn]          DATETIME     CONSTRAINT [DF_CRM_CarPQLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          NUMERIC (18) NOT NULL,
    [UpdatedOn]          DATETIME     NULL,
    [UpdatedBy]          NUMERIC (18) NULL,
    [PQCompletedEventBy] NUMERIC (18) NULL,
    [PQCompletedEventOn] DATETIME     NULL,
    CONSTRAINT [PK_CRM_CarPQLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarPQLog__CBDId__IsPQRequested__IsPQCompleted__IsPQNotRequired]
    ON [dbo].[CRM_CarPQLog]([CBDId] ASC, [IsPQRequested] ASC, [IsPQCompleted] ASC, [IsPQNotRequired] ASC)
    INCLUDE([PQRequestDate]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarPQLog__IsPQRequested__IsPQCompleted__IsPQNotRequired]
    ON [dbo].[CRM_CarPQLog]([IsPQRequested] ASC, [IsPQCompleted] ASC, [IsPQNotRequired] ASC)
    INCLUDE([CBDId], [PQRequestDate]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarPQLog__IsPQCompleted__IsPQNotRequired]
    ON [dbo].[CRM_CarPQLog]([IsPQCompleted] ASC, [IsPQNotRequired] ASC)
    INCLUDE([CBDId]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarPQLog__CBDId]
    ON [dbo].[CRM_CarPQLog]([CBDId] ASC)
    INCLUDE([IsPQRequested], [IsPQCompleted], [IsPQNotRequired], [PQRequestDate], [PQCompleteDate]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarPQLog__IsPQCompleted]
    ON [dbo].[CRM_CarPQLog]([IsPQCompleted] ASC)
    INCLUDE([CBDId], [PQRequestDate], [PQCompleteDate]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarPQLog__CBDId__IsPQCompleted]
    ON [dbo].[CRM_CarPQLog]([CBDId] ASC, [IsPQCompleted] ASC)
    INCLUDE([PQRequestDate], [PQCompleteDate]);


GO

-- ==========================================================
-- Author: AMIT KUMAR
-- Create date: 29 th OCT 2012
-- Description: Set PQCompletedEventBy AND  PQCompletedEventOn after altering the IsPQCompleted or  IsPQNotRequired to 1
-- ===========================================================

CREATE TRIGGER [dbo].[TRG_AIU_CRM_CarPQLog] ON [dbo].[CRM_CarPQLog]
FOR INSERT,UPDATE
AS
IF(UPDATE(IsPQCompleted) OR UPDATE(IsPQNotRequired))
BEGIN
	DECLARE @UpdateID  NUMERIC(18,0)
	DECLARE @UpdatedBy  NUMERIC(18,0)


	DECLARE @IsPQCompletedI  INT
	DECLARE @IsPQNotRequiredI  INT

	DECLARE @IsPQCompletedD  INT
	DECLARE @IsPQNotRequiredD  INT

	SELECT @IsPQCompletedI = I.IsPQCompleted ,@IsPQNotRequiredI = I.IsPQNotRequired, @UpdateID  = I.Id,@UpdatedBy=I.UpdatedBy FROM INSERTED I

	SELECT @IsPQCompletedD = D.IsPQCompleted,@IsPQNotRequiredD = D.IsPQNotRequired FROM DELETED D

		IF ((@IsPQCompletedD != @IsPQCompletedI) OR (@IsPQNotRequiredD != @IsPQNotRequiredI) OR (@IsPQCompletedD IS NULL AND @IsPQCompletedI IS NOT NULL) OR (@IsPQNotRequiredD IS NULL AND @IsPQNotRequiredI IS NOT NULL))
		BEGIN
			 UPDATE CRM_CarPQLog SET PQCompletedEventBy = @UpdatedBy , PQCompletedEventOn = GETDATE() 
			 WHERE Id = @UpdateID 
		END 
END

GO

CREATE TRIGGER [dbo].[TRG_CRM_CarPQLog]
ON [dbo].[CRM_CarPQLog]
AFTER INSERT , UPDATE 
AS
IF ( UPDATE (IsPQRequested) OR UPDATE (IsPQNotRequired) )
	BEGIN
		DECLARE @LeadId BIGINT
		DECLARE @uIsPQRequested BIT,@dIsPQRequested BIT,
				@uIsPQNotRequired BIT,@dIsPQNotRequired BIT

		SELECT @uIsPQRequested=ISNULL(I.IsPQRequested,0) , @dIsPQRequested=ISNULL(D.IsPQRequested,0)
		FROM INSERTED AS I LEFT JOIN DELETED AS D ON I.ID=D.ID
  
		SELECT @uIsPQNotRequired=ISNULL(I.IsPQNotRequired,0) , @dIsPQNotRequired=ISNULL(D.IsPQNotRequired,0)
		FROM INSERTED AS I LEFT JOIN DELETED AS D ON I.ID=D.ID

		IF ((@uIsPQRequested <>@dIsPQRequested) OR (@uIsPQNotRequired <>@dIsPQNotRequired))
			BEGIN
				SELECT top 1 @LeadId =  LeadId 
				FROM CRM_CarBasicData AS CBD JOIN inserted AS I ON CBD.ID=i.CBDId
  
				EXEC CRM.LSUpdateLeadScore 4, @LeadId, -1
			END
	END;




GO
DISABLE TRIGGER [dbo].[TRG_CRM_CarPQLog]
    ON [dbo].[CRM_CarPQLog];

