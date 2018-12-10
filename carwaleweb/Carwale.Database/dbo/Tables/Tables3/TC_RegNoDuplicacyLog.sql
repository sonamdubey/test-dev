CREATE TABLE [dbo].[TC_RegNoDuplicacyLog] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [NewStockId]          INT      NOT NULL,
    [OldStockId]          INT      NOT NULL,
    [AbSure_CarDetailsId] INT      NOT NULL,
    [CreatedOn]           DATETIME NOT NULL,
    [CreatedBy]           INT      NOT NULL,
    CONSTRAINT [PK_TC_RegNoDuplicacyLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

