CREATE TABLE [dbo].[AP_VerifiedSellInq] (
    [APV_Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SellInqId]        NUMERIC (18) NOT NULL,
    [VerificationDate] DATETIME     NOT NULL,
    [Status]           SMALLINT     CONSTRAINT [DF_AP_VerifiedSellInq_Status] DEFAULT ((2)) NOT NULL,
    [IsProcessed]      BIT          CONSTRAINT [DF_AP_VerifiedSellInq_IsProcessed] DEFAULT ((0)) NULL,
    [ProcessDate]      DATETIME     NULL,
    CONSTRAINT [PK_AP_VerifiedSellInq] PRIMARY KEY CLUSTERED ([APV_Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-New, 2-Onhold', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AP_VerifiedSellInq', @level2type = N'COLUMN', @level2name = N'Status';

