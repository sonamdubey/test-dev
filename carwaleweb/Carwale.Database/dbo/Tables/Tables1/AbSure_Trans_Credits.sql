CREATE TABLE [dbo].[AbSure_Trans_Credits] (
    [ID]           NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [DealerId]     NUMERIC (18)    NOT NULL,
    [CreditAmount] NUMERIC (18, 2) NULL,
    [CreditDate]   DATETIME        CONSTRAINT [DF_AbSure_Trans_DealerCredits_CreditDate] DEFAULT (getdate()) NOT NULL,
    [CreditedBy]   INT             NOT NULL,
    [DiscountPer]  FLOAT (53)      NULL,
    [NoOfCars]     SMALLINT        NOT NULL,
    [ActivationId] INT             NULL,
    CONSTRAINT [PK_AbSure_Trans_DealerCredits] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id of ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AbSure_Trans_Credits', @level2type = N'COLUMN', @level2name = N'ActivationId';

