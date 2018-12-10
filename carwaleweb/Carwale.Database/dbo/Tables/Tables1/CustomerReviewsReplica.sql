CREATE TABLE [dbo].[CustomerReviewsReplica] (
    [ID]               NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ReviewId]         NUMERIC (18)   NULL,
    [StyleR]           SMALLINT       NOT NULL,
    [ComfortR]         SMALLINT       NOT NULL,
    [PerformanceR]     SMALLINT       NOT NULL,
    [ValueR]           SMALLINT       NOT NULL,
    [FuelEconomyR]     SMALLINT       NOT NULL,
    [OverallR]         FLOAT (53)     NOT NULL,
    [Pros]             VARCHAR (100)  NULL,
    [Cons]             VARCHAR (100)  NULL,
    [Comments]         VARCHAR (8000) NULL,
    [Title]            VARCHAR (100)  NULL,
    [IsVerified]       BIT            NOT NULL,
    [LastUpdatedOn]    DATETIME       NULL,
    [LastUpdatedBy]    NUMERIC (18)   NULL,
    [IsOwned]          BIT            NULL,
    [IsNewlyPurchased] BIT            NULL,
    [Familiarity]      INT            NULL,
    [Mileage]          FLOAT (53)     NULL,
    CONSTRAINT [PK_CustomerReviewsReplica] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

