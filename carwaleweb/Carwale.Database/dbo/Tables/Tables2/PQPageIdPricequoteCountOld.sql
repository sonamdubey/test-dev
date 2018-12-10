CREATE TABLE [dbo].[PQPageIdPricequoteCountOld] (
    [pqPageId]           INT          NOT NULL,
    [pqPageIdCount]      NUMERIC (18) NULL,
    [pqPageId4HourCount] INT          NULL,
    CONSTRAINT [PK_PQPageIdPricequoteCount] PRIMARY KEY CLUSTERED ([pqPageId] ASC)
);

