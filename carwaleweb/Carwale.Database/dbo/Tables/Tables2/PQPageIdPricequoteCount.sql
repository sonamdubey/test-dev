CREATE TABLE [dbo].[PQPageIdPricequoteCount] (
    [pqPageId]           INT          NOT NULL,
    [pqPageIdCount]      NUMERIC (18) NULL,
    [pqPageId4HourCount] INT          NULL,
    [HourPart]           TINYINT      NULL,
    [updatedon]          DATETIME     DEFAULT (getdate()) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_PQPageIdPricequoteCount_PqpageId]
    ON [dbo].[PQPageIdPricequoteCount]([pqPageId] ASC, [HourPart] ASC);

