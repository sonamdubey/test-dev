CREATE TABLE [dbo].[TC_MailData] (
    [TC_MailDataId] NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Subject]       VARCHAR (500) NOT NULL,
    [MailBody]      VARCHAR (MAX) NOT NULL,
    [CustomerEmail] VARCHAR (50)  NOT NULL,
    [MailDate]      DATETIME      NOT NULL,
    [MailBy]        VARCHAR (50)  NOT NULL,
    [MailFile]      VARCHAR (50)  NOT NULL,
    [TC_PQId]       BIGINT        NOT NULL,
    CONSTRAINT [PK_TC_MailData] PRIMARY KEY CLUSTERED ([TC_MailDataId] ASC)
);

