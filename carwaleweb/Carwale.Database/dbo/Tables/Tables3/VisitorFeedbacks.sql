CREATE TABLE [dbo].[VisitorFeedbacks] (
    [Id]               NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]             VARCHAR (50)   NULL,
    [Email]            VARCHAR (50)   NULL,
    [Feedback]         VARCHAR (2000) NULL,
    [UserIP]           VARCHAR (20)   NULL,
    [FeedbackDateTime] DATETIME       NULL,
    [FBSource]         NVARCHAR (200) NULL,
    [FeedBackRating]   TINYINT        NULL,
    [CarInfo]          NVARCHAR (200) NULL,
    [FBSourceID]       TINYINT        NULL,
    [ReportID]         BIGINT         NULL,
    CONSTRAINT [PK_VisitorFeedbacks] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

