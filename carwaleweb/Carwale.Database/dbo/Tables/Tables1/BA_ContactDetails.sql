CREATE TABLE [dbo].[BA_ContactDetails] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [ContactId]       INT          NOT NULL,
    [DealerId]        INT          NOT NULL,
    [Email]           VARCHAR (50) NULL,
    [ContactName]     VARCHAR (50) NULL,
    [IsCarWaleDealer] TINYINT      NOT NULL,
    [IsActive]        BIT          NOT NULL,
    CONSTRAINT [PK_BA_ContactDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);

