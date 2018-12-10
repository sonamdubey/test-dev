CREATE TABLE [dbo].[Skoda_CustomerCare] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)  NULL,
    [EmailId]        VARCHAR (100) NULL,
    [Tele/Mobile]    VARCHAR (10)  NULL,
    [City]           INT           NULL,
    [Model]          VARCHAR (20)  NOT NULL,
    [ChasisNo]       VARCHAR (50)  NULL,
    [RegistrationNo] VARCHAR (50)  NULL,
    [Dateofpurchase] DATE          NOT NULL,
    [KMsRun]         NUMERIC (18)  NOT NULL,
    [SellingDealer]  VARCHAR (50)  NULL,
    [ServiceDealer]  VARCHAR (50)  NULL,
    [Concern]        VARCHAR (250) NOT NULL,
    [EntryDate]      DATETIME      NULL,
    CONSTRAINT [PK_Skoda_CustomerCare] PRIMARY KEY CLUSTERED ([Id] ASC)
);

