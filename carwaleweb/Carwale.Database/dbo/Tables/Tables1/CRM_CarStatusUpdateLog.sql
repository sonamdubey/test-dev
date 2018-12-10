CREATE TABLE [dbo].[CRM_CarStatusUpdateLog] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [CBDId]            NUMERIC (18) NULL,
    [SubDispositionId] NUMERIC (18) NULL,
    [UpdatedBy]        NUMERIC (18) NULL,
    [UpdatedOn]        DATETIME     CONSTRAINT [DF_CRM_CarStatusUpdateLog_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_CarStatusUpdateLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarStatusUpdateLog]
    ON [dbo].[CRM_CarStatusUpdateLog]([CBDId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarStatusUpdateLog_SubDispositionId]
    ON [dbo].[CRM_CarStatusUpdateLog]([SubDispositionId] ASC);

