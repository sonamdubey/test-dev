CREATE TABLE [dbo].[DealerWebsite_SEOContent] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [PageId]          INT            NULL,
    [PageName]        VARCHAR (100)  NULL,
    [DealerId]        INT            NULL,
    [Title]           VARCHAR (500)  NULL,
    [MetaKeywords]    VARCHAR (1000) NULL,
    [MetaDescription] VARCHAR (MAX)  NULL,
    [DateCreated]     DATETIME       NULL,
    [DateUpdated]     DATETIME       NULL,
    [IsActive]        BIT            NULL
);

