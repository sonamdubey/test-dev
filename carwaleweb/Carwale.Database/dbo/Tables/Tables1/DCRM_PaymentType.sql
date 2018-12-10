CREATE TABLE [dbo].[DCRM_PaymentType] (
    [PaymentTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [Description]   VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_DCRM_PaymentType] PRIMARY KEY CLUSTERED ([PaymentTypeId] ASC)
);

