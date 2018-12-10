CREATE TABLE [dbo].[CT_AddOnPackagesLog] (
    [Id]                 INT      IDENTITY (1, 1) NOT NULL,
    [CT_AddOnPackagesId] INT      NOT NULL,
    [AddOnPackageId]     INT      NOT NULL,
    [CWDealerId]         INT      NOT NULL,
    [StartDate]          DATETIME NOT NULL,
    [EndDate]            DATETIME NOT NULL,
    [CreatedOn]          DATETIME NOT NULL,
    [UpdatedOn]          DATETIME NULL,
    [IsActive]           BIT      NOT NULL,
    CONSTRAINT [PK_CT_AddOnPackagesLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

