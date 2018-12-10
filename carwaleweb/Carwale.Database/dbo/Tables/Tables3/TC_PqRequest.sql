CREATE TABLE [dbo].[TC_PqRequest] (
    [TC_PQId]              INT           IDENTITY (1, 1) NOT NULL,
    [TC_NewCarInquiriesId] INT           NULL,
    [RequestedDate]        DATETIME      CONSTRAINT [DF_TC_PQ_RequestedDate] DEFAULT (getdate()) NOT NULL,
    [PqStatus]             TINYINT       NULL,
    [Comments]             VARCHAR (MAX) NULL,
    [LastUpdatedDate]      DATETIME      NULL,
    [PqCompletedDate]      DATETIME      NULL,
    [TC_UsersId]           INT           NULL,
    CONSTRAINT [PK_TC_PqId] PRIMARY KEY NONCLUSTERED ([TC_PQId] ASC),
    CONSTRAINT [DF_TC_PQRequest_TC_LeadDisposition] FOREIGN KEY ([PqStatus]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_PQRequest_TC_Users] FOREIGN KEY ([TC_UsersId]) REFERENCES [dbo].[TC_Users] ([Id])
);

