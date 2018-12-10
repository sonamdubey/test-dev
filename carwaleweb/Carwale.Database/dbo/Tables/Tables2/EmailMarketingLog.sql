CREATE TABLE [dbo].[EmailMarketingLog] (
    [EmailMarketingLog_Id] INT           IDENTITY (1, 1) NOT NULL,
    [DownloadedBy]         VARCHAR (100) NULL,
    [Criteria]             VARCHAR (100) NULL,
    [Number]               BIGINT        NULL,
    [DownloadDate]         DATETIME      CONSTRAINT [DF_EmailMarketingLog_DownloadDate] DEFAULT (getdate()) NULL
);

