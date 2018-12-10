CREATE TABLE [dbo].[Packages] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Name]               VARCHAR (50)   NOT NULL,
    [Validity]           INT            NOT NULL,
    [InquiryPoints]      INT            NOT NULL,
    [InqPtCategoryId]    INT            NULL,
    [ForDealer]          BIT            CONSTRAINT [DF_Packages_ForDealer] DEFAULT ((0)) NOT NULL,
    [ForIndividual]      BIT            CONSTRAINT [DF_Packages_ForIndividual] DEFAULT ((0)) NOT NULL,
    [Amount]             INT            NOT NULL,
    [isActive]           BIT            CONSTRAINT [DF_Packages_isActive] DEFAULT ((1)) NOT NULL,
    [Description]        VARCHAR (1000) NULL,
    [IsInternal]         BIT            CONSTRAINT [DF_Packages_IsInternal] DEFAULT ((0)) NOT NULL,
    [IsTopup]            BIT            CONSTRAINT [DF_Packages_IsTopup] DEFAULT ((0)) NULL,
    [IsStockBased]       BIT            CONSTRAINT [DF_Packages_IsStockBased] DEFAULT ((1)) NULL,
    [ClassifiedValidity] INT            NULL,
    [TermsAndConditions] VARCHAR (MAX)  NULL,
    [HasShowroom]        BIT            DEFAULT ((0)) NULL,
    [RenewValidity]      INT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Packages] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

