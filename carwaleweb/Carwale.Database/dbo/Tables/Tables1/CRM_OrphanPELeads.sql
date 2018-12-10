CREATE TABLE [dbo].[CRM_OrphanPELeads] (
    [Id]      NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CBDId]   NUMERIC (18) NOT NULL,
    [MakeId]  INT          NOT NULL,
    [ModelId] INT          NOT NULL,
    [StateId] INT          NOT NULL,
    [CityId]  INT          NOT NULL,
    CONSTRAINT [PK_CRM_OrphanPELeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

