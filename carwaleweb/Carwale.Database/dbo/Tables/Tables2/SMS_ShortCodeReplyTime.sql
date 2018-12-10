CREATE TABLE [dbo].[SMS_ShortCodeReplyTime] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [SMS_ShortCodeId] INT           NOT NULL,
    [SMSText]         VARCHAR (250) NOT NULL,
    [StartTime]       TIME (7)      NOT NULL,
    [EndTime]         TIME (7)      NOT NULL,
    CONSTRAINT [PK_SMS_ShortCodeReplyTime] PRIMARY KEY CLUSTERED ([Id] ASC)
);

