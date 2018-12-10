CREATE TABLE [dbo].[SMS_ShortCodes] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [Shortcode]     VARCHAR (50)  NOT NULL,
    [Mailto]        VARCHAR (50)  NULL,
    [MailName]      VARCHAR (50)  NULL,
    [MailSubject]   VARCHAR (100) NULL,
    [ReturnMessage] VARCHAR (150) NOT NULL,
    [StatusMessage] VARCHAR (50)  NOT NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_SMS_ShortCodes_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [MailerGroup]   INT           NULL,
    [IsPushCRM]     BIT           CONSTRAINT [DF_SMS_ShortCodes_IsPushCRM] DEFAULT ((1)) NOT NULL,
    [SourceId]      INT           NULL,
    [CRMVersionId]  INT           NULL,
    [IsTimeBased]   BIT           CONSTRAINT [DF_SMS_ShortCodes_IsTimeBased] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SMS_ShortCodes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

