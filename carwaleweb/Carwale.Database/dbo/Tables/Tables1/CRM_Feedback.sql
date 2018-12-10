CREATE TABLE [dbo].[CRM_Feedback] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CBDId]      NUMERIC (18)  NULL,
    [AnswerId]   NUMERIC (18)  NULL,
    [FBDate]     DATETIME      NULL,
    [UpdatedOn]  DATETIME      NULL,
    [UpdatedBy]  NUMERIC (18)  NULL,
    [Comments]   VARCHAR (500) NULL,
    [ItemType]   INT           CONSTRAINT [DF_CRM_Feedback_ItemType] DEFAULT ((2)) NULL,
    [QuestionId] NUMERIC (18)  NULL,
    CONSTRAINT [PK_CRM_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_CRM_Feedback_CBDId_AnswerId]
    ON [dbo].[CRM_Feedback]([CBDId] ASC)
    INCLUDE([AnswerId]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-LeadId, 2-CBDId', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_Feedback', @level2type = N'COLUMN', @level2name = N'ItemType';

