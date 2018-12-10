CREATE TABLE [dbo].[MailerTypes] (
    [MailerTypesId] INT           IDENTITY (1, 1) NOT NULL,
    [MailerName]    VARCHAR (50)  NOT NULL,
    [LandingURL]    VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_MailerTypes] PRIMARY KEY CLUSTERED ([MailerTypesId] ASC)
);

