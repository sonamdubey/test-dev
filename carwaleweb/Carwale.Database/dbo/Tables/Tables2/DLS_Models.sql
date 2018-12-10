CREATE TABLE [dbo].[DLS_Models] (
    [ModelId]  NUMERIC (18) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_DLS_Models_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_DLS_Models] PRIMARY KEY CLUSTERED ([ModelId] ASC) WITH (FILLFACTOR = 90)
);

