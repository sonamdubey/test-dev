CREATE TYPE [dbo].[CRM_FeedBackPushLead] AS TABLE (
    [id]          INT      NULL,
    [LeadId]      BIGINT   NULL,
    [CallType]    BIGINT   NULL,
    [IsTeam]      BIT      NULL,
    [CallerId]    BIGINT   NULL,
    [ScheduledOn] DATETIME NULL,
    [CreatedOn]   DATETIME NULL,
    [DealerId]    BIGINT   NULL,
    [CbdId]       BIGINT   NULL);

