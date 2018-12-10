CREATE TABLE [CD].[ItemType] (
    [ItemTypeId]  SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [Description] VARCHAR (150) NULL,
    [IsViewable]  BIT           NULL,
    [IsEditable]  BIT           NULL,
    [CreatedOn]   DATETIME      NULL,
    [UpdatedOn]   DATETIME      NULL,
    [UpdatedBy]   VARCHAR (50)  NULL,
    CONSTRAINT [PK_ItemType] PRIMARY KEY CLUSTERED ([ItemTypeId] ASC)
);

