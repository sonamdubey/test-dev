CREATE TABLE [dbo].[SimilarCars] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]       NUMERIC (18)   NOT NULL,
    [SimilarVersions] VARCHAR (8000) NOT NULL,
    [UpdatedOn]       DATETIME       NULL,
    [IsActive]        BIT            CONSTRAINT [DF_SimilarCars_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SimilarCars_1] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_SimilarCars_VersionId]
    ON [dbo].[SimilarCars]([VersionId] ASC);

