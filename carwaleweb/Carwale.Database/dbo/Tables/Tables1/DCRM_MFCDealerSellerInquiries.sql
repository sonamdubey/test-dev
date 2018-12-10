CREATE TABLE [dbo].[DCRM_MFCDealerSellerInquiries] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [LeadID]             NUMERIC (18)  NOT NULL,
    [Name]               VARCHAR (100) NULL,
    [Mobile]             VARCHAR (50)  NULL,
    [Phone]              VARCHAR (50)  NULL,
    [Email]              VARCHAR (100) NULL,
    [City]               VARCHAR (100) NULL,
    [Make]               VARCHAR (50)  NULL,
    [Model]              VARCHAR (50)  NULL,
    [Variant]            VARCHAR (50)  NULL,
    [ModelYear]          VARCHAR (10)  NULL,
    [RegistrationYear]   VARCHAR (10)  NULL,
    [Color]              VARCHAR (50)  NULL,
    [Kilometer]          VARCHAR (50)  NULL,
    [Owners]             VARCHAR (50)  NULL,
    [RegistrationNumber] VARCHAR (50)  NULL,
    [RegistrationCity]   VARCHAR (50)  NULL,
    [ReturnResult]       NUMERIC (18)  NOT NULL,
    [LeadDate]           DATETIME      CONSTRAINT [DF_DCRM_MFCDealerSellerInquiries_LeadDate] DEFAULT (getdate()) NULL
);

