CREATE TABLE [dbo].[NCS_CDDiscount] (
    [Id]          NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarModelId]  NUMERIC (18)    NULL,
    [CFId]        NUMERIC (18)    NULL,
    [Discount]    NUMERIC (18, 2) NULL,
    [LastUpdated] DATETIME        NULL,
    CONSTRAINT [PK_NCS_CCDiscount] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

