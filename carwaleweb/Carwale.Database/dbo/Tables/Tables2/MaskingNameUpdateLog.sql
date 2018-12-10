CREATE TABLE [dbo].[MaskingNameUpdateLog] (
    [ModelId]     INT           NULL,
    [MaskingName] VARCHAR (100) NULL,
    UNIQUE NONCLUSTERED ([MaskingName] ASC)
);

