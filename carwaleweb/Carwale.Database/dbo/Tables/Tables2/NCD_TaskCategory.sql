CREATE TABLE [dbo].[NCD_TaskCategory] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName] VARCHAR (50) NOT NULL,
    [EntryDate]    DATETIME     NOT NULL,
    [IsActive]     BIT          NOT NULL,
    CONSTRAINT [PK_NCD_TaskCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

