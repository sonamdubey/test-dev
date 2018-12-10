CREATE TABLE [dbo].[CRM_DealerFollowUp] (
    [Id]               NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [CustId]           NUMERIC (18)   NOT NULL,
    [LeadId]           NUMERIC (18)   NOT NULL,
    [ProductStatus]    NUMERIC (18)   NOT NULL,
    [Eagerness]        NUMERIC (18)   NULL,
    [CreatedOn]        DATETIME       NOT NULL,
    [NextCallDate]     DATETIME       NULL,
    [LastCallDate]     DATETIME       NULL,
    [LastComment]      VARCHAR (1250) NULL,
    [Comment]          VARCHAR (8000) NULL,
    [UpdatedBy]        NUMERIC (18)   NOT NULL,
    [DealerId]         NUMERIC (18)   NOT NULL,
    [PanelType]        BIT            NOT NULL,
    [SubDispositionId] INT            NULL,
    CONSTRAINT [PK_FollowUp_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_DealerFollowUp__CustId__LeadId__DealerId__Id]
    ON [dbo].[CRM_DealerFollowUp]([CustId] ASC, [LeadId] ASC, [DealerId] ASC, [Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_DealerFollowUp_ProductStatus]
    ON [dbo].[CRM_DealerFollowUp]([ProductStatus] ASC, [Eagerness] ASC, [PanelType] ASC, [NextCallDate] ASC, [DealerId] ASC)
    INCLUDE([LeadId]);

