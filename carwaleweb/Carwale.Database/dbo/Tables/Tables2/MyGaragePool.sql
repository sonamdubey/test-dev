CREATE TABLE [dbo].[MyGaragePool] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (100) NULL,
    [Email]         VARCHAR (100) NULL,
    [City]          VARCHAR (100) NULL,
    [Phone]         VARCHAR (50)  NULL,
    [Mobile]        VARCHAR (50)  NULL,
    [Car]           VARCHAR (100) NULL,
    [OtherComments] VARCHAR (500) NULL,
    [PurchaseDate]  DATETIME      NULL,
    [IsAssigned]    BIT           CONSTRAINT [DF_MyGaragePool_IsAssigned] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_MyGarageCustomers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

