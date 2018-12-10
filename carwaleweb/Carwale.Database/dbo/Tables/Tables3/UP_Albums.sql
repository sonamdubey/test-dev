CREATE TABLE [dbo].[UP_Albums] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]         VARCHAR (200) NULL,
    [Description]  VARCHAR (500) NULL,
    [CreationDate] DATETIME      NULL,
    [Photos]       NUMERIC (18)  NULL,
    [UserId]       NUMERIC (18)  NULL,
    [IsActive]     BIT           NULL,
    [IsFeatured]   BIT           CONSTRAINT [DF_UP_Albums_IsFeatured] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_UP_Albums] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

