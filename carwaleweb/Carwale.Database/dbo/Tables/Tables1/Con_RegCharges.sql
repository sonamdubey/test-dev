CREATE TABLE [dbo].[Con_RegCharges] (
    [Id]       NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]  NUMERIC (18)   NULL,
    [CityId]   NUMERIC (18)   NULL,
    [Amount]   DECIMAL (9, 2) NULL,
    [IsActive] BIT            CONSTRAINT [DF_Con_RegCharges_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Con_RegChagres] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

