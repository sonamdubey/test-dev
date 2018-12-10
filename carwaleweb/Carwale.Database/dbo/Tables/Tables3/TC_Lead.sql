CREATE TABLE [dbo].[TC_Lead] (
    [TC_LeadId]               BIGINT       IDENTITY (1, 1) NOT NULL,
    [BranchId]                NUMERIC (18) NULL,
    [TC_CustomerId]           BIGINT       NULL,
    [TC_InquirySourceId]      INT          NULL,
    [TC_LeadStageId]          TINYINT      NULL,
    [LeadVerificationDate]    DATETIME     NULL,
    [LeadVerifiedBy]          INT          NULL,
    [LeadCreationDate]        DATETIME     NULL,
    [LeadClosedDate]          DATETIME     NULL,
    [AppointmentDate]         DATETIME     NULL,
    [LeadType]                TINYINT      NULL,
    [TC_LeadDispositionId]    TINYINT      NULL,
    [TC_CampaignSchedulingId] INT          NULL,
    [TC_BusinessTypeId]       TINYINT      NULL,
    CONSTRAINT [PK_TC_LeadId] PRIMARY KEY NONCLUSTERED ([TC_LeadId] ASC),
    CONSTRAINT [DF_TC_Lead_Dealers] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Dealers] ([ID]),
    CONSTRAINT [DF_TC_Lead_TC_LeadDisposition] FOREIGN KEY ([TC_LeadDispositionId]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_Lead_TC_LeadInquiryType] FOREIGN KEY ([LeadType]) REFERENCES [dbo].[TC_LeadInquiryType] ([TC_LeadInquiryTypeId]),
    CONSTRAINT [DF_TC_Lead_TC_LeadStage] FOREIGN KEY ([TC_LeadStageId]) REFERENCES [dbo].[TC_LeadStage] ([TC_LeadStageId]),
    CONSTRAINT [DF_TC_Lead_TC_users] FOREIGN KEY ([LeadVerifiedBy]) REFERENCES [dbo].[TC_Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Lead_BranchId]
    ON [dbo].[TC_Lead]([BranchId] ASC)
    INCLUDE([TC_LeadId], [TC_LeadStageId], [TC_LeadDispositionId]);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Lead_LeadCreationDate]
    ON [dbo].[TC_Lead]([LeadCreationDate] DESC);


GO
CREATE NONCLUSTERED INDEX [ IX_TC_CustomerId_TC_LeadStageId]
    ON [dbo].[TC_Lead]([TC_CustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Lead_leadverifiedby]
    ON [dbo].[TC_Lead]([LeadVerifiedBy] ASC);

