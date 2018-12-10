CREATE TABLE [DST].[DrishtiNotProcessedCalls] (
    [DrishtiNotProcessedCallsId] INT          IDENTITY (1, 1) NOT NULL,
    [CallId]                     VARCHAR (50) NULL,
    [PhoneNo]                    VARCHAR (20) NULL,
    [CreatedOn]                  DATETIME     CONSTRAINT [DF_DrishtiNotProcessedCalls_CreatedOn] DEFAULT (getdate()) NULL,
    [IsActionTaken]              BIT          CONSTRAINT [DF_DrishtiNotProcessedCalls_IsActionTaken] DEFAULT ((0)) NULL,
    [ActionTakenBy]              INT          NULL,
    [ActionTakenOn]              DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([DrishtiNotProcessedCallsId] ASC)
);

