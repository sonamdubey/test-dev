CREATE TABLE [dbo].[AW_AdClients] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ClientName] VARCHAR (100) NOT NULL,
    [AgencyName] VARCHAR (100) NOT NULL,
    [loginid]    VARCHAR (50)  NOT NULL,
    [passwd]     VARCHAR (50)  NOT NULL,
    [IsActive]   BIT           NOT NULL,
    [LastLogin]  DATETIME      NULL,
    CONSTRAINT [PK_AdClients1] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

