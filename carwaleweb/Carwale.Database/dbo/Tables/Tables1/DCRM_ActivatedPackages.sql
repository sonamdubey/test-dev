CREATE TABLE [dbo].[DCRM_ActivatedPackages] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [DealerId]        INT      NOT NULL,
    [UpdatedOn]       DATETIME CONSTRAINT [DF_DCRM_ActivatedPackages_UpdatedOn] DEFAULT (getdate()) NULL,
    [UpdatedBy]       INT      NULL,
    [IsAutobizSynced] BIT      CONSTRAINT [DF_DCRM_ActivatedPackages_IsAutobizSynced] DEFAULT ((0)) NULL
);

