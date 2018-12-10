CREATE TABLE [dbo].[DCRM_UCDFeedback] (
    [Id]                NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [AnswerId]          NUMERIC (18)   NULL,
    [FBSubmitDate]      DATETIME       NOT NULL,
    [UpdatedOn]         DATETIME       NOT NULL,
    [UpdatedBy]         NUMERIC (18)   NULL,
    [Comments]          VARCHAR (1000) NULL,
    [QuestionId]        INT            NOT NULL,
    [DealerId]          NUMERIC (18)   NOT NULL,
    [FBScheduledDate]   DATETIME       NULL,
    [CustomerId]        NUMERIC (18)   NOT NULL,
    [CustomerCallingId] NUMERIC (18)   NULL,
    CONSTRAINT [PK_CRM_UCDFeedback] PRIMARY KEY CLUSTERED ([Id] ASC)
);

