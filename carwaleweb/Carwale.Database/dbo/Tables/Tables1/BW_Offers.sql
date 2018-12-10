CREATE TABLE [dbo].[BW_Offers] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [MaskingName]     VARCHAR (50)  NOT NULL,
    [OfferTitle]      VARCHAR (500) NOT NULL,
    [OfferValue]      INT           NULL,
    [OfferContent]    VARCHAR (MAX) NULL,
    [Terms]           VARCHAR (MAX) NULL,
    [CreatedDate]     DATETIME      NULL,
    [LastUpdatedDate] DATETIME      NULL,
    [UpdatedBy]       INT           NULL,
    [ExpiryDate]      DATETIME      NULL,
    [IsActive]        BIT           NULL,
    CONSTRAINT [Unique_masking] UNIQUE NONCLUSTERED ([MaskingName] ASC)
);

