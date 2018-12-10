CREATE TABLE [dbo].[NewCarDiscounts] (
    [Id]                  NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId]        NUMERIC (18)    NOT NULL,
    [DiscountFrom]        DATETIME        NOT NULL,
    [DiscountTo]          DATETIME        NOT NULL,
    [IsCashDiscount]      BIT             CONSTRAINT [DF_NewCarDiscounts_IsCashDiscount] DEFAULT (0) NOT NULL,
    [DiscountDescription] VARCHAR (1000)  NULL,
    [DiscountValue]       DECIMAL (18, 2) NULL,
    [IsActive]            CHAR (10)       NULL,
    CONSTRAINT [PK_NewCarDiscounts] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

