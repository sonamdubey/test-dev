CREATE TABLE [dbo].[LDCars] (
    [LDTakerId] INT          NOT NULL,
    [VersionId] NUMERIC (18) NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_LDCars_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_LDCars] PRIMARY KEY CLUSTERED ([LDTakerId] ASC, [VersionId] ASC) WITH (FILLFACTOR = 90)
);

