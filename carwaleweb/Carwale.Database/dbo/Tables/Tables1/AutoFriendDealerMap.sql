CREATE TABLE [dbo].[AutoFriendDealerMap] (
    [AutoFriendDealer] VARCHAR (200) NOT NULL,
    [CarwaleDealerId]  NUMERIC (18)  NOT NULL,
    [IsActive]         BIT           NULL,
    CONSTRAINT [PK_AutoFriendDealerMap] PRIMARY KEY CLUSTERED ([AutoFriendDealer] ASC, [CarwaleDealerId] ASC) WITH (FILLFACTOR = 90)
);

