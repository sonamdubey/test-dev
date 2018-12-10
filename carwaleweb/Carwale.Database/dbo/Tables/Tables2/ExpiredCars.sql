CREATE TABLE [dbo].[ExpiredCars] (
    [CarVersionId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ExpiredCars] PRIMARY KEY CLUSTERED ([CarVersionId] ASC) WITH (FILLFACTOR = 90)
);

