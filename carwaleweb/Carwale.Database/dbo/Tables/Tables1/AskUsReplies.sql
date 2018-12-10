CREATE TABLE [dbo].[AskUsReplies] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QuestionId]   NUMERIC (18)   NOT NULL,
    [Post]         VARCHAR (2000) NOT NULL,
    [PostDateTime] DATETIME       NOT NULL,
    [PostedBy]     NUMERIC (18)   NOT NULL,
    [IsQuestioner] BIT            CONSTRAINT [DF_AskUsReplies_IsQuestioner] DEFAULT (1) NOT NULL,
    [IsThanks]     BIT            CONSTRAINT [DF__AskUsRepl__IsTha__61FB72FB] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_AskUsReplies] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

