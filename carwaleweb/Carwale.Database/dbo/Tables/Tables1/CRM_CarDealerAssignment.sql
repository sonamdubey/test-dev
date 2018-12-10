CREATE TABLE [dbo].[CRM_CarDealerAssignment] (
    [Id]                      NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CBDId]                   NUMERIC (18)   NULL,
    [DealerId]                NUMERIC (18)   NULL,
    [ContactPerson]           VARCHAR (50)   NULL,
    [Contact]                 VARCHAR (50)   NULL,
    [Status]                  SMALLINT       NULL,
    [CreatedOn]               DATETIME       NULL,
    [CreatedBy]               NUMERIC (18)   NULL,
    [UpdatedOn]               DATETIME       NULL,
    [UpdatedBy]               NUMERIC (18)   NULL,
    [Comments]                VARCHAR (5000) NULL,
    [LostDate]                DATETIME       NULL,
    [LostName]                VARCHAR (100)  NULL,
    [ReasonLost]              VARCHAR (100)  NULL,
    [NoResponseDate]          DATETIME       NULL,
    [CreateOnDatePart]        DATETIME       CONSTRAINT [DF_CRM_CarDealerAssignment_CreateOnDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NULL,
    [StatusUpdatedBy]         NUMERIC (18)   NULL,
    [StatusUpdatedOn]         DATETIME       NULL,
    [IsNCDFeedback]           BIT            NULL,
    [IsConCall]               BIT            NULL,
    [CampaignId]              BIGINT         NULL,
    [LatestStatus]            SMALLINT       NULL,
    [LatestStatusDate]        DATETIME       NULL,
    [LastConnectedStatus]     SMALLINT       NULL,
    [LastConnectedStatusDate] DATETIME       NULL,
    [SalesExecutiveId]        INT            NULL,
    [DealershipStatus]        BIT            NULL,
    [IsFollowDone]            BIT            NULL,
    [DealershipTagDate]       DATETIME       NULL,
    CONSTRAINT [PK_CRM_CarDealerAssignment] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarDealerAssignment_CBDId]
    ON [dbo].[CRM_CarDealerAssignment]([CBDId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarDealerAssignment_Status]
    ON [dbo].[CRM_CarDealerAssignment]([Status] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarDealerAssignment__DealerId__Status]
    ON [dbo].[CRM_CarDealerAssignment]([DealerId] ASC, [Status] ASC)
    INCLUDE([CBDId]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarDealerAssignment__DealerId]
    ON [dbo].[CRM_CarDealerAssignment]([DealerId] ASC)
    INCLUDE([Id], [CBDId], [Status]);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarDealerAssignment_CreatedOn]
    ON [dbo].[CRM_CarDealerAssignment]([CreatedOn] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarDealerAssignment_LatestStatus]
    ON [dbo].[CRM_CarDealerAssignment]([LatestStatus] ASC);


GO
-- ==========================================================
-- Author: AMIT KUMAR
-- Create date: 31 st OCT 2012
-- Description: Set StatusUpdatedBy AND  StatusUpdatedOn after altering the Status to any value
-- ===========================================================

CREATE TRIGGER [dbo].[TRG_AIU_CRM_CarDealerAssignment] ON dbo.CRM_CarDealerAssignment
FOR INSERT,UPDATE
AS
IF (UPDATE(Status))
	BEGIN
	DECLARE @UpdateID  NUMERIC(18,0)
	DECLARE @UpdatedBy  NUMERIC(18,0)


	DECLARE @StatusI  INT
	DECLARE @StatusD  INT

	SELECT @StatusI = I.Status,@UpdateID  = I.Id, @UpdatedBy=I.UpdatedBy FROM INSERTED I
	SELECT @StatusD = D.Status FROM DELETED D

		IF ((@StatusD != @StatusI) OR (@StatusD IS NULL AND @StatusI IS NOT NULL))
		BEGIN
			 UPDATE CRM_CarDealerAssignment SET StatusUpdatedBy = @UpdatedBy , StatusUpdatedOn = GETDATE() 
			 WHERE Id = @UpdateID 
		END 
	
END

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dealer contact person', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_CarDealerAssignment', @level2type = N'COLUMN', @level2name = N'ContactPerson';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contact of dealer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_CarDealerAssignment', @level2type = N'COLUMN', @level2name = N'Contact';

