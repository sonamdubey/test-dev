CREATE TABLE [dbo].[CRM_PESurvey] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CBDId]             NUMERIC (18)  NOT NULL,
    [UserId]            NUMERIC (18)  NOT NULL,
    [CWExperience]      SMALLINT      NULL,
    [CTServive]         SMALLINT      NULL,
    [ConsultantAble]    SMALLINT      NULL,
    [ConsultantAnswer]  SMALLINT      NULL,
    [ConsultantAskedTD] SMALLINT      NULL,
    [Comments]          VARCHAR (500) NULL,
    [FBDate]            DATETIME      CONSTRAINT [DF_CRM_PESurvey_FBDate] DEFAULT (getdate()) NOT NULL,
    [CWRecommend]       SMALLINT      NULL,
    [IsCompleted]       BIT           CONSTRAINT [DF_CRM_PESurvey_IsCompleted] DEFAULT ((0)) NOT NULL,
    [UpdatedOn]         DATETIME      NULL,
    CONSTRAINT [PK_CRM_PESurvey] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'How would you rate your experience with CarWale? 1-Excellent, 2-Very Good, 3-Good, 4-Poor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_PESurvey', @level2type = N'COLUMN', @level2name = N'CWExperience';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1.Yes, It is extremely usefu, 2.It is some what useful, 3.It is not useful, 4.It is a nuisanc and I do not wish to be called again, Do you feel that CarWale’s telephonic service is useful for people who are thinking about buying a car? ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_PESurvey', @level2type = N'COLUMN', @level2name = N'CTServive';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Yes, fully, 2.Somewhat, 3.No, not really,   Was CarWale''s consultant able to fully understand your requirements about your car purchase?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_PESurvey', @level2type = N'COLUMN', @level2name = N'ConsultantAble';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1.Yes, all of them, 2.Some of them, 3.No, not really,  Was the consultant able to answer ALL your questions about the cars you are considering?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_PESurvey', @level2type = N'COLUMN', @level2name = N'ConsultantAnswer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Yes, 2.No,  Did the CarWale consultant asked you for a Test Drive?', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_PESurvey', @level2type = N'COLUMN', @level2name = N'ConsultantAskedTD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Please do let us know if you have any other comments or suggestions', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_PESurvey', @level2type = N'COLUMN', @level2name = N'Comments';

