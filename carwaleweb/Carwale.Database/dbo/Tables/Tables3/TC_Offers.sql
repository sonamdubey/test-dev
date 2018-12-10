CREATE TABLE [dbo].[TC_Offers] (
    [TC_OffersId]   INT          NOT NULL,
    [OfferName]     VARCHAR (50) NULL,
    [IsActive]      BIT          NULL,
    [MakeId]        INT          NULL,
    [TC_OffersType] INT          NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1 for Retail(New Car)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_Offers', @level2type = N'COLUMN', @level2name = N'TC_OffersType';

