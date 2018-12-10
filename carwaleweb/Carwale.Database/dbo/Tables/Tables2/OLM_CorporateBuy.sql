CREATE TABLE [dbo].[OLM_CorporateBuy] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [FullName]        VARCHAR (150) NOT NULL,
    [Email]           VARCHAR (100) NOT NULL,
    [Mobile]          VARCHAR (15)  NOT NULL,
    [Landline]        VARCHAR (15)  NULL,
    [Organisation]    VARCHAR (50)  NOT NULL,
    [City]            NUMERIC (18)  NOT NULL,
    [Address]         VARCHAR (500) NOT NULL,
    [PreferredDealer] NUMERIC (18)  NOT NULL,
    [Testdrive]       BIT           CONSTRAINT [DF_OLM_CorporateBuy_Testdrive] DEFAULT ((0)) NOT NULL,
    [Message]         VARCHAR (500) NULL,
    [IPAddress]       VARCHAR (50)  NULL,
    [LeaseCompany]    NUMERIC (18)  NULL,
    [LeadDate]        DATETIME      CONSTRAINT [DF_OLM_CorporateBuy_LeadDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_OLM_CorporateBuy] PRIMARY KEY CLUSTERED ([Id] ASC)
);

