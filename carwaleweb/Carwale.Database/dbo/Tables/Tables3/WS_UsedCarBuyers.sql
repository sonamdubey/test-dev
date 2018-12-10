CREATE TABLE [dbo].[WS_UsedCarBuyers] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ProfileId] VARCHAR (50)  NULL,
    [Name]      VARCHAR (150) NULL,
    [Email]     VARCHAR (50)  NULL,
    [Mobile]    VARCHAR (50)  NULL,
    [EntryDate] DATETIME      CONSTRAINT [DF_WS_UsedCarBuyers_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WS_UsedCarBuyers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

