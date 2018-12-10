CREATE TABLE [CRM].[LSScoreLog] (
    [ScoreLogId]    BIGINT       IDENTITY (1, 1) NOT NULL,
    [LeadId]        NUMERIC (18) NOT NULL,
    [PreviousScore] FLOAT (53)   NOT NULL,
    [NewScore]      FLOAT (53)   NOT NULL,
    [SubCategoryId] INT          NOT NULL,
    [CreatedOn]     DATETIME     CONSTRAINT [DF_LSScoreLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_LSScoreLog] PRIMARY KEY CLUSTERED ([ScoreLogId] ASC)
);

