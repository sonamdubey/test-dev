CREATE TABLE [dbo].[CRM_ADM_QueueRuleParamsLog] (
    [ID]         NUMERIC (18) NOT NULL,
    [QueueId]    NUMERIC (18) NOT NULL,
    [MakeId]     NUMERIC (18) NOT NULL,
    [ModelId]    NUMERIC (18) NOT NULL,
    [CityId]     NUMERIC (18) NOT NULL,
    [CreatedOn]  DATETIME     NOT NULL,
    [SourceId]   INT          NULL,
    [IsResearch] BIT          NULL,
    [DealerId]   NUMERIC (18) NULL,
    [DeletedBy]  NUMERIC (18) NULL,
    [DeletedOn]  DATETIME     NULL
);

