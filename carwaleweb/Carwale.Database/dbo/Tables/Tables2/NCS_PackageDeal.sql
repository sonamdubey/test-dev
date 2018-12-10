CREATE TABLE [dbo].[NCS_PackageDeal] (
    [ID]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]      NUMERIC (18)    NOT NULL,
    [VersionId]     NUMERIC (18)    NOT NULL,
    [InsPremium]    DECIMAL (18, 2) NULL,
    [TotalDiscount] DECIMAL (18, 2) NULL,
    [LastUpdated]   DATETIME        NOT NULL,
    [Comments]      VARCHAR (200)   NULL,
    CONSTRAINT [PK_INS_PackageDeal] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

