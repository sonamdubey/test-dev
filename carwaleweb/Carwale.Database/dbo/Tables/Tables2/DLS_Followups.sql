CREATE TABLE [dbo].[DLS_Followups] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CWCustomerId]     NUMERIC (18)  NOT NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_DLS_Followups_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]        DATETIME      NULL,
    [UpdatedBy]        NUMERIC (18)  NOT NULL,
    [LastComment]      VARCHAR (150) NULL,
    [Comments]         VARCHAR (500) NULL,
    [NextCallDate]     DATETIME      NULL,
    [ActionId]         INT           NULL,
    [FollowupLeadType] SMALLINT      CONSTRAINT [DF_DLS_Followups_FollowupLeadType] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DLS_Followups] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_ActId_CWCuId]
    ON [dbo].[DLS_Followups]([ActionId] ASC)
    INCLUDE([CWCustomerId]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Verified & Assigned, 2-Fake,3-Not Intetested, 4-InPipeline', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DLS_Followups', @level2type = N'COLUMN', @level2name = N'ActionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Fiat Leads Followup, 2-OtherLeads Followup', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DLS_Followups', @level2type = N'COLUMN', @level2name = N'FollowupLeadType';

