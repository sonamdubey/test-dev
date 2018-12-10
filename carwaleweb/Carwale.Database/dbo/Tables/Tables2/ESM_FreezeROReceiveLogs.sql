CREATE TABLE [dbo].[ESM_FreezeROReceiveLogs] (
    [Id]                    INT          IDENTITY (1, 1) NOT NULL,
    [ESM_ProposedProductId] INT          NULL,
    [Discount]              NUMERIC (18) NULL,
    [FinalROValue]          NUMERIC (18) NULL,
    [UpdatedBy]             INT          NULL,
    [UpdatedOn]             DATETIME     NULL,
    CONSTRAINT [PK_ESM_FreezeROReceiveLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This Table is used for log data which have 100 percent probability(Final Ro Receive)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESM_FreezeROReceiveLogs', @level2type = N'COLUMN', @level2name = N'Id';

