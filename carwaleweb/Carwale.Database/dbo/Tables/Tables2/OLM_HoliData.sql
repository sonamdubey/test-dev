CREATE TABLE [dbo].[OLM_HoliData] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (100) NULL,
    [Email]      VARCHAR (100) NULL,
    [CreatedOn]  DATETIME      CONSTRAINT [DF_OLM_HoliData_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [ImgHostUrl] VARCHAR (50)  NULL,
    CONSTRAINT [PK_OLM_HoliData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

