CREATE TABLE [dbo].[CW_ESSurveyOptions] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [QuestionId]   INT           NOT NULL,
    [OptionNumber] INT           NULL,
    [OptionValue]  VARCHAR (150) NULL,
    [IsActive]     BIT           CONSTRAINT [DF_CW_ESSurveyOptions_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_ESSurveyOptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

