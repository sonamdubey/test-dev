CREATE TABLE [dbo].[TC_BugFeedback] (
    [TC_Bug_Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [BranchId]              NUMERIC (18)  NOT NULL,
    [CustomerFeedback]      VARCHAR (400) NOT NULL,
    [FilePath]              VARCHAR (100) NULL,
    [HostUrl]               VARCHAR (50)  CONSTRAINT [DF__TC_BugFeedback__HostU__Image] DEFAULT ('img.carwale.com') NULL,
    [UserAgent]             VARCHAR (50)  NULL,
    [ClientOS]              VARCHAR (50)  NULL,
    [EntryDate]             DATE          CONSTRAINT [DF_TC_BugFeedback_EntryDate] DEFAULT (getdate()) NULL,
    [IsActive]              BIT           CONSTRAINT [DF_TC_BugFeedback_IsActive] DEFAULT ((1)) NULL,
    [IsReplicated]          BIT           DEFAULT ((0)) NULL,
    [TC_FeedbackCategoryId] BIGINT        NULL,
    [BugScreenShotStatusId] INT           NULL,
    [OriginalImgPath]       VARCHAR (250) NULL,
    CONSTRAINT [PK_TC_BugFeedback] PRIMARY KEY CLUSTERED ([TC_Bug_Id] ASC)
);

