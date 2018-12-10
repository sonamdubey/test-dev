CREATE TABLE [dbo].[DCRM_PackageStatusReasons] (
    [Id]           NUMERIC (18) NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    [IsOpenReason] BIT          NULL,
    [IsLostReason] BIT          NULL,
    CONSTRAINT [PK_DCRM_PackageStatusReasons] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Package open reasons', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_PackageStatusReasons', @level2type = N'COLUMN', @level2name = N'IsOpenReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Package Lost Reasons', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_PackageStatusReasons', @level2type = N'COLUMN', @level2name = N'IsLostReason';

