CREATE TABLE [dbo].[DCRM_PaymentTransaction] (
    [TransactionId]      INT             IDENTITY (1, 1) NOT NULL,
    [TotalClosingAmount] INT             NULL,
    [DiscountAmount]     INT             NULL,
    [ProductAmount]      INT             NULL,
    [ServiceTax]         FLOAT (53)      NULL,
    [IsTDSGiven]         BIT             NULL,
    [TDSAmount]          DECIMAL (18, 2) NULL,
    [FinalAmount]        DECIMAL (18, 2) NULL,
    [PANNumber]          VARCHAR (10)    NULL,
    [TANNumber]          VARCHAR (15)    NULL,
    [CreatedBy]          INT             NULL,
    [CreatedOn]          DATETIME        NULL,
    [UpdatedBy]          INT             NULL,
    [UpdatedOn]          DATETIME        NULL,
    [IsActive]           INT             DEFAULT ((1)) NOT NULL,
    [Comments]           VARCHAR (1000)  NULL,
    [Source]             SMALLINT        DEFAULT ((1)) NULL,
    CONSTRAINT [PK_DCRM_PaymentTransaction] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contains Id of DCRM_SalesDealer table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DCRM_PaymentTransaction', @level2type = N'COLUMN', @level2name = N'TotalClosingAmount';

