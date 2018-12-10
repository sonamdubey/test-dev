CREATE TABLE [dbo].[Microsite_Customers] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NULL,
    [EmailId]      VARCHAR (50) NULL,
    [MobileNum]    VARCHAR (10) NULL,
    [EntryDate]    DATETIME     DEFAULT (getdate()) NULL,
    [ModifiedDate] DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

