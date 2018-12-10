CREATE TABLE [dbo].[DealerFeaturedCars] (
    [DFC_Id]   NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId] NUMERIC (18) NOT NULL,
    [CarId]    NUMERIC (18) NOT NULL,
    [UpdateOn] DATETIME     CONSTRAINT [DF_DealerFeaturedCars_UpdateOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DealerFeaturedCars] PRIMARY KEY CLUSTERED ([DFC_Id] ASC) WITH (FILLFACTOR = 90)
);

