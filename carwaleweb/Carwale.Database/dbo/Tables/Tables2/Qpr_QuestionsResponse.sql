CREATE TABLE [dbo].[Qpr_QuestionsResponse] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Qpr_RatingDataId]     INT            NULL,
    [Question_Id]          INT            NULL,
    [Comments]             VARCHAR (1000) NULL,
    [Qpr_ResponseValuesId] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

