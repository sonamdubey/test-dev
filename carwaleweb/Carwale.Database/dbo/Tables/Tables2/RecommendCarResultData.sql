CREATE TABLE [dbo].[RecommendCarResultData] (
    [SearchId] NUMERIC (18)   NOT NULL,
    [Result]   VARCHAR (8000) NOT NULL,
    CONSTRAINT [PK_RecommendCarResultData] PRIMARY KEY CLUSTERED ([SearchId] ASC) WITH (FILLFACTOR = 90)
);

