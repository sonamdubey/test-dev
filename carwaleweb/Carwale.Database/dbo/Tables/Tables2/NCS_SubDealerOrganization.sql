CREATE TABLE [dbo].[NCS_SubDealerOrganization] (
    [PKId] BIGINT       IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [OId]  NUMERIC (18) NOT NULL,
    [DId]  NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_NCS_SubDealerOrganization_1] PRIMARY KEY CLUSTERED ([PKId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_NCS_SubDealerOrganization_DId]
    ON [dbo].[NCS_SubDealerOrganization]([DId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NCS_SubDealerOrganization_OId]
    ON [dbo].[NCS_SubDealerOrganization]([OId] ASC);

