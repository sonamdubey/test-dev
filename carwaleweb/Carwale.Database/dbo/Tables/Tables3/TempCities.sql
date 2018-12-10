CREATE TABLE [dbo].[TempCities] (
    [Id]          FLOAT (53)     NOT NULL,
    [Name]        NVARCHAR (255) NULL,
    [StateId]     FLOAT (53)     NULL,
    [IsDeleted]   BIT            NOT NULL,
    [IsUniversal] BIT            NOT NULL,
    [Lattitude]   FLOAT (53)     NULL,
    [Longitude]   FLOAT (53)     NULL,
    [StdCode]     FLOAT (53)     NULL,
    CONSTRAINT [PK_TempCities] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

