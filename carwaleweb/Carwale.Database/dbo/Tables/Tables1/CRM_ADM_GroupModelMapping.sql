CREATE TABLE [dbo].[CRM_ADM_GroupModelMapping] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [GroupType] INT          NULL,
    [ModelId]   NUMERIC (18) NULL,
    [CreatedBy] NUMERIC (18) NULL,
    [CreatedOn] DATETIME     NULL,
    [CityId]    INT          NULL,
    CONSTRAINT [PK_CRM_ADM_GroupModelMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

