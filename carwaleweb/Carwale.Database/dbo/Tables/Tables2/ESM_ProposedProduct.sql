CREATE TABLE [dbo].[ESM_ProposedProduct] (
    [id]                            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProposedId]                    NUMERIC (18) NOT NULL,
    [ProductTypeId]                 NUMERIC (18) NOT NULL,
    [ProductId]                     NUMERIC (18) NOT NULL,
    [Quantity]                      NUMERIC (18) NOT NULL,
    [MRPAmount]                     NUMERIC (18) NOT NULL,
    [ProposedAmount]                NUMERIC (18) NOT NULL,
    [CreatedOn]                     DATETIME     NULL,
    [UpdatedOn]                     DATETIME     NULL,
    [UpdatedBy]                     NUMERIC (18) NOT NULL,
    [ProductProbability]            NUMERIC (18) NULL,
    [ProductLastProbabilityUpdated] NUMERIC (18) NULL,
    [Property]                      INT          NULL,
    [Platform]                      INT          NULL,
    CONSTRAINT [PK_ESM_ProposedProduct] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

