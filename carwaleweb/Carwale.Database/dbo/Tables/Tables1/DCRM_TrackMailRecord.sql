CREATE TABLE [dbo].[DCRM_TrackMailRecord] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]     NUMERIC (18)  NOT NULL,
    [MailSentDate] SMALLDATETIME NOT NULL,
    [Email]        VARCHAR (100) NULL,
    [MobileNo]     VARCHAR (20)  NULL
);

