CREATE TABLE [dbo].[CW_CompanyList] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [CompanyCode]          VARCHAR (200)  NULL,
    [CompanyName]          VARCHAR (1000) NULL,
    [CW_CompanyCategoryId] INT            NULL,
    [CompanyGroup]         VARCHAR (1000) NULL,
    [CompanyType]          VARCHAR (200)  NULL,
    [IsActive]             BIT            CONSTRAINT [DF_CW_CompanyList_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]            DATETIME       CONSTRAINT [DF_CW_CompanyList_EntryDate] DEFAULT (getdate()) NULL,
    [UpdatedBy]            INT            NULL,
    [UpdatedOn]            DATETIME       NULL,
    [ClientId]             INT            NULL,
    CONSTRAINT [PK_CW_CompanyList] PRIMARY KEY CLUSTERED ([Id] ASC)
);

