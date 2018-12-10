CREATE TABLE [dbo].[PM_Messages] (
    [Id]             NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConversationId] NUMERIC (18)   NOT NULL,
    [Message]        VARCHAR (2000) NOT NULL,
    [PostedDate]     DATETIME       CONSTRAINT [DF_PM_Messages_PostedDate] DEFAULT (getdate()) NOT NULL,
    [IsDraft]        BIT            CONSTRAINT [DF_PM_Messages_IsDraft] DEFAULT ((0)) NOT NULL,
    [CreatedBy]      NUMERIC (18)   NOT NULL,
    CONSTRAINT [PK_PM_Messages] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

