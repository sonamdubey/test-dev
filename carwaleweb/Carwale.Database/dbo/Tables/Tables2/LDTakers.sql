CREATE TABLE [dbo].[LDTakers] (
    [ID]       INT           NOT NULL,
    [Name]     VARCHAR (200) NOT NULL,
    [Type]     SMALLINT      NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_LDTakers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-NewCar, 2-Finance, 3-Insurance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LDTakers', @level2type = N'COLUMN', @level2name = N'Type';

