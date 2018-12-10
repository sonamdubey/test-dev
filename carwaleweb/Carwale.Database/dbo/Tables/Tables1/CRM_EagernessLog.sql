CREATE TABLE [dbo].[CRM_EagernessLog] (
    [ID]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [LeadId]        NUMERIC (18) NOT NULL,
    [EagernessId]   NUMERIC (18) NOT NULL,
    [EventBy]       NUMERIC (18) NOT NULL,
    [EntryDateTime] DATETIME     CONSTRAINT [DF_CRM_EagernessLog_EntryDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_EagernessLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

