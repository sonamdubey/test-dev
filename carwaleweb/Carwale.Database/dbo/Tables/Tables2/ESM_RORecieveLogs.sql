CREATE TABLE [dbo].[ESM_RORecieveLogs] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [ProposalId]   INT          NULL,
    [Discount]     NUMERIC (18) NULL,
    [FinalROValue] NUMERIC (18) NULL,
    [UpdatedBy]    INT          NULL,
    [UpdatedOn]    DATETIME     NULL,
    CONSTRAINT [PK_ESM_FreezeRORecieveLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This table is used for Log data when Probility get 100 percent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESM_RORecieveLogs', @level2type = N'COLUMN', @level2name = N'Id';

