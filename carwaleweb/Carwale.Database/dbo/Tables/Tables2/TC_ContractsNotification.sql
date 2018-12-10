CREATE TABLE [dbo].[TC_ContractsNotification] (
    [Id]                           INT IDENTITY (1, 1) NOT NULL,
    [TC_ContractCampaignMappingId] INT NULL,
    [IsEmailSent]                  BIT NULL,
    [IsRenewalRequested]           BIT NULL,
    CONSTRAINT [PK_TC_ContractsNotification] PRIMARY KEY CLUSTERED ([Id] ASC)
);

