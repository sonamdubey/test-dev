CREATE TABLE [dbo].[CRM_ADM_PEConsultantRules] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ConsultantId]  INT           NOT NULL,
    [MakeId]        INT           NOT NULL,
    [ModelId]       INT           NOT NULL,
    [StateId]       INT           NOT NULL,
    [CityId]        INT           NOT NULL,
    [CreatedOn]     DATETIME      NOT NULL,
    [UpdatedOn]     DATETIME      NOT NULL,
    [UpdatedBy]     NVARCHAR (50) NOT NULL,
    [IsActiveLogin] BIT           CONSTRAINT [DF_CRM_ADM_PEConsultantRules_IsActiveLogin] DEFAULT ((0)) NOT NULL,
    [LastLeadTime]  DATETIME      NULL,
    CONSTRAINT [PK_CRM_ADM_PEConsultantRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);

