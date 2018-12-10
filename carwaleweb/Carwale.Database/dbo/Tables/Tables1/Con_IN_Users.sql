CREATE TABLE [dbo].[Con_IN_Users] (
    [ID]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]              VARCHAR (100) NOT NULL,
    [Email]             VARCHAR (100) NOT NULL,
    [Mobile]            VARCHAR (20)  NULL,
    [Phone]             VARCHAR (20)  NULL,
    [ReceiveNewsletter] BIT           NOT NULL,
    [CreateDate]        DATETIME      NOT NULL,
    CONSTRAINT [PK_Con_IN_Users] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

