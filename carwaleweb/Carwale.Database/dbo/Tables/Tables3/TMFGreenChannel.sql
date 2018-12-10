CREATE TABLE [dbo].[TMFGreenChannel] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerName] VARCHAR (100)  NULL,
    [CityId]       NUMERIC (18)   NULL,
    [Email]        VARCHAR (100)  NULL,
    [PhoneNo]      VARCHAR (50)   NULL,
    [MobileNo]     VARCHAR (50)   NULL,
    [CarName]      VARCHAR (50)   NULL,
    [EntryDate]    DATETIME       NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_TMFGreenChannel_IsActive] DEFAULT (1) NOT NULL,
    [Criterias]    VARCHAR (5000) NULL,
    CONSTRAINT [PK_TMFGreenChannel] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

