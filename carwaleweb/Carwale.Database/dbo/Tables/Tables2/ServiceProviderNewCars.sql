CREATE TABLE [dbo].[ServiceProviderNewCars] (
    [ID]                      NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ServiceProviderBranchId] NUMERIC (18) NOT NULL,
    [MakeId]                  NUMERIC (18) NOT NULL,
    [IsAuthorised]            BIT          CONSTRAINT [DF_ServiceProviderNewCars_IsAuthorised] DEFAULT (0) NOT NULL,
    [IsActive]                BIT          CONSTRAINT [DF_ServiceProviderNewCars_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ServiceProviderNewCars] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ServiceProviderNewCars_CarMakes] FOREIGN KEY ([MakeId]) REFERENCES [dbo].[CarMakes] ([ID]),
    CONSTRAINT [FK_ServiceProviderNewCars_ServiceProviderBranchs] FOREIGN KEY ([ServiceProviderBranchId]) REFERENCES [dbo].[ServiceProviderBranchs] ([ID]),
    CONSTRAINT [IX_ServiceProviderNewCars] UNIQUE NONCLUSTERED ([ServiceProviderBranchId] ASC, [MakeId] ASC) WITH (FILLFACTOR = 90)
);

