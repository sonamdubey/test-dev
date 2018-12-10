CREATE TABLE [dbo].[CRM_ADM_QueueRuleParams] (
    [ID]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [QueueId]    NUMERIC (18) NOT NULL,
    [MakeId]     NUMERIC (18) NOT NULL,
    [CityId]     NUMERIC (18) NOT NULL,
    [CreatedOn]  DATETIME     CONSTRAINT [DF_CRM_ADM_QueueRuleParams_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]  NUMERIC (18) NOT NULL,
    [UpdatedOn]  DATETIME     NOT NULL,
    [ModelId]    NUMERIC (18) CONSTRAINT [DF_CRM_ADM_QueueRuleParams_ModelId] DEFAULT ((-1)) NOT NULL,
    [SourceId]   INT          CONSTRAINT [DF_CRM_ADM_QueueRuleParams_SourceId] DEFAULT ((-1)) NOT NULL,
    [IsResearch] BIT          CONSTRAINT [DF_CRM_ADM_QueueRuleParams_IsResearch] DEFAULT ((0)) NOT NULL,
    [IsActive]   BIT          CONSTRAINT [DF_CRM_ADM_QueueRuleParams_IsActive] DEFAULT ((1)) NULL,
    [DealerId]   NUMERIC (18) NULL,
    CONSTRAINT [PK_ADM_QueueRuleParams] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

