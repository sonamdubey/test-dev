CREATE TABLE [dbo].[TC_CorporateList] (
    [TC_CorporateListId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]               VARCHAR (250) NULL,
    [MakeId]             INT           NULL,
    [IsActive]           BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([TC_CorporateListId] ASC)
);

