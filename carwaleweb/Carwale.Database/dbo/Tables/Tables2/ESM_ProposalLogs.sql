CREATE TABLE [dbo].[ESM_ProposalLogs] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [ESM_ProposalId] INT          NULL,
    [Title]          VARCHAR (50) NULL,
    [ClientId]       INT          NULL,
    [BrandId]        INT          NULL,
    [AgencyId]       INT          NULL,
    [CampaignTypeId] INT          NULL,
    [UpdatedBy]      NUMERIC (18) NULL,
    [UpdatedOn]      DATETIME     NULL,
    [DeletedBy]      NUMERIC (18) NULL,
    [DeletedOn]      DATETIME     NULL,
    [Probability]    INT          NULL,
    CONSTRAINT [PK_ESM_ProposalLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

