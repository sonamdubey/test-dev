CREATE TABLE [UCAlert].[ModelListNotConsiderInAlgo] (
    [ModelListNotConsiderInAlgoId] INT IDENTITY (1, 1) NOT NULL,
    [CarModelsId]                  INT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_ModelListNotConsiderInAlgo_CarModelsId]
    ON [UCAlert].[ModelListNotConsiderInAlgo]([CarModelsId] ASC);

