CREATE TABLE [dbo].[Comments] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [UserName]          VARCHAR (100) NOT NULL,
    [Email]             VARCHAR (150) NOT NULL,
    [Comment]           VARCHAR (500) NOT NULL,
    [CommentCategoryId] NUMERIC (18)  NOT NULL,
    [CategorySourceId]  NUMERIC (18)  NOT NULL,
    [CommentDateTime]   DATETIME      NOT NULL,
    [LoginSource]       VARCHAR (50)  NOT NULL,
    [IsActive]          BIT           CONSTRAINT [DF_Comments_IsActive] DEFAULT ((1)) NOT NULL,
    [ReportAbuse]       BIT           CONSTRAINT [DF_Comments_ReportAbuse] DEFAULT ((0)) NOT NULL,
    [IsApproved]        BIT           CONSTRAINT [DF_Comments_IsApproved] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

