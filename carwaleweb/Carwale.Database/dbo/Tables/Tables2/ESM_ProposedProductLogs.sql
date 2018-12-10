CREATE TABLE [dbo].[ESM_ProposedProductLogs] (
    [Id]                    INT          IDENTITY (1, 1) NOT NULL,
    [ESM_ProposedProductId] INT          NULL,
    [ProposedId]            INT          NULL,
    [ProductTypeId]         INT          NULL,
    [ProductId]             INT          NULL,
    [Property]              INT          NULL,
    [Platform]              INT          NULL,
    [Quantity]              INT          NULL,
    [MRPAmount]             NUMERIC (18) NULL,
    [ProposedAmount]        NUMERIC (18) NULL,
    [Probability]           INT          NULL,
    [CreatedOn]             DATETIME     NULL,
    [UpdatedOn]             DATETIME     NULL,
    [UpdatedBy]             NUMERIC (18) NULL,
    CONSTRAINT [PK_ESM_ProposedProductLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

