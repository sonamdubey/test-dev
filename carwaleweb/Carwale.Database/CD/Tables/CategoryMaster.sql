CREATE TABLE [CD].[CategoryMaster] (
    [CategoryMasterID] BIGINT              IDENTITY (1, 1) NOT NULL,
    [CategoryName]     VARCHAR (100)       NOT NULL,
    [HierId]           [sys].[hierarchyid] NOT NULL,
    [lvl]              AS                  ([HierId].[GetLevel]()) PERSISTED,
    [NodeCode]         AS                  ([HierId].[ToString]()) PERSISTED,
    [Layout]           TINYINT             NULL,
    [SortOrder]        INT                 NULL,
    CONSTRAINT [PK_CategoryMaster] PRIMARY KEY CLUSTERED ([CategoryMasterID] ASC),
    CONSTRAINT [IX_CategoryMaster_NodeCode] UNIQUE NONCLUSTERED ([NodeCode] ASC)
);

