CREATE TABLE [dbo].[ESM_Products] (
    [id]           NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Product]      VARCHAR (50)    NOT NULL,
    [ProductType]  INT             NULL,
    [UnitPrice]    NUMERIC (18)    NULL,
    [MinimumPrice] DECIMAL (18, 2) NULL,
    [IsActive]     BIT             CONSTRAINT [DF_ESM_Products_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn]    DATETIME        NOT NULL,
    [UpdatedBy]    NUMERIC (18)    NOT NULL,
    CONSTRAINT [PK_ESM_Products] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

