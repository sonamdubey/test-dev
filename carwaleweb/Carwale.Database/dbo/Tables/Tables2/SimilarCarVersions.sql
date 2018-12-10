CREATE TABLE [dbo].[SimilarCarVersions] (
    [ID]                  NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId]        NUMERIC (18) NULL,
    [SimilarCarVersionId] NUMERIC (18) NULL,
    [SimPoint]            NUMERIC (18) CONSTRAINT [DF_SimilarCarVersions_SimPoint] DEFAULT (0) NULL,
    [Isactive]            BIT          CONSTRAINT [DF_SimilarCarVersions_Isactibe] DEFAULT (1) NULL,
    CONSTRAINT [PK_SimilarCarVersions] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

