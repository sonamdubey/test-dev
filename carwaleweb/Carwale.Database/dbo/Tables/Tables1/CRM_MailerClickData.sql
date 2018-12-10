CREATE TABLE [dbo].[CRM_MailerClickData] (
    [CBDId]       NUMERIC (18) NOT NULL,
    [RequestType] INT          NOT NULL,
    [ClickDate]   DATETIME     NOT NULL,
    [CallId]      NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_MailerClickData] PRIMARY KEY CLUSTERED ([CBDId] ASC, [RequestType] ASC)
);

