CREATE TABLE [CD].[GroupItemConfig] (
    [GroupItemID]  INT            IDENTITY (1, 1) NOT NULL,
    [ItemMasterID] BIGINT         NULL,
    [Template]     NVARCHAR (50)  NOT NULL,
    [ItemIDs]      NVARCHAR (50)  NOT NULL,
    [UnitCode]     VARCHAR (50)   NOT NULL,
    [Rules]        NVARCHAR (MAX) NULL,
    CONSTRAINT [FK_GroupItemConfig_ItemMasterID] FOREIGN KEY ([ItemMasterID]) REFERENCES [CD].[ItemMaster] ([ItemMasterId])
);

