CREATE TABLE [dbo].[LauraSegment] (
    [Id] NUMERIC (18) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_id1]
    ON [dbo].[LauraSegment]([Id] ASC);

