CREATE TABLE [dbo].[OLM_FeedbackData] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CustomerName] VARCHAR (50)   NOT NULL,
    [Email]        VARCHAR (50)   NOT NULL,
    [Mobile]       VARCHAR (15)   NOT NULL,
    [CarModel]     NUMERIC (18)   NULL,
    [CarRegNum]    VARCHAR (15)   NULL,
    [Feedback]     VARCHAR (1000) NULL,
    [ClientIp]     VARCHAR (20)   NULL,
    [EntryDate]    DATETIME       CONSTRAINT [DF_OLM_FeedbackData_EntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_OLM_FeedbackData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

