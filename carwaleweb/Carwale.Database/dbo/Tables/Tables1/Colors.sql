CREATE TABLE [dbo].[Colors] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]            VARCHAR (100) NOT NULL,
    [BackgroundColor] VARCHAR (50)  NOT NULL,
    [Color]           VARCHAR (50)  NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Colors_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

