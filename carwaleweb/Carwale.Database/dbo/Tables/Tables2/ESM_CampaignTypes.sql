CREATE TABLE [dbo].[ESM_CampaignTypes] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [UpdatedBy] INT          NULL,
    [UpdatedOn] DATETIME     NULL,
    [IsActive]  BIT          NULL,
    CONSTRAINT [PK_ESM_CampaignTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This table is used For Enterprise Sale   ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ESM_CampaignTypes', @level2type = N'COLUMN', @level2name = N'Name';

