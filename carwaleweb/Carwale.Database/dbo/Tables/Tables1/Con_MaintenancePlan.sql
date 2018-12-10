CREATE TABLE [dbo].[Con_MaintenancePlan] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PlanId]      NUMERIC (18) NULL,
    [PartId]      NUMERIC (18) NULL,
    [Type]        SMALLINT     NULL,
    [IndDistance] NUMERIC (18) NULL,
    [IndTime]     NUMERIC (18) NULL,
    [EntryDate]   DATETIME     NULL,
    CONSTRAINT [PK_Con_MaintenancePlan] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Inspect, 2-Clean, 3-Replace', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Con_MaintenancePlan', @level2type = N'COLUMN', @level2name = N'Type';

