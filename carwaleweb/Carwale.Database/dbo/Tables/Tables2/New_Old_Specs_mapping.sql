CREATE TABLE [dbo].[New_Old_Specs_mapping] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ItemMasterID]   INT            NOT NULL,
    [ItemMasterName] NVARCHAR (255) NOT NULL,
    [NCS_Col_Name]   NVARCHAR (255) NOT NULL,
    [Template]       NVARCHAR (255) NOT NULL,
    [ItemIDs]        NVARCHAR (255) NOT NULL
);

