CREATE TABLE [dbo].[RVN_DealerPackageFeaturesLogs] (
    [ID]                     INT      IDENTITY (1, 1) NOT NULL,
    [DealerPackageFeatureId] INT      NOT NULL,
    [OldMakeId]              INT      NULL,
    [NewMakeId]              INT      NULL,
    [OldModelId]             INT      NULL,
    [NewModelId]             INT      NULL,
    [OldPackageStartDate]    DATETIME NULL,
    [NewPackageStartDate]    DATETIME NULL,
    [OldPackageEndDate]      DATETIME NULL,
    [NewPackageEndDate]      DATETIME NULL,
    [OldLeadCount]           INT      NULL,
    [NewLeadCount]           INT      NULL,
    [OldPackageStatus]       INT      NULL,
    [NewPackageStatus]       INT      NULL,
    [UpdatedBy]              INT      NULL,
    [UpdatedOn]              DATETIME NULL
);

