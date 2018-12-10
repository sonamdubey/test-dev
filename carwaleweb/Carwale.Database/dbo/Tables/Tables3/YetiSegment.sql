CREATE TABLE [dbo].[YetiSegment] (
    [Id] NUMERIC (18) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_id2]
    ON [dbo].[YetiSegment]([Id] ASC);

