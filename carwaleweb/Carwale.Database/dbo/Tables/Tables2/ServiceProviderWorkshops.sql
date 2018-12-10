CREATE TABLE [dbo].[ServiceProviderWorkshops] (
    [ID]                      NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ServiceProviderBranchId] NUMERIC (18) NOT NULL,
    [IsActive]                BIT          CONSTRAINT [DF_ServiceProviderWorkshops_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ServiceProviderWorkshops] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ServiceProviderWorkshops_ServiceProviderBranchs] FOREIGN KEY ([ServiceProviderBranchId]) REFERENCES [dbo].[ServiceProviderBranchs] ([ID]),
    CONSTRAINT [IX_ServiceProviderWorkshops] UNIQUE NONCLUSTERED ([ServiceProviderBranchId] ASC) WITH (FILLFACTOR = 90)
);

