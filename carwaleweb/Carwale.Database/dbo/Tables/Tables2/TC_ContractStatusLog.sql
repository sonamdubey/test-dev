CREATE TABLE [dbo].[TC_ContractStatusLog] (
    [Id]                           INT      IDENTITY (1, 1) NOT NULL,
    [TC_ContractCampaignMappingId] INT      NULL,
    [PreContractStatus]            SMALLINT NULL,
    [CurrentContractStatus]        SMALLINT NULL,
    [ActionTakenBy]                INT      NULL,
    [ActionTakenOn]                DATETIME NULL,
    CONSTRAINT [PK_TC_ContractStatusLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

