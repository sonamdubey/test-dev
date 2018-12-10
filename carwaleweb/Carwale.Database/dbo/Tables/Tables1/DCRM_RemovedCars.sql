CREATE TABLE [dbo].[DCRM_RemovedCars] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [InquiryId]   NUMERIC (18)  NOT NULL,
    [Reason]      NUMERIC (18)  NOT NULL,
    [SoldMedium]  NUMERIC (18)  NULL,
    [SoldPrice]   NUMERIC (18)  NULL,
    [Soldto]      SMALLINT      NULL,
    [RemovalDate] DATETIME      CONSTRAINT [DF_DCRM_RemovedCars_RemovalDate] DEFAULT (getdate()) NOT NULL,
    [RemovedBy]   NUMERIC (18)  NOT NULL,
    [Comments]    VARCHAR (500) NULL,
    CONSTRAINT [PK_DCRM_RemovedCars] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Dealer, 2-Individual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_RemovedCars', @level2type = N'COLUMN', @level2name = N'Soldto';

