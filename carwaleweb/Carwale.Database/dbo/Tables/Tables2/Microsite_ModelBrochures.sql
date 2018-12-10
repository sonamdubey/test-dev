CREATE TABLE [dbo].[Microsite_ModelBrochures] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ModelId]      INT           NOT NULL,
    [GeneratedKey] VARCHAR (100) NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Microsite_ModelBrochures_IsActive] DEFAULT ((0)) NULL,
    [IsReplicated] BIT           CONSTRAINT [DF_Microsite_ModelBrochures_IsReplicated] DEFAULT ((0)) NULL,
    [CreatedBy]    INT           NULL,
    [CreatedOn]    DATETIME      CONSTRAINT [DF_Microsite_ModelBrochures_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedBy]    INT           NULL,
    [UpdatedOn]    DATETIME      NULL,
    CONSTRAINT [PK__Microsit__3214EC0716209037] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [ix_Microsite_ModelBrochures_ModelId]
    ON [dbo].[Microsite_ModelBrochures]([ModelId] ASC);

