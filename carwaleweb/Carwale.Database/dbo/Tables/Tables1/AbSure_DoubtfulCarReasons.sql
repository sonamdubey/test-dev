CREATE TABLE [dbo].[AbSure_DoubtfulCarReasons] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId] INT      NULL,
    [DoubtfulReason]      INT      NULL,
    [EntryDate]           DATETIME NULL,
    [IsActive]            BIT      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

