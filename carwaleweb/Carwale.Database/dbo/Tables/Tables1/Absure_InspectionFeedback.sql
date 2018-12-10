CREATE TABLE [dbo].[Absure_InspectionFeedback] (
    [Absure_InspectionFeedbackId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Comments]                    VARCHAR (500) NULL,
    [BranchId]                    BIGINT        NOT NULL,
    [AbSure_CarDetailsId]         NUMERIC (18)  NOT NULL,
    [TC_UserId]                   INT           NOT NULL,
    [SurveyorId]                  BIGINT        NOT NULL,
    [EntryDate]                   DATETIME      NOT NULL,
    CONSTRAINT [PK_Absure_InspectionFeedback] PRIMARY KEY CLUSTERED ([Absure_InspectionFeedbackId] ASC)
);

