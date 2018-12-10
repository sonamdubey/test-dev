CREATE TABLE [dbo].[Microsite_WebsitePages] (
    [Id]             INT           NOT NULL,
    [PageName]       VARCHAR (50)  NOT NULL,
    [TitleTag]       VARCHAR (250) NULL,
    [KeywordsTag]    VARCHAR (200) NULL,
    [DescriptionTag] VARCHAR (300) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_Microsite_WebsitePages_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Microsite_WebsitePages] PRIMARY KEY CLUSTERED ([Id] ASC)
);

