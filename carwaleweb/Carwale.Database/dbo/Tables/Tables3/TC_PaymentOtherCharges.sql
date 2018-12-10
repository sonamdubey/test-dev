CREATE TABLE [dbo].[TC_PaymentOtherCharges] (
    [TC_PaymentOtherCharges_Id] INT           IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]          INT           NOT NULL,
    [TC_PaymentVariables_Id]    INT           NOT NULL,
    [Amount]                    DECIMAL (18)  NULL,
    [Comments]                  VARCHAR (100) NULL,
    [EntryDate]                 DATETIME      DEFAULT (getdate()) NULL,
    [ModifiedBy]                INT           NULL,
    [ModifiedDate]              DATETIME      NULL,
    CONSTRAINT [PK_TC_PaymentIncludes] PRIMARY KEY CLUSTERED ([TC_PaymentOtherCharges_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id of TC_payment table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_PaymentOtherCharges', @level2type = N'COLUMN', @level2name = N'TC_CarBooking_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id of Tc_Variable table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_PaymentOtherCharges', @level2type = N'COLUMN', @level2name = N'TC_PaymentVariables_Id';

