CREATE TABLE [dbo].[FabiaSegment] (
    [Id] NUMERIC (18) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_id]
    ON [dbo].[FabiaSegment]([Id] ASC);

