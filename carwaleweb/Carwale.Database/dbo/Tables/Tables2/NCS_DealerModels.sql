CREATE TABLE [dbo].[NCS_DealerModels] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]   NUMERIC (18) NULL,
    [ModelId]    NUMERIC (18) NOT NULL,
    [DealerCode] VARCHAR (50) NULL,
    CONSTRAINT [PK_NCS_DealerModels] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

