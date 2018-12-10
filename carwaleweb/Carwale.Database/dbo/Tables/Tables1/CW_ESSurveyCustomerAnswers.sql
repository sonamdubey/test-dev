CREATE TABLE [dbo].[CW_ESSurveyCustomerAnswers] (
    [Id]                 INT          IDENTITY (1, 1) NOT NULL,
    [CustomerId]         INT          NULL,
    [QuestionId]         VARCHAR (50) NULL,
    [OptionId]           VARCHAR (50) NULL,
    [EntryDate]          DATETIME     CONSTRAINT [DF_ESSurveyCustomerAnswers_EntryDate] DEFAULT (getdate()) NULL,
    [ESSurveyCampaignId] INT          NULL,
    CONSTRAINT [PK_CW_ESSurveyCustomerAnswers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

