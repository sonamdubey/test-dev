CREATE TABLE [dbo].[FeedbackQuestions] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Question]      VARCHAR (500) NULL,
    [FeedbackType]  INT           NULL,
    [Alias]         VARCHAR (50)  NULL,
    [QuestionOrder] INT           NULL,
    [DataType]      SMALLINT      NULL,
    CONSTRAINT [PK_FeedbackQuestions] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-SaleInquiry', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FeedbackQuestions', @level2type = N'COLUMN', @level2name = N'FeedbackType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Numeric, 2-Text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FeedbackQuestions', @level2type = N'COLUMN', @level2name = N'DataType';

