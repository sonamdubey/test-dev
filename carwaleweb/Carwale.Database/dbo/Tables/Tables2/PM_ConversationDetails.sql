CREATE TABLE [dbo].[PM_ConversationDetails] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ConversationId] NUMERIC (18) NOT NULL,
    [UserId]         NUMERIC (18) NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_PM_ConversationDetails_IsActive] DEFAULT ((1)) NOT NULL,
    [IsSentActive]   BIT          CONSTRAINT [DF_PM_ConversationDetails_IsSentActive] DEFAULT ((1)) NOT NULL,
    [IsRead]         BIT          CONSTRAINT [DF_PM_ConversationDetails_IsRead] DEFAULT ((0)) NOT NULL,
    [UpdatedDate]    DATETIME     CONSTRAINT [DF_PM_ConversationDetails_UpdatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_PM_Drafts] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

