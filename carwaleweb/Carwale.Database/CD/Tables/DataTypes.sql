CREATE TABLE [CD].[DataTypes] (
    [DataTypeId]  SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [Description] VARCHAR (150) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_DataTypes_IsActive] DEFAULT ((1)) NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_DataTypes_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]   DATETIME      NULL,
    [UpdatedBy]   VARCHAR (50)  NULL,
    CONSTRAINT [PK_DataType] PRIMARY KEY CLUSTERED ([DataTypeId] ASC)
);

