CREATE TABLE [dbo].[PQ_Template_Category] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [CategoryName]       VARCHAR (100) NULL,
    [TemplateDisciption] VARCHAR (200) NULL,
    CONSTRAINT [PK_PQ_Template_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);

