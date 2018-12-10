CREATE TYPE [dbo].[TC_LeadsSMSDataLog] AS TABLE (
    [Mobile]   VARCHAR (15)   NULL,
    [Message]  VARCHAR (1000) NULL,
    [SendedOn] DATETIME       NULL);

