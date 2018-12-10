CREATE TABLE [dbo].[TC_DealerMakes] (
    [TC_DealerMakesId] INT IDENTITY (1, 1) NOT NULL,
    [DealerId]         INT NULL,
    [MakeId]           INT NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_DealerMakes_DealerId]
    ON [dbo].[TC_DealerMakes]([DealerId] ASC)
    INCLUDE([MakeId]);


GO
CREATE NONCLUSTERED INDEX [ix_TC_DealerMakes_MakeId]
    ON [dbo].[TC_DealerMakes]([MakeId] ASC)
    INCLUDE([DealerId]);

