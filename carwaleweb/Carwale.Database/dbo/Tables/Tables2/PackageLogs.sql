CREATE TABLE [dbo].[PackageLogs] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [InqPtCategoryId] INT            NOT NULL,
    [PackageId]       NUMERIC (18)   NOT NULL,
    [UserId]          NUMERIC (18)   NOT NULL,
    [Name]            VARCHAR (50)   NOT NULL,
    [Validity]        INT            NOT NULL,
    [InquiryPoints]   INT            NOT NULL,
    [ForDealer]       BIT            CONSTRAINT [DF_PackageLogs_ForDealer] DEFAULT ((0)) NOT NULL,
    [ForIndividual]   BIT            CONSTRAINT [DF_PackageLogs_ForIndividual] DEFAULT ((0)) NOT NULL,
    [Amount]          NUMERIC (18)   NOT NULL,
    [EditDate]        DATETIME       NOT NULL,
    [Description]     VARCHAR (1000) NULL,
    [IsActive]        BIT            CONSTRAINT [DF_PackageLogs_IsActive] DEFAULT ((1)) NOT NULL,
    [IsTopup]         BIT            CONSTRAINT [DF_PackageLogs_IsTopup] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PackageLogs] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

