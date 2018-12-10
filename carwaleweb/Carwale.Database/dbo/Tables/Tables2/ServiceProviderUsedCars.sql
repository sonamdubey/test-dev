CREATE TABLE [dbo].[ServiceProviderUsedCars] (
    [ID]                      NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ServiceProviderBranchId] NUMERIC (18) NOT NULL,
    [IsActive]                BIT          CONSTRAINT [DF_ServiceProviderUsedCars_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ServiceProviderUsedCars] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ServiceProviderUsedCars_ServiceProviderBranchs] FOREIGN KEY ([ServiceProviderBranchId]) REFERENCES [dbo].[ServiceProviderBranchs] ([ID]),
    CONSTRAINT [IX_ServiceProviderUsedCars] UNIQUE NONCLUSTERED ([ServiceProviderBranchId] ASC) WITH (FILLFACTOR = 90)
);

