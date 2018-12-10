CREATE TABLE [dbo].[TC_MM_ContactDetails] (
    [TC_MM_ContactDetailsId] INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]               INT           NULL,
    [Quantity]               INT           NULL,
    [Email]                  VARCHAR (100) NULL,
    [PhoneNumber]            VARCHAR (10)  NULL,
    [CreatedOn]              DATETIME      NULL,
    [CreatedBy]              INT           NULL,
    [IsMailSent]             BIT           NULL
);

