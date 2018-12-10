CREATE TABLE [dbo].[TC_DispositionItem] (
    [TC_DispositionItemId] TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]                 VARCHAR (50) NULL,
    [IsActive]             BIT          CONSTRAINT [DF_TC_DispositionItem_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_DispositionItemId] PRIMARY KEY NONCLUSTERED ([TC_DispositionItemId] ASC)
);

