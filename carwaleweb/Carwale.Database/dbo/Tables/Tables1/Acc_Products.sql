CREATE TABLE [dbo].[Acc_Products] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProductName] VARCHAR (50) NOT NULL,
    [IsActive]    BIT          CONSTRAINT [DF_Accessories_Products_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Accessories_Products] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Acc_Products_IsActive]
    ON [dbo].[Acc_Products]([IsActive] ASC);

