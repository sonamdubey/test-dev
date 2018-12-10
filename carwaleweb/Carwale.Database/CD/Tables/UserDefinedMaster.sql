CREATE TABLE [CD].[UserDefinedMaster] (
    [UserDefinedId]   INT           IDENTITY (1, 1) NOT NULL,
    [ItemMasterId]    BIGINT        NULL,
    [Name]            VARCHAR (100) NULL,
    [Description]     VARCHAR (150) NULL,
    [IsActive]        BIT           CONSTRAINT [DF_UserDefinedMaster_IsActive] DEFAULT ((1)) NULL,
    [IsAvailable]     BIT           NULL,
    [SortOrder]       SMALLINT      NULL,
    [CreatedOn]       DATETIME      CONSTRAINT [DF_UserDefinedMaster_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]       DATETIME      NULL,
    [UpdatedBy]       VARCHAR (50)  NULL,
    [ValueImportance] FLOAT (53)    NULL,
    CONSTRAINT [PK_UserDefinedMaster] PRIMARY KEY CLUSTERED ([UserDefinedId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'Description', @value = N'This table is used to store custom i.e. user defined data type.
e.g. Car engine power measured as 1.2 BHP, so 1.2 is value and BHP is user defined data type', @level0type = N'SCHEMA', @level0name = N'CD', @level1type = N'TABLE', @level1name = N'UserDefinedMaster';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sortorder column used to compare the features and specification with other car. It will have range of values so that at any time we can add or remove values withoiut reordering.', @level0type = N'SCHEMA', @level0name = N'CD', @level1type = N'TABLE', @level1name = N'UserDefinedMaster', @level2type = N'COLUMN', @level2name = N'SortOrder';

