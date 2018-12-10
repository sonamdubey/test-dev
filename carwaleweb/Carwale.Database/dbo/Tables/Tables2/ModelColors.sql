CREATE TABLE [dbo].[ModelColors] (
    [ID]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarModelID] NUMERIC (18) NOT NULL,
    [Color]      VARCHAR (50) NOT NULL,
    [Code]       VARCHAR (50) NULL,
    [HexCode]    VARCHAR (50) NULL,
    [IsActive]   BIT          CONSTRAINT [DF_ModelColors_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ModelColors] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

