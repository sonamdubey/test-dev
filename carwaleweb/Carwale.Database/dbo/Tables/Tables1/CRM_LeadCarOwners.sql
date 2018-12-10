CREATE TABLE [dbo].[CRM_LeadCarOwners] (
    [Id]                NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [LeadId]            NUMERIC (18) NOT NULL,
    [CBDId]             NUMERIC (18) NOT NULL,
    [CarConsultant]     NUMERIC (18) NULL,
    [CustCoordinator]   NUMERIC (18) NULL,
    [DealerCoordinator] NUMERIC (18) NULL,
    [ConsultantDate]    DATETIME     NULL,
    [CCoordinatorDate]  DATETIME     NULL,
    [DCoordinatorDate]  DATETIME     NULL,
    [UpdatedOn]         DATETIME     NOT NULL,
    [UpdatedBy]         NUMERIC (18) NOT NULL,
    [CreatedOn]         DATETIME     CONSTRAINT [DF_CRM_LeadCarOwners_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_LeadCarOwners] PRIMARY KEY CLUSTERED ([Id] ASC)
);

