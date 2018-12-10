CREATE TABLE [dbo].[DCRM_PackagesClosingStage] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ClosingStage] INT           NOT NULL,
    [Description]  VARCHAR (100) NOT NULL,
    [IsActive]     BIT           NULL,
    [DisplayName]  VARCHAR (50)  NULL,
    CONSTRAINT [PK_DCRM_PackagesClosingStage] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Closing Probability stage of packages', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_PackagesClosingStage', @level2type = N'COLUMN', @level2name = N'ClosingStage';

