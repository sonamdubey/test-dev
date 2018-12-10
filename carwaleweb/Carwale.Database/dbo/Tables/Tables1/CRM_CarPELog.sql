CREATE TABLE [dbo].[CRM_CarPELog] (
    [Id]                 NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CBDId]              NUMERIC (18) NOT NULL,
    [IsPERequested]      BIT          CONSTRAINT [DF_CRM_CarPELog_IsPERequested] DEFAULT ((1)) NOT NULL,
    [IsPECompleted]      BIT          NULL,
    [IsPENotRequired]    BIT          NULL,
    [PERequestDate]      DATETIME     NOT NULL,
    [PECompleteDate]     DATETIME     NULL,
    [PENRDispositionId]  NUMERIC (18) NULL,
    [CreatedOn]          DATETIME     CONSTRAINT [DF_CRM_CarPELog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]          NUMERIC (18) NOT NULL,
    [UpdatedOn]          DATETIME     NULL,
    [UpdatedBy]          NUMERIC (18) NULL,
    [PECompletedEventBy] NUMERIC (18) NULL,
    [PECompletedEventOn] DATETIME     NULL,
    CONSTRAINT [PK_CRM_CarPELog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

-- ==========================================================
-- Author: AMIT KUMAR
-- Create date: 30 th OCT 2012
-- Description: Set PECompletedEventBy AND  PECompletedEventOn after altering the IsPECompleted or  IsPENotRequired to 1
-- ===========================================================

CREATE TRIGGER [dbo].[TRG_AIU_CRM_CarPELog] ON [dbo].[CRM_CarPELog]
FOR INSERT,UPDATE
AS
IF(UPDATE(IsPECompleted) OR UPDATE(IsPENotRequired))
BEGIN
	DECLARE @UpdateID  NUMERIC(18,0)
	DECLARE @UpdatedBy  NUMERIC(18,0)


	DECLARE @IsPECompletedI  INT
	DECLARE @IsPENotRequiredI  INT

	DECLARE @IsPECompletedD  INT
	DECLARE @IsPENotRequiredD  INT

	SELECT @IsPECompletedI = I.IsPECompleted,@IsPENotRequiredI = I.IsPENotRequired,@UpdateID  = I.Id, @UpdatedBy=I.UpdatedBy FROM INSERTED I
	SELECT @IsPECompletedD = D.IsPECompleted,@IsPENotRequiredD = D.IsPENotRequired FROM DELETED D


		IF ((@IsPECompletedD != @IsPECompletedI) OR (@IsPENotRequiredD != @IsPENotRequiredI) OR (@IsPECompletedD IS NULL AND @IsPECompletedI IS NOT NULL) OR (@IsPENotRequiredD IS NULL AND @IsPENotRequiredI IS NOT NULL))
		BEGIN
			 UPDATE CRM_CarPELog SET PECompletedEventBy = @UpdatedBy , PECompletedEventOn = GETDATE() 
			 WHERE Id = @UpdateID 
		END 
	
END

GO


CREATE TRIGGER [dbo].[TRG_CRM_CarPELog]
ON [dbo].[CRM_CarPELog]
	AFTER INSERT , UPDATE 
	AS
	IF (UPDATE (IsPECompleted) OR UPDATE (IsPENotRequired) )
		BEGIN
			PRINT 'UPDATE TRIGGER'
			DECLARE @LeadId BIGINT
			DECLARE @uIsPECompleted BIT, @dIsPECompleted BIT,
					@uIsPENotRequired BIT,@dIsPENotRequired BIT

			SELECT @uIsPECompleted=ISNULL(I.IsPECompleted,0), @dIsPECompleted = ISNULL(d.IsPECompleted, 0)
			FROM INSERTED AS I LEFT JOIN DELETED AS D ON I.Id = D.Id
			  
			PRINT @uIsPECompleted
			PRINT @dIsPECompleted
			  
			SELECT @uIsPENotRequired=ISNULL(I.IsPENotRequired,0) , @dIsPENotRequired=ISNULL(D.IsPENotRequired,0)
			FROM INSERTED AS I LEFT JOIN DELETED AS D ON I.Id=D.Id
			
			PRINT @uIsPENotRequired
			PRINT @dIsPENotRequired
			  
			IF ((@uIsPECompleted <> @dIsPECompleted) OR (@uIsPENotRequired <> @dIsPENotRequired))
				BEGIN
					PRINT 'UPDATE TRIGGER2'
					SELECT top 1 @LeadId =  LeadId 
					FROM CRM_CarBasicData as CBD
					JOIN inserted as i on CBD.ID=i.CBDId
					
					PRINT @LeadId
					
					EXEC CRM.LSUpdateLeadScore 6, @LeadId, -1
			END
	END;





GO
DISABLE TRIGGER [dbo].[TRG_CRM_CarPELog]
    ON [dbo].[CRM_CarPELog];

