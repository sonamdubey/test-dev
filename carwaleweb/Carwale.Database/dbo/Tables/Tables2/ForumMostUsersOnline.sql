CREATE TABLE [dbo].[ForumMostUsersOnline] (
    [NoOfUsers]        NUMERIC (18) NOT NULL,
    [ModifiedDateTime] DATETIME     NOT NULL,
    [IsFinal]          BIT          CONSTRAINT [DF_ForumMostUsersOnline_IsFinal] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ForumMostUsersOnline] PRIMARY KEY CLUSTERED ([NoOfUsers] ASC, [ModifiedDateTime] ASC, [IsFinal] ASC) WITH (FILLFACTOR = 90)
);

