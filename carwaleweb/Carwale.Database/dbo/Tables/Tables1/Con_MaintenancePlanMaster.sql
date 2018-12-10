CREATE TABLE [dbo].[Con_MaintenancePlanMaster] (
    [Id]       NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PlanName] VARCHAR (100) NULL,
    [MakeId]   NUMERIC (18)  NULL,
    CONSTRAINT [PK_Con_MaintenancePlanMaster] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

