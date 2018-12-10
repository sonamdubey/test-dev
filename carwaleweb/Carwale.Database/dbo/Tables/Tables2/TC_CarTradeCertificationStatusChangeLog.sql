CREATE TABLE [dbo].[TC_CarTradeCertificationStatusChangeLog] (
    [TC_CarTradeCertificationStatusChangeLogId] INT      IDENTITY (1, 1) NOT NULL,
    [TC_CarTradeCertificationRequestId]         INT      NOT NULL,
    [TC_CarTradeCertificationStatusId]          INT      NOT NULL,
    [EntryDate]                                 DATETIME NOT NULL,
    [StatusDate]                                DATETIME NULL,
    CONSTRAINT [PK_TC_CTCertStatusLog] PRIMARY KEY CLUSTERED ([TC_CarTradeCertificationStatusChangeLogId] ASC)
);

