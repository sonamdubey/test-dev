CREATE TABLE [dbo].[NCS_DealerMakes] (
    [DealerId] NUMERIC (18) NOT NULL,
    [MakeId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_NCS_DealerMakes] PRIMARY KEY CLUSTERED ([DealerId] ASC, [MakeId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_NCS_DealerMakes_MakeId]
    ON [dbo].[NCS_DealerMakes]([MakeId] ASC);

