CREATE TABLE [dbo].[AE_PaymentDebits] (
    [Id]           NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BidderId]     NUMERIC (18)    NOT NULL,
    [AuctionCarId] NUMERIC (18)    NOT NULL,
    [Type]         SMALLINT        NULL,
    [Amount]       DECIMAL (18, 2) NOT NULL,
    [Comment]      VARCHAR (250)   NULL,
    [EntryDate]    DATETIME        NOT NULL,
    [UpdatedOn]    DATETIME        NULL,
    [UpdatedBy]    NUMERIC (18)    NULL,
    CONSTRAINT [PK_AE_PaymentDebits] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Purchase, 2-Forfeit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AE_PaymentDebits', @level2type = N'COLUMN', @level2name = N'Type';

