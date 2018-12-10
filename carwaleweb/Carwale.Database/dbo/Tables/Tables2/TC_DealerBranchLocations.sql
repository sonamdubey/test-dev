CREATE TABLE [dbo].[TC_DealerBranchLocations] (
    [Id]       INT    IDENTITY (1, 1) NOT NULL,
    [BranchId] BIGINT NULL,
    [IsActive] BIT    NULL,
    [AreaId]   INT    NULL
);

