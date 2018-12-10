CREATE TABLE [dbo].[CT_AddOnPackages] (
    [Id]             INT      IDENTITY (1, 1) NOT NULL,
    [AddOnPackageId] INT      NOT NULL,
    [CWDealerId]     INT      NOT NULL,
    [StartDate]      DATETIME NOT NULL,
    [EndDate]        DATETIME NOT NULL,
    [CreatedOn]      DATETIME CONSTRAINT [DF_CT_AddOnPackages_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]      DATETIME NULL,
    [IsActive]       BIT      CONSTRAINT [DF_CT_AddOnPackages_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CT_AddOnPackages] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CT_AddOnPackages_CWDealerId]
    ON [dbo].[CT_AddOnPackages]([CWDealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CT_AddOnPackages_AddOnPackageId]
    ON [dbo].[CT_AddOnPackages]([AddOnPackageId] ASC);

