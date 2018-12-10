CREATE TABLE [dbo].[TC_TaskCategory] (
    [Id]                  SMALLINT      IDENTITY (1, 1) NOT NULL,
    [CategoryName]        VARCHAR (50)  NOT NULL,
    [EntryDate]           DATETIME      CONSTRAINT [DF_TC_TaskCategory1_EntryDate] DEFAULT (getdate()) NOT NULL,
    [isActive]            BIT           CONSTRAINT [DF_TC_TaskCategory1_isActive] DEFAULT ((1)) NOT NULL,
    [TC_DealerTypeId]     TINYINT       NULL,
    [CategoryDescription] VARCHAR (100) NULL
);

