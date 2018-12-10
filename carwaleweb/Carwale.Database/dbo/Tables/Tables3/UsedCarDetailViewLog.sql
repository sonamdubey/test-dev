CREATE TABLE [dbo].[UsedCarDetailViewLog] (
    [IMEICode]              VARCHAR (50) NOT NULL,
    [ProfileId]             VARCHAR (50) NOT NULL,
    [UsedCarNotificationId] TINYINT      DEFAULT ((0)) NOT NULL,
    [SourceId]              SMALLINT     NOT NULL,
    [EntryTime]             DATETIME     NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UsedCarDetailViewLog_SourceId]
    ON [dbo].[UsedCarDetailViewLog]([SourceId] ASC);

