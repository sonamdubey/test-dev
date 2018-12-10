CREATE TABLE [dbo].[Acc_ItemCategory] (
    [CategoryId] NUMERIC (18) NOT NULL,
    [ItemId]     NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_Acc_ItemCategory] PRIMARY KEY CLUSTERED ([CategoryId] ASC, [ItemId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Acc_ItemCategory_ItemId]
    ON [dbo].[Acc_ItemCategory]([ItemId] ASC);

