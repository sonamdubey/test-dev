CREATE TYPE [dbo].[DCRM_UCDFeedBackSave] AS TABLE (
    [CustomerId]        BIGINT         NULL,
    [AnswerId]          BIGINT         NULL,
    [FBSubmitDate]      DATETIME       NULL,
    [UpdatedOn]         DATETIME       NULL,
    [UpdatedBy]         BIGINT         NULL,
    [Comments]          VARCHAR (1000) NULL,
    [QuestionId]        BIGINT         NULL,
    [DealerId]          BIGINT         NULL,
    [FBScheduledDate]   DATETIME       NULL,
    [CustomerCallingId] BIGINT         NULL);

