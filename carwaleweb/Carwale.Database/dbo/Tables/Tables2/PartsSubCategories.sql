CREATE TABLE [dbo].[PartsSubCategories] (
    [ID]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId] NUMERIC (18) NOT NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [IsActive]   BIT          CONSTRAINT [DF_PartsSubCategories_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_PartsSubCategories] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

