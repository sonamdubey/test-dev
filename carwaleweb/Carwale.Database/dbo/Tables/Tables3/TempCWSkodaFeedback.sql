CREATE TABLE [dbo].[TempCWSkodaFeedback] (
    [Id]                    NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]            VARCHAR (200) NULL,
    [CWBuyingDecision]      VARCHAR (50)  NULL,
    [SkodaBuyingExperience] VARCHAR (50)  NULL,
    [CWRecommend]           VARCHAR (50)  NULL,
    [FeedbackDate]          DATETIME      NULL,
    CONSTRAINT [PK_TempCWSkodaFeedback] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

