CREATE TABLE [dbo].[SimilarCarModels] (
    [Id]            NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]       NUMERIC (18)   NOT NULL,
    [SimilarModels] VARCHAR (5000) NULL,
    [UpdatedOn]     DATETIME       NULL,
    [IsActive]      BIT            CONSTRAINT [DF_SimilarCarModels_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SimilarCarModels] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_SimilarCarModels_ModelId]
    ON [dbo].[SimilarCarModels]([ModelId] ASC);

