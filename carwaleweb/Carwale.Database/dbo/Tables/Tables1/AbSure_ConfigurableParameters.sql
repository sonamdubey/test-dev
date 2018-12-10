CREATE TABLE [dbo].[AbSure_ConfigurableParameters] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Category]      VARCHAR (MAX) NOT NULL,
    [Parameter]     VARCHAR (100) NOT NULL,
    [MinValue]      INT           NULL,
    [MaxValue]      INT           NULL,
    [ConstantValue] INT           NULL,
    [EnteredBy]     INT           NULL,
    [EntryDate]     DATETIME      NULL,
    [UpdatedBy]     INT           NULL,
    [UpdatedOn]     DATETIME      NULL,
    [IsActive]      BIT           NULL,
    [Sequence]      INT           NULL,
    CONSTRAINT [PK_AbSure_ConfigurableParameters] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text to identify the parameters', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AbSure_ConfigurableParameters', @level2type = N'COLUMN', @level2name = N'Category';

