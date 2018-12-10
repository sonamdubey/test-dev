CREATE TABLE [dbo].[Con_MaintenancePlanVersions] (
    [CPV_Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MaintenancePlanId] NUMERIC (18) NULL,
    [VersionId]         NUMERIC (18) NULL,
    [LabourCost]        NUMERIC (18) NULL,
    [PartCost]          NUMERIC (18) NULL,
    CONSTRAINT [PK_Con_MaintenancePlanVersions] PRIMARY KEY CLUSTERED ([CPV_Id] ASC) WITH (FILLFACTOR = 90)
);

