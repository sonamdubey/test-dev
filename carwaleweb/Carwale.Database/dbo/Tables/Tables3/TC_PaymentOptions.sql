CREATE TABLE [dbo].[TC_PaymentOptions] (
    [TC_PaymentOptions_Id] TINYINT      IDENTITY (1, 1) NOT NULL,
    [OptionName]           VARCHAR (10) NOT NULL,
    [IsActive]             BIT          CONSTRAINT [DF_TC_PaymentOptions_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]            DATETIME     DEFAULT (getdate()) NULL,
    [ModifiedBy]           INT          NULL,
    [ModifiedDate]         DATETIME     NULL,
    CONSTRAINT [PK_TC_PaymentOptions] PRIMARY KEY CLUSTERED ([TC_PaymentOptions_Id] ASC)
);

