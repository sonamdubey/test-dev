CREATE TABLE [dbo].[Forum_ReportAbuse] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ThreadId]   NUMERIC (18)  NULL,
    [PostId]     NUMERIC (18)  NULL,
    [CustomerId] NUMERIC (18)  NULL,
    [Comment]    VARCHAR (100) NULL,
    [Status]     SMALLINT      CONSTRAINT [DF_Forum_ReportAbuse_Status] DEFAULT ((0)) NULL,
    [CreateDate] DATETIME      CONSTRAINT [DF_Forum_ReportAbuse_CreateDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Forum_ReportAbuse] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

