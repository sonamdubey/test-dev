CREATE TABLE [dbo].[MobilePatterns] (
    [Number]  NUMERIC (18)  NOT NULL,
    [SP]      VARCHAR (50)  NULL,
    [State]   VARCHAR (100) NULL,
    [Region]  VARCHAR (100) NULL,
    [StateId] VARCHAR (100) NULL,
    [CityId]  VARCHAR (100) NULL,
    CONSTRAINT [PK_MobilePatterns] PRIMARY KEY CLUSTERED ([Number] ASC) WITH (FILLFACTOR = 90)
);

