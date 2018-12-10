CREATE TABLE [dbo].[AffiliateMembers] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LoginId]         VARCHAR (100) NOT NULL,
    [Passwd]          VARCHAR (50)  NOT NULL,
    [ContactPerson]   VARCHAR (100) NULL,
    [ContactEmail]    VARCHAR (100) NULL,
    [ContactNumber]   VARCHAR (100) NULL,
    [FaxNumber]       VARCHAR (100) NULL,
    [ChequePayableTo] VARCHAR (200) NULL,
    [CompanyName]     VARCHAR (200) NULL,
    [Address1]        VARCHAR (150) NULL,
    [Address2]        VARCHAR (150) NULL,
    [CityId]          NUMERIC (18)  NULL,
    [PinNo]           VARCHAR (10)  NULL,
    [PanNumber]       VARCHAR (50)  NULL,
    [PaypalEmailId]   VARCHAR (100) NULL,
    [IsActive]        BIT           CONSTRAINT [DF_AffiliateMembers_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_AffiliateMembers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

