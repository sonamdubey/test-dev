CREATE TABLE [dbo].[TC_DealerWebsiteLog] (
    [BranchId]  NUMERIC (18) NULL,
    [StartDate] DATETIME     NULL,
    [EndDate]   DATETIME     NULL,
    [OprUserId] NUMERIC (18) NULL,
    [IsActive]  BIT          DEFAULT ((1)) NULL,
    CONSTRAINT [DF_TC_DealerWebsiteLog_DEALERS] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Dealers] ([ID])
);

