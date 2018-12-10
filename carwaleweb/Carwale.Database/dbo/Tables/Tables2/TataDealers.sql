CREATE TABLE [dbo].[TataDealers] (
    [CityId]     NUMERIC (18)  NOT NULL,
    [DealerId]   NUMERIC (18)  NOT NULL,
    [DealerName] VARCHAR (250) NULL,
    [ContactNo]  VARCHAR (200) NULL,
    CONSTRAINT [PK_TataDealers] PRIMARY KEY CLUSTERED ([CityId] ASC, [DealerId] ASC) WITH (FILLFACTOR = 90)
);

