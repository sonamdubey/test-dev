CREATE TABLE [dbo].[NCS_IndependentDeal] (
    [ID]          NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]    NUMERIC (18)    NOT NULL,
    [VersionId]   NUMERIC (18)    NOT NULL,
    [Discount]    DECIMAL (18, 2) NULL,
    [LastUpdated] DATETIME        NOT NULL,
    [Comments]    VARCHAR (200)   NULL,
    CONSTRAINT [PK_INS_IndependentDeal] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

