CREATE TABLE [dbo].[PQPageIdPricequoteCount2] (
    [pqPageId]           INT          NOT NULL,
    [pqPageIdCount]      NUMERIC (18) NULL,
    [pqPageId4HourCount] INT          NULL,
    [HourPart]           TINYINT      NULL,
    CONSTRAINT [PK_PQPageIdPricequoteCount21] PRIMARY KEY CLUSTERED ([pqPageId] ASC)
);

