CREATE TABLE [dbo].[PQDealerFailedLeads] (
    [PqDealerLeadId] INT     NULL,
    [Reason]         TINYINT NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_PQDealerFailedLeads_PqDealerLeadId]
    ON [dbo].[PQDealerFailedLeads]([PqDealerLeadId] ASC)
    INCLUDE([Reason]);

