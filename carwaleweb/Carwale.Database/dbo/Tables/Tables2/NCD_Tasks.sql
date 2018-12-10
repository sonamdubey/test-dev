CREATE TABLE [dbo].[NCD_Tasks] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [CategoryId]      SMALLINT      NOT NULL,
    [TaskName]        VARCHAR (50)  NOT NULL,
    [TaskDescription] VARCHAR (200) NOT NULL,
    [EntryDate]       DATETIME      NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [IsVisible]       BIT           NULL,
    CONSTRAINT [PK_NCD_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC)
);

