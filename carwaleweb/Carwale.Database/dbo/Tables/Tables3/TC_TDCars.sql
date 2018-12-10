CREATE TABLE [dbo].[TC_TDCars] (
    [TC_TDCarsId] INT           IDENTITY (1, 1) NOT NULL,
    [BranchId]    BIGINT        NOT NULL,
    [VersionId]   INT           NOT NULL,
    [CarName]     VARCHAR (150) NULL,
    [RegNo]       VARCHAR (50)  NULL,
    [KmsDriven]   NUMERIC (18)  NULL,
    [EntryDate]   DATETIME2 (7) CONSTRAINT [DF_TC_TDCars_EntryDate] DEFAULT (getdate()) NOT NULL,
    [VinNo]       VARCHAR (50)  NULL,
    [IsActive]    BIT           CONSTRAINT [DF_TC_TDCars_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_TDCars] PRIMARY KEY CLUSTERED ([TC_TDCarsId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_TDCars_IsActive]
    ON [dbo].[TC_TDCars]([IsActive] ASC);

