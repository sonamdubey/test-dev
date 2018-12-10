CREATE TABLE [dbo].[Acc_Categories] (
    [Id]            NUMERIC (10)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId]    VARCHAR (50)  NOT NULL,
    [CategoryName]  VARCHAR (100) NOT NULL,
    [ParentNode]    VARCHAR (50)  NULL,
    [NextChildNode] INT           NOT NULL,
    [Ancestors]     VARCHAR (50)  NULL,
    [Depth]         INT           NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_Accessories_Categories_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Accessories_Categories] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Acc_Categories_IsActive]
    ON [dbo].[Acc_Categories]([CategoryId] ASC);

