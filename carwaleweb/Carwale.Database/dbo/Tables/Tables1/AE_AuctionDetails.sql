CREATE TABLE [dbo].[AE_AuctionDetails] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Title]               VARCHAR (50)   NOT NULL,
    [StartDate]           DATETIME       NULL,
    [EndDate]             DATETIME       NULL,
    [InspectionStartDate] DATETIME       NULL,
    [InspectionEndDate]   DATETIME       NULL,
    [IsActive]            BIT            CONSTRAINT [DF_AE_AuctionDetails_IsActive] DEFAULT ((1)) NOT NULL,
    [YardId]              NUMERIC (18)   NULL,
    [IsCompleted]         BIT            CONSTRAINT [DF_AE_AuctionDetails_IsCompleted] DEFAULT ((0)) NULL,
    [CreatedOn]           DATETIME       CONSTRAINT [DF_AE_AuctionDetails_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]           DATETIME       NULL,
    [UpdatedBy]           NUMERIC (18)   NULL,
    [TopBidder]           NUMERIC (9, 2) NULL,
    [TotalBids]           NUMERIC (9, 2) NULL,
    [HighestBid]          NUMERIC (9, 2) NULL,
    CONSTRAINT [PK_AE_AuctionDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

