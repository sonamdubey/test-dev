CREATE TABLE [dbo].[TC_BugFeedback_Bkp15082015] (
    [TC_Bug_Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [BranchId]              NUMERIC (18)  NOT NULL,
    [CustomerFeedback]      VARCHAR (400) NOT NULL,
    [FilePath]              VARCHAR (100) NULL,
    [HostUrl]               VARCHAR (50)  NULL,
    [UserAgent]             VARCHAR (50)  NULL,
    [ClientOS]              VARCHAR (50)  NULL,
    [EntryDate]             DATE          NULL,
    [IsActive]              BIT           NULL,
    [IsReplicated]          BIT           NULL,
    [TC_FeedbackCategoryId] BIGINT        NULL,
    [BugScreenShotStatusId] INT           NULL,
    [OriginalImgPath]       VARCHAR (250) NULL
);

