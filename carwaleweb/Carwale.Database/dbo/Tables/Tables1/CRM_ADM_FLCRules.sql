CREATE TABLE [dbo].[CRM_ADM_FLCRules] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [GroupId]   INT          NOT NULL,
    [MakeId]    INT          NOT NULL,
    [ModelId]   INT          NOT NULL,
    [CityId]    NUMERIC (18) NOT NULL,
    [SourceId]  INT          NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_CRM_ADM_FLCRules_IsActive] DEFAULT ((1)) NOT NULL,
    [Rank]      SMALLINT     CONSTRAINT [DF_CRM_ADM_FLCRules_Rank] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME     CONSTRAINT [DF_CRM_ADM_FLCRules_UpdatedOn] DEFAULT (getdate()) NULL,
    [UpdatedBy] NUMERIC (18) NULL,
    CONSTRAINT [PK_CRM_ADM_FLCRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);

