CREATE TABLE [dbo].[AbSure_InspectionDataLog] (
    [ID]                  NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId] NUMERIC (18)  NOT NULL,
    [AbSure_QuestionsId]  NUMERIC (18)  NOT NULL,
    [AbSure_AnswerValue]  SMALLINT      NOT NULL,
    [AnswerDate]          DATETIME      NOT NULL,
    [AnswerComments]      VARCHAR (500) NULL,
    [Timestamp]           DATETIME      NULL,
    CONSTRAINT [PK_AbSure_InspectionDataLog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

