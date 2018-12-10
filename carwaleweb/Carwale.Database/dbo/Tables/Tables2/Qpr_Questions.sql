CREATE TABLE [dbo].[Qpr_Questions] (
    [Id]          INT            NOT NULL,
    [Type]        TINYINT        NOT NULL,
    [Description] VARCHAR (1000) NULL,
    [Priority]    TINYINT        NULL,
    CONSTRAINT [PK_Qpr_Questions] PRIMARY KEY CLUSTERED ([Id] ASC, [Type] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for Self Rating, 2 for Manager, 3 For Both', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Qpr_Questions', @level2type = N'COLUMN', @level2name = N'Type';

