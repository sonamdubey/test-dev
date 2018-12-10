CREATE TABLE [dbo].[ForumSubscriptions] (
    [FSC_Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]          NUMERIC (18) NOT NULL,
    [ForumThreadId]       NUMERIC (18) NOT NULL,
    [EmailSubscriptionId] INT          NOT NULL,
    CONSTRAINT [PK_ForumSubscriptions] PRIMARY KEY CLUSTERED ([FSC_Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_ForumSubscriptions__ForumThreadId__EmailSubscriptionId]
    ON [dbo].[ForumSubscriptions]([ForumThreadId] ASC, [EmailSubscriptionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ForumSubscriptions_CustomerId]
    ON [dbo].[ForumSubscriptions]([CustomerId] ASC);

