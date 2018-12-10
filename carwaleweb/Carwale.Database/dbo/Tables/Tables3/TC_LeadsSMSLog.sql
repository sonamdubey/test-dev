CREATE TABLE [dbo].[TC_LeadsSMSLog] (
    [SNO]      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [MobileNo] VARCHAR (50)   NOT NULL,
    [Message]  VARCHAR (1000) NOT NULL,
    [SendedOn] DATETIME       NULL
);

