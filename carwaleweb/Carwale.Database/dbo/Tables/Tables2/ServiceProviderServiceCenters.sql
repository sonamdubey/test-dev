﻿CREATE TABLE [dbo].[ServiceProviderServiceCenters] (
    [ID]                      NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ServiceProviderBranchId] NUMERIC (18) NOT NULL,
    [MakeId]                  NUMERIC (18) NOT NULL,
    [IsActive]                BIT          CONSTRAINT [DF_ServiceProviderServiceCenters_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_ServiceProviderServiceCenters] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_ServiceProviderServiceCenters_CarMakes] FOREIGN KEY ([MakeId]) REFERENCES [dbo].[CarMakes] ([ID]),
    CONSTRAINT [FK_ServiceProviderServiceCenters_ServiceProviderBranchs] FOREIGN KEY ([ServiceProviderBranchId]) REFERENCES [dbo].[ServiceProviderBranchs] ([ID]),
    CONSTRAINT [IX_ServiceProviderServiceCenters] UNIQUE NONCLUSTERED ([ServiceProviderBranchId] ASC, [MakeId] ASC) WITH (FILLFACTOR = 90)
);

