CREATE TABLE [dbo].[OLM_RapidCarAmounts] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [VersionId]      INT          NOT NULL,
    [FinanceAmount]  NUMERIC (18) NOT NULL,
    [Emi]            NUMERIC (18) NOT NULL,
    [BalloonPayment] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_OLM_RapidCarAmounts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

