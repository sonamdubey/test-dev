CREATE TABLE [dbo].[DCRM_ADM_Alerts] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (150) NOT NULL,
    [Code]        NVARCHAR (30)  NOT NULL,
    [RoleId]      INT            NOT NULL,
    [Priority]    SMALLINT       NOT NULL,
    [IsActive]    BIT            CONSTRAINT [DF_DCRM_ADM_Alerts_IsActive] DEFAULT ((1)) NOT NULL,
    [Type]        SMALLINT       CONSTRAINT [DF_DCRM_ADM_Alerts_Type] DEFAULT ((1)) NOT NULL,
    [Description] NVARCHAR (250) NULL,
    [ShName]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_DCRM_ADM_Alerts] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-PreSales, 2-Sales BO, 3-Sales Field, 4-Service BO, 5-Service Field', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_ADM_Alerts', @level2type = N'COLUMN', @level2name = N'RoleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Automated, 2-Manual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_ADM_Alerts', @level2type = N'COLUMN', @level2name = N'Type';

