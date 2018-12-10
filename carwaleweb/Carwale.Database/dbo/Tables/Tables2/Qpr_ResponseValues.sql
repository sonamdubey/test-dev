CREATE TABLE [dbo].[Qpr_ResponseValues] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Text]     VARCHAR (50) NULL,
    [Value]    SMALLINT     NULL,
    [Priority] SMALLINT     NULL,
    [Type]     SMALLINT     NULL,
    CONSTRAINT [PK__Qpr_Resp__3214EC07096F09E1] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for Strongly etc, 2 for Agree etc', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Qpr_ResponseValues', @level2type = N'COLUMN', @level2name = N'Type';

