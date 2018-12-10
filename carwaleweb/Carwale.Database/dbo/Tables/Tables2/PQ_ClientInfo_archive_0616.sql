CREATE TABLE [dbo].[PQ_ClientInfo_archive_0616] (
    [PQId]              BIGINT        NOT NULL,
    [AspNetSessionId]   VARCHAR (100) NULL,
    [ClientIP]          VARCHAR (50)  NULL,
    [EntryDate]         DATETIME      NOT NULL,
    [CWCookieValue]     VARCHAR (100) NULL,
    [LeadScore]         FLOAT (53)    NULL,
    [LeadScoreVersion]  SMALLINT      NULL,
    [ls_email_flag]     BIT           NULL,
    [ls_mobile_flag]    BIT           NULL,
    [ls_name_flag]      BIT           NULL,
    [TopLevelLeadScore] FLOAT (53)    NULL
);

