CREATE TYPE [dbo].[CRM_PushLeads] AS TABLE (
    [CustId]      BIGINT   NULL,
    [PushedDate]  DATETIME NULL,
    [UpdatedBy]   BIGINT   NULL,
    [LeadId]      BIGINT   NULL,
    [InQuiryDate] DATETIME NULL);

