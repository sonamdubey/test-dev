CREATE TABLE [dbo].[CW_NewCarLTV] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [CarModelId] INT      NOT NULL,
    [Tenor]      INT      NULL,
    [LTV]        INT      NULL,
    [IsActive]   BIT      CONSTRAINT [DF_CW_NewCarLTV_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedBy]  INT      NULL,
    [UpdatedOn]  DATETIME NULL,
    [EntryDate]  DATETIME DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CW_NewCarLTV] PRIMARY KEY CLUSTERED ([Id] ASC)
);

