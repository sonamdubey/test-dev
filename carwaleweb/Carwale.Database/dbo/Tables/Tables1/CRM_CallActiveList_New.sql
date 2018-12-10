CREATE TABLE [dbo].[CRM_CallActiveList_New] (
    [CallId]      NUMERIC (18)  NOT NULL,
    [LeadId]      NUMERIC (18)  NOT NULL,
    [CallType]    INT           NOT NULL,
    [IsTeam]      BIT           NOT NULL,
    [CallerId]    NUMERIC (18)  NOT NULL,
    [ScheduledOn] DATETIME      NOT NULL,
    [Subject]     VARCHAR (500) NULL,
    [AlertId]     INT           NULL,
    [DealerId]    NUMERIC (18)  NULL,
    CONSTRAINT [PK_CRM_ActiveCalls1] PRIMARY KEY CLUSTERED ([CallId] ASC) WITH (FILLFACTOR = 90)
);

