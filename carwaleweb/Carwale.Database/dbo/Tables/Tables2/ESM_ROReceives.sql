CREATE TABLE [dbo].[ESM_ROReceives] (
    [Id]                    INT          IDENTITY (1, 1) NOT NULL,
    [ESM_ProposedProductId] INT          NULL,
    [Discount]              NUMERIC (18) NULL,
    [FinalRoValue]          NUMERIC (18) NULL,
    CONSTRAINT [PK_ESM_ROReceives] PRIMARY KEY CLUSTERED ([Id] ASC)
);

