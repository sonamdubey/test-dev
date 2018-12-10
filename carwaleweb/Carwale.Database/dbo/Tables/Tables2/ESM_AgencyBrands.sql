CREATE TABLE [dbo].[ESM_AgencyBrands] (
    [id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ContactId] NUMERIC (18) NOT NULL,
    [ClientId]  NUMERIC (18) NOT NULL,
    [BrandId]   NUMERIC (18) NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ESM_AgencyBrands] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

