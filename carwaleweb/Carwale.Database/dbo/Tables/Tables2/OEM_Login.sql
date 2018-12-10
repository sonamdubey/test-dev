CREATE TABLE [dbo].[OEM_Login] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [LoginId]  VARCHAR (150) NOT NULL,
    [Password] VARCHAR (150) NOT NULL,
    [Name]     VARCHAR (150) NOT NULL,
    [MakeId]   NUMERIC (18)  NOT NULL,
    [RoleId]   INT           NULL,
    [IsActive] BIT           CONSTRAINT [DF_OEMLogin_IsActive] DEFAULT ((1)) NOT NULL,
    [RegionId] INT           NULL,
    CONSTRAINT [PK_OEMLogin] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Superadmin, 2-Admin, 3-Regional Manager', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OEM_Login', @level2type = N'COLUMN', @level2name = N'RoleId';

