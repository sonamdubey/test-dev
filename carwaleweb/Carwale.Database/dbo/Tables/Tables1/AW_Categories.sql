CREATE TABLE [dbo].[AW_Categories] (
    [ID]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryName] VARCHAR (100) NULL,
    [Description]  VARCHAR (500) NULL,
    [IsActive]     BIT           NULL,
    [AwardYear]    INT           NULL,
    [ImageName]    VARCHAR (50)  NULL,
    [Position]     SMALLINT      NULL,
    [IsReplicated] BIT           DEFAULT ((1)) NULL,
    [HostURL]      VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_AW_Categories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

