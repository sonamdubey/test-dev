CREATE TABLE [dbo].[CW_ESSurveyQuestion] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [Question]           VARCHAR (300) NULL,
    [QuestionNumber]     INT           NULL,
    [IsActive]           BIT           CONSTRAINT [DF_CW_ESSurveyQuestion_IsActive] DEFAULT ((1)) NULL,
    [ESSurveyCampaignId] INT           NULL,
    [ImageUrl]           VARCHAR (150) NULL,
    [IsMultiSelect]      BIT           CONSTRAINT [DF_CW_ESSurveyQuestion_IsMultiSelect] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ESSurveyQuestion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

