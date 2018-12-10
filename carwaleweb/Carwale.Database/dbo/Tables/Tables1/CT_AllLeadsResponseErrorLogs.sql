CREATE TABLE [dbo].[CT_AllLeadsResponseErrorLogs] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [CWLeadId]         VARCHAR (100) NULL,
    [ErrorDescription] VARCHAR (200) NULL,
    [EntryDate]        DATETIME      DEFAULT (getdate()) NOT NULL
);

