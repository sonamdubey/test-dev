CREATE TABLE [dbo].[Acc_Seller] (
    [Id]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SellerName]     VARCHAR (100) NOT NULL,
    [Address]        VARCHAR (200) NULL,
    [CityId]         INT           NOT NULL,
    [Email]          VARCHAR (100) NULL,
    [AlternateEmail] VARCHAR (300) NULL,
    [Phone]          VARCHAR (50)  NULL,
    [AlternatePhone] VARCHAR (50)  NULL,
    [Fax]            VARCHAR (50)  NULL,
    [Mobile]         NUMERIC (18)  NULL,
    [JoiningDate]    DATETIME      NOT NULL,
    [IsActive]       BIT           CONSTRAINT [DF_Accessories_Seller_IsActive] DEFAULT (1) NOT NULL,
    [IsApproved]     BIT           CONSTRAINT [DF_Accessories_Seller_IsApproved] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_Accessories_Seller] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

