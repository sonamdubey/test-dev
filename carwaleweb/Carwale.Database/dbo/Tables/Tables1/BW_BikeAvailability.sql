CREATE TABLE [dbo].[BW_BikeAvailability] (
    [ID]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [DealerId]      INT          NULL,
    [BikeVersionId] INT          NULL,
    [NumOfDays]     INT          NULL,
    [IsActive]      BIT          CONSTRAINT [DF__BW_BikeAv__IsAct__032A89F8] DEFAULT ((1)) NULL
);

