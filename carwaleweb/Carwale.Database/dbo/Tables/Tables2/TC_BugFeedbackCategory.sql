CREATE TABLE [dbo].[TC_BugFeedbackCategory] (
    [TC_BugFeedbackCategory_Id] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Category]                  VARCHAR (100) NULL,
    [IsActive]                  BIT           CONSTRAINT [DF_TC_BugFeedbackCategory_IsActive] DEFAULT ((1)) NULL
);

