CREATE TABLE [dbo].[AdClickLog] (
    [ID]            NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AdId]          NUMERIC (18)   NULL,
    [AdName]        VARCHAR (200)  NULL,
    [ReferringPage] VARCHAR (1000) NULL,
    [CustomerId]    NUMERIC (18)   CONSTRAINT [DF_AdClickLog_CustomerId] DEFAULT ((-1)) NULL,
    [IPAddress]     VARCHAR (100)  NULL,
    [ClickDateTime] DATETIME       NOT NULL,
    [ReferredTo]    VARCHAR (1000) NULL,
    CONSTRAINT [PK_AdClickLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

