CREATE TABLE [dbo].[SkodaTCDealerMap] (
    [TCDealerId]    INT           NOT NULL,
    [SkodaDealerId] INT           NOT NULL,
    [DealerName]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_SkodaTCDealerMap] PRIMARY KEY CLUSTERED ([TCDealerId] ASC, [SkodaDealerId] ASC)
);

