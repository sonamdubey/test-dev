CREATE TABLE [dbo].[Microsite_DealerSeoDetails] (
    [Id]                       INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]                 INT           NOT NULL,
    [Microsite_WebsitePagesId] INT           NOT NULL,
    [TitleTag]                 VARCHAR (250) NULL,
    [KeywordsTag]              VARCHAR (200) NULL,
    [DescriptionTag]           VARCHAR (300) NULL,
    [CreatedOn]                DATETIME      CONSTRAINT [DF_Microsite_DealerSeoDetails_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]                DATETIME      NULL,
    [UpdatedBy]                INT           NOT NULL,
    CONSTRAINT [PK_Microsite_DealerSeoDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

