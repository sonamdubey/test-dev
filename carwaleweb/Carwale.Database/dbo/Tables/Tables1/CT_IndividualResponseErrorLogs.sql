CREATE TABLE [dbo].[CT_IndividualResponseErrorLogs] (
    [CWLeadId]         INT           NULL,
    [ErrorDescription] VARCHAR (100) NULL,
    [EntryDate]        DATETIME      DEFAULT (getdate()) NULL
);

