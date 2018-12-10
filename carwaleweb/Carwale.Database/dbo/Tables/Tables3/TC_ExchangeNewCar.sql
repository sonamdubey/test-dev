CREATE TABLE [dbo].[TC_ExchangeNewCar] (
    [TC_NewCarInquiriesId] INT  NULL,
    [CarVersionId]         INT  NULL,
    [Kms]                  INT  NULL,
    [MakeYear]             DATE NULL,
    [ExpectedPrice]        INT  NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_ExchangeNewCar_TC_NewCarInquiriesId_CarVersionId]
    ON [dbo].[TC_ExchangeNewCar]([TC_NewCarInquiriesId] ASC, [CarVersionId] ASC);

