CREATE TABLE [dbo].[AbSure_InspectionData] (
    [ID]                  NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId] NUMERIC (18)  NOT NULL,
    [AbSure_QuestionsId]  NUMERIC (18)  NOT NULL,
    [AbSure_AnswerValue]  SMALLINT      NOT NULL,
    [AnswerDate]          DATETIME      CONSTRAINT [DF_AbSure_InspectionData_AnswerDate] DEFAULT (getdate()) NOT NULL,
    [AnswerComments]      VARCHAR (500) NULL,
    [Timestamp]           DATETIME      NULL,
    CONSTRAINT [PK_AbSure_InspectionData] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AbSure_InspectionData_AbSure_CarDetailsId]
    ON [dbo].[AbSure_InspectionData]([AbSure_CarDetailsId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_AbSure_InspectionData_QuestionsId]
    ON [dbo].[AbSure_InspectionData]([AbSure_QuestionsId] ASC, [AbSure_CarDetailsId] ASC);

