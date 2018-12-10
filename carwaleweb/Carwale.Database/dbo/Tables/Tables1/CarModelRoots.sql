CREATE TABLE [dbo].[CarModelRoots] (
    [RootId]        INT          IDENTITY (1, 1) NOT NULL,
    [RootName]      VARCHAR (80) NULL,
    [MakeId]        SMALLINT     NULL,
    [IsSuperLuxury] BIT          DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CarModelRoots] PRIMARY KEY CLUSTERED ([RootId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CarModelRoots_MakeId]
    ON [dbo].[CarModelRoots]([MakeId] ASC);

