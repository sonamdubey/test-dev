CREATE TABLE [dbo].[CarInsuranceLeadSentLogs] (
    [ClientId]       INT      NULL,
    [LastLeadIdSent] INT      NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_CarInsuranceLeadSentLogs_CreatedOn] DEFAULT (getdate()) NULL
);

