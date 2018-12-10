CREATE TABLE [dbo].[TC_AvailableRSAPackages] (
    [Id]                INT      IDENTITY (1, 1) NOT NULL,
    [PackageId]         INT      NULL,
    [BranchId]          INT      NULL,
    [UserId]            INT      NULL,
    [AvailableQuantity] INT      NULL,
    [IsActive]          BIT      DEFAULT ((1)) NULL,
    [EntryDate]         DATETIME DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]         DATETIME NULL,
    [ActivationDate]    DATETIME NULL,
    [ExpiryDate]        DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

