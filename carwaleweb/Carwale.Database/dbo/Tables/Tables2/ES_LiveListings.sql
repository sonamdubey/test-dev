CREATE TABLE [dbo].[ES_LiveListings] (
    [ProfileID]      VARCHAR (50) NOT NULL,
    [Action]         CHAR (6)     NULL,
    [ActionType]     TINYINT      NULL,
    [LastUpdateTime] DATETIME     NULL,
    [IsSynced]       BIT          CONSTRAINT [DF_IsSynced] DEFAULT ((0)) NULL,
    [SyncTime]       DATETIME     NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_ES_LiveListings_IsSynced]
    ON [dbo].[ES_LiveListings]([ProfileID] ASC, [Action] ASC, [IsSynced] ASC);

