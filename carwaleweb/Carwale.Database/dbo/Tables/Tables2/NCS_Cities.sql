CREATE TABLE [dbo].[NCS_Cities] (
    [CityId]   NUMERIC (18) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_NCS_Cities_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_NCS_Cities] PRIMARY KEY CLUSTERED ([CityId] ASC) WITH (FILLFACTOR = 90)
);

