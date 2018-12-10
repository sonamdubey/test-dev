CREATE TABLE [dbo].[TmfFinance] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerName] VARCHAR (100) NOT NULL,
    [CityId]       NUMERIC (18)  NOT NULL,
    [Email]        VARCHAR (100) NOT NULL,
    [PhoneNo]      VARCHAR (50)  NULL,
    [MobileNo]     VARCHAR (50)  NULL,
    [CarName]      VARCHAR (50)  NOT NULL,
    [EntryDate]    DATETIME      NOT NULL,
    [IsActive]     BIT           CONSTRAINT [DF_TmfFinance_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_TmfFinance] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

