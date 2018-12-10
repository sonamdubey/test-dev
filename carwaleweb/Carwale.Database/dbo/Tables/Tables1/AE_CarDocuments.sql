CREATE TABLE [dbo].[AE_CarDocuments] (
    [CarId]      NUMERIC (18) NOT NULL,
    [DocumentId] NUMERIC (18) NOT NULL,
    [UpdatedOn]  DATETIME     NULL,
    [UpdatedBy]  NUMERIC (18) NULL,
    CONSTRAINT [PK_AE_CarDocuments_1] PRIMARY KEY CLUSTERED ([CarId] ASC, [DocumentId] ASC) WITH (FILLFACTOR = 90)
);

