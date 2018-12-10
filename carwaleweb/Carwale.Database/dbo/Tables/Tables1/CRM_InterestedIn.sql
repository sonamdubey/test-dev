CREATE TABLE [dbo].[CRM_InterestedIn] (
    [ID]                 NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]             NUMERIC (18)   NOT NULL,
    [ProductTypeId]      SMALLINT       NOT NULL,
    [ProductStatusId]    INT            NOT NULL,
    [ClosingProbability] INT            NOT NULL,
    [ClosingDate]        DATETIME       NULL,
    [CreatedOn]          DATETIME       NOT NULL,
    [UpdatedOn]          DATETIME       NOT NULL,
    [UpdatedBy]          NUMERIC (18)   NOT NULL,
    [ClosingComments]    VARCHAR (1000) NULL,
    [IsMultipleBooking]  BIT            NULL,
    [BookingCount]       SMALLINT       NULL,
    CONSTRAINT [PK_CNS_InterestedIn] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IDX_LeadId_ProductTypeid]
    ON [dbo].[CRM_InterestedIn]([LeadId] ASC, [ProductTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_InterestedIn_IX_CRM_InterestedIn_ProductTypeId]
    ON [dbo].[CRM_InterestedIn]([ProductTypeId] ASC)
    INCLUDE([LeadId], [ProductStatusId], [ClosingProbability]);

