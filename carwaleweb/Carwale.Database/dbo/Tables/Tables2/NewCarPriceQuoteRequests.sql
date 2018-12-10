CREATE TABLE [dbo].[NewCarPriceQuoteRequests] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]      NUMERIC (18) NOT NULL,
    [CarVersionId]    NUMERIC (18) NOT NULL,
    [DealerId]        NUMERIC (18) NULL,
    [RequestDateTime] DATETIME     NOT NULL,
    [IsApproved]      BIT          CONSTRAINT [DF_NewCarPriceQuoteRequests_IsApproved] DEFAULT (0) NOT NULL,
    [IsFake]          BIT          CONSTRAINT [DF_NewCarPriceQuoteRequests_IsFake] DEFAULT (0) NOT NULL,
    [StatusId]        SMALLINT     CONSTRAINT [DF_NewCarPriceQuoteRequests_StatusId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_NewCarPriceRequests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

