CREATE TABLE [dbo].[Admin_Feedback] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [Link]         VARCHAR (50)   NOT NULL,
    [Section]      VARCHAR (50)   NOT NULL,
    [Department]   VARCHAR (50)   NOT NULL,
    [Feedback]     VARCHAR (1000) NOT NULL,
    [FeedbackBy]   NUMERIC (18)   NOT NULL,
    [FeedbackDate] DATE           NOT NULL,
    CONSTRAINT [PK_Admin_Feedback] PRIMARY KEY CLUSTERED ([Id] ASC)
);

