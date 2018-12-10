CREATE TABLE [dbo].[NewCarQuoteType] (
    [MakeId]    INT     NOT NULL,
    [CityId]    INT     NOT NULL,
    [QuoteType] TINYINT NOT NULL,
    [IsActive]  BIT     CONSTRAINT [DF_NewCarQuoteType_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_NewCarQuoteType] PRIMARY KEY CLUSTERED ([MakeId] ASC, [CityId] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This table will be used to take decision like which quote should be shown(price quote or finance quote). If quote type is 1 then its ''Price Quote'' and if its 2 than ''Finance Quote''', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NewCarQuoteType', @level2type = N'COLUMN', @level2name = N'IsActive';

