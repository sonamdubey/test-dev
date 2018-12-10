CREATE TABLE [dbo].[Con_MaintenancePlanType] (
    [Id]       SMALLINT     NOT NULL,
    [PlanType] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MaintenancePlanType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Inspect, 2-Clean, 3-Replace', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Con_MaintenancePlanType', @level2type = N'COLUMN', @level2name = N'Id';

