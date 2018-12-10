CREATE TABLE [dbo].[DealerOffersVersion] (
    [OfferId]   NUMERIC (18) NOT NULL,
    [MakeId]    NUMERIC (18) NULL,
    [ModelId]   NUMERIC (18) NULL,
    [VersionId] NUMERIC (18) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_DealerOffersVersion_OfferId]
    ON [dbo].[DealerOffersVersion]([OfferId] ASC);

