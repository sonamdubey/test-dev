CREATE TABLE [dbo].[TC_ActionsPlatformTrack] (
    [Id]               INT      IDENTITY (1, 1) NOT NULL,
    [TC_ActionId]      BIGINT   NULL,
    [BranchId]         BIGINT   NULL,
    [ActionPlatformId] TINYINT  NULL,
    [ActionDate]       DATETIME NULL
);

