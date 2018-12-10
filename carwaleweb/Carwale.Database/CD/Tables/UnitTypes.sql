CREATE TABLE [CD].[UnitTypes] (
    [UnitTypeId]  SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [Description] VARCHAR (150) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_UnitTypes_IsActive] DEFAULT ((1)) NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_UnitTypes_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]   DATETIME      NULL,
    [UpdatedBy]   VARCHAR (50)  NULL,
    CONSTRAINT [PK_UnitType] PRIMARY KEY CLUSTERED ([UnitTypeId] ASC)
);

