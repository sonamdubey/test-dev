CREATE TABLE [dbo].[DealerOffersVersionLog] (
    [OfferId]   NUMERIC (18) NOT NULL,
    [MakeId]    NUMERIC (18) NULL,
    [ModelId]   NUMERIC (18) NULL,
    [VersionId] NUMERIC (18) NULL,
    [EntryDate] DATETIME     NULL,
    [EnteredBy] NUMERIC (18) NULL
);

