CREATE TABLE [dbo].[SMSModule] (
    [ID]             NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MobileNo]       VARCHAR (50)  NULL,
    [Keyword]        VARCHAR (50)  NULL,
    [SMSFor]         VARCHAR (100) NULL,
    [SMSText]        VARCHAR (500) NULL,
    [SMSDateTime]    VARCHAR (50)  NULL,
    [Status]         VARCHAR (50)  NULL,
    [SubmitDateTime] DATETIME      NULL,
    [IsProcessed]    BIT           CONSTRAINT [DF_SMSModule_IsProcessed] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_SMSModule] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IXSMSModule_IsProcessed]
    ON [dbo].[SMSModule]([IsProcessed] ASC)
    INCLUDE([ID], [MobileNo], [SMSText], [SubmitDateTime]);

