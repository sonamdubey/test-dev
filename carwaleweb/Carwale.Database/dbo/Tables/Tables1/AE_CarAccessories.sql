CREATE TABLE [dbo].[AE_CarAccessories] (
    [CarId]       NUMERIC (18) NOT NULL,
    [AccessoryId] NUMERIC (18) NOT NULL,
    [UpdatedOn]   DATETIME     NULL,
    [UpdatedBy]   NUMERIC (18) NULL,
    CONSTRAINT [PK_AE_CarAccessories] PRIMARY KEY CLUSTERED ([CarId] ASC, [AccessoryId] ASC) WITH (FILLFACTOR = 90)
);

