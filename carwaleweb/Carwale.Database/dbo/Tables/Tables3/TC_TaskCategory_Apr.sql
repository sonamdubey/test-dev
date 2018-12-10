CREATE TABLE [dbo].[TC_TaskCategory_Apr] (
    [Id]           SMALLINT     NOT NULL,
    [CategoryName] VARCHAR (50) NOT NULL,
    [EntryDate]    DATETIME     NOT NULL,
    [isActive]     BIT          CONSTRAINT [DF_TC_TaskCategory_isActive] DEFAULT ((1)) NOT NULL
);

