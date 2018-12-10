CREATE TABLE [dbo].[LDCities] (
    [LDTakerId] INT          NOT NULL,
    [CityId]    NUMERIC (18) NOT NULL,
    [LDType]    SMALLINT     CONSTRAINT [DF_LDCities_LDType] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_LDCities] PRIMARY KEY CLUSTERED ([LDTakerId] ASC, [CityId] ASC, [LDType] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for new, 2 for loan, 3 for insurance', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LDCities', @level2type = N'COLUMN', @level2name = N'LDType';

