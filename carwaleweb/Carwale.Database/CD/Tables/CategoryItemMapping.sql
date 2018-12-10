CREATE TABLE [CD].[CategoryItemMapping] (
    [CategoryItemMappingId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [ItemMasterId]          BIGINT          NULL,
    [NodeCode]              NVARCHAR (4000) NULL,
    CONSTRAINT [PK_CategoryItemMapping] PRIMARY KEY CLUSTERED ([CategoryItemMappingId] ASC),
    CONSTRAINT [FK_CategoryItemMapping_ItemMaster] FOREIGN KEY ([ItemMasterId]) REFERENCES [CD].[ItemMaster] ([ItemMasterId]),
    CONSTRAINT [FK_CategoryItemMapping_NodeCode] FOREIGN KEY ([NodeCode]) REFERENCES [CD].[CategoryMaster] ([NodeCode])
);

