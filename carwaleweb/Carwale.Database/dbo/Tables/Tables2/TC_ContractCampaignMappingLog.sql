CREATE TABLE [dbo].[TC_ContractCampaignMappingLog] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [ContractId]            INT      NULL,
    [CampaignId]            INT      NULL,
    [DealerId]              BIGINT   NULL,
    [StartDate]             DATETIME NULL,
    [EndDate]               DATETIME NULL,
    [TotalGoal]             INT      NULL,
    [TotalDelivered]        INT      NULL,
    [ContractStatus]        INT      NOT NULL,
    [ContractBehaviour]     SMALLINT NULL,
    [CostPerLead]           INT      NULL,
    [ContractType]          INT      NULL,
    [ReplacementContractId] INT      NULL,
    [ActionTakenBy]         INT      NULL,
    [CreatedOn]             DATETIME NULL,
    [AutoPauseDate]         DATETIME NULL
);

