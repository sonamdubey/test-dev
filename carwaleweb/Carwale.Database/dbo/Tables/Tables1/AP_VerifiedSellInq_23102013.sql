CREATE TABLE [dbo].[AP_VerifiedSellInq_23102013] (
    [APV_Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [SellInqId]        NUMERIC (18) NOT NULL,
    [VerificationDate] DATETIME     NOT NULL,
    [Status]           SMALLINT     NOT NULL
);

