CREATE TABLE [dbo].[JobApplication] (
    [JobId]                  NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AppName]                VARCHAR (100) NOT NULL,
    [ContactNo]              VARCHAR (50)  NOT NULL,
    [EmailId]                VARCHAR (100) NOT NULL,
    [ApplyingFor]            VARCHAR (100) NOT NULL,
    [CVFileName]             VARCHAR (100) NULL,
    [FeedBack]               VARCHAR (500) NULL,
    [UserIP]                 VARCHAR (50)  NULL,
    [AppDateTime]            DATETIME      NOT NULL,
    [CityName]               VARCHAR (100) NULL,
    [AlternateContactNumber] VARCHAR (50)  NULL,
    [CoverNote]              VARCHAR (MAX) NULL,
    [LinkedIn]               VARCHAR (100) NULL,
    [Facebook]               VARCHAR (100) NULL,
    [Twitter]                VARCHAR (100) NULL,
    [Blog]                   VARCHAR (100) NULL,
    [Portfolio]              VARCHAR (MAX) NULL,
    [AppliedJobId]           INT           NULL,
    [CVPath]                 VARCHAR (100) NULL,
    CONSTRAINT [PK_JobApplication] PRIMARY KEY CLUSTERED ([JobId] ASC) WITH (FILLFACTOR = 90)
);

