CREATE TABLE [dbo].[TC_FeedBackCalling_PushedData] (
    [Id]               INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]         INT      NOT NULL,
    [ToDate]           DATETIME NOT NULL,
    [FromDate]         DATETIME NOT NULL,
    [FeedbackDealerId] INT      NULL,
    [CreatedBy]        INT      NULL,
    [CreatedOn]        DATETIME NULL,
    [IsProcessed]      BIT      DEFAULT ((0)) NULL,
    [ModifiedDate]     DATETIME NULL
);

