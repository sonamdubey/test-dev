CREATE TABLE [dbo].[AwardSMSModule] (
    [ID]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MobileNo]       VARCHAR (50)  NULL,
    [Keyword]        VARCHAR (50)  NULL,
    [SMSText]        VARCHAR (500) NULL,
    [SMSDateTime]    VARCHAR (50)  NULL,
    [SubmitDateTime] DATETIME      NULL,
    CONSTRAINT [PK_AwardSMSModule] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

