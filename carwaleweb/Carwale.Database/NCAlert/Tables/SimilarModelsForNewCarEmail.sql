CREATE TABLE [NCAlert].[SimilarModelsForNewCarEmail] (
    [SimilarCarModelsId]   INT           IDENTITY (1, 1) NOT NULL,
    [PQVersionId]          INT           NULL,
    [SmModelId]            INT           NULL,
    [SmMakeId]             INT           NULL,
    [SmMakeName]           VARCHAR (50)  NULL,
    [SmModelName]          VARCHAR (50)  NULL,
    [SmModelMaskingName]   VARCHAR (50)  NULL,
    [SmImageUrl]           VARCHAR (250) NULL,
    [SmMinPrice]           INT           NULL,
    [SimilarPriorityOrder] TINYINT       NULL,
    CONSTRAINT [PK_SimilarCarModelsId] PRIMARY KEY NONCLUSTERED ([SimilarCarModelsId] ASC)
);

