CREATE TYPE [dbo].[CRM_FeedBackSave] AS TABLE (
    [CBDId]      BIGINT        NULL,
    [AnswerId]   BIGINT        NULL,
    [FBDate]     DATETIME      NULL,
    [UpdatedOn]  DATETIME      NULL,
    [UpdatedBy]  BIGINT        NULL,
    [Comments]   VARCHAR (500) NULL,
    [ItemType]   INT           NULL,
    [QuestionId] BIGINT        NULL);

