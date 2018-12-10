CREATE TABLE [Mobile].[RegIdOStypeIntermediate] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [GCMRegId]     VARCHAR (200) NULL,
    [OSType]       INT           NOT NULL,
    [MobileUserId] INT           NULL
);


GO
CREATE CLUSTERED INDEX [Idx_Clst_RegIdOStypeIntermediate]
    ON [Mobile].[RegIdOStypeIntermediate]([ID] ASC);

