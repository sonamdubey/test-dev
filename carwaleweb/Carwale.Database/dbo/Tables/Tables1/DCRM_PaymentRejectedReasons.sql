CREATE TABLE [dbo].[DCRM_PaymentRejectedReasons] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (100) NOT NULL,
    [IsActive] BIT           CONSTRAINT [DF_DCRM_PaymentRejectedReasons_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DCRM_PaymentRejectedReasons] PRIMARY KEY CLUSTERED ([Id] ASC)
);

