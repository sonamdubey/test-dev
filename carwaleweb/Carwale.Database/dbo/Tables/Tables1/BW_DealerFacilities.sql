CREATE TABLE [dbo].[BW_DealerFacilities] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [DealerId] INT           NULL,
    [Facility] VARCHAR (500) NULL,
    [IsActive] BIT           CONSTRAINT [DF_BW_DealerFacilities_IsActive] DEFAULT ((0)) NULL
);

