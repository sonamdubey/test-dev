CREATE TABLE [dbo].[TempDoNotSendMail] (
    [Email]      VARCHAR (100) NULL,
    [CustomerId] NUMERIC (18)  NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_tempdonotsendmail_email]
    ON [dbo].[TempDoNotSendMail]([Email] ASC);

