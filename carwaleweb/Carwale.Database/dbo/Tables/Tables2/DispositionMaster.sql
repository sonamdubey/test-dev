CREATE TABLE [dbo].[DispositionMaster] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (150) NOT NULL,
    [IsActive]    BIT           CONSTRAINT [DF_DispositionMaster_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DispositionMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

