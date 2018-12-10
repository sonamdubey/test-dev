CREATE TABLE [dbo].[Forum_BannedList] (
    [CustomerId] NUMERIC (18) NOT NULL,
    [BannedBy]   NUMERIC (18) NOT NULL,
    [BannedDate] DATETIME     NOT NULL,
    CONSTRAINT [PK_ForumBannedList] PRIMARY KEY CLUSTERED ([CustomerId] ASC) WITH (FILLFACTOR = 90)
);

