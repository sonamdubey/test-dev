CREATE TABLE [dbo].[CRM_FeedbackType] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Feedbacktype] VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_CRM_FeedbackType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

