CREATE TABLE [dbo].[ESM_DeleteProposedProducts] (
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,
    [ProposedId]                    NUMERIC (18)  NULL,
    [ProductTypeId]                 NUMERIC (18)  NULL,
    [ProductId]                     NUMERIC (18)  NULL,
    [Quantity]                      NUMERIC (18)  NULL,
    [MRPAmount]                     NUMERIC (18)  NULL,
    [ProposedAmount]                NUMERIC (18)  NULL,
    [ProductProbability]            NUMERIC (18)  NULL,
    [ProductLastProbabilityUpdated] NUMERIC (18)  NULL,
    [DeletedReason]                 VARCHAR (500) NULL,
    [DeletedOn]                     DATETIME      NULL,
    [DeletedBy]                     NCHAR (10)    NULL,
    CONSTRAINT [PK_ESM_DeleteProposedProducts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

