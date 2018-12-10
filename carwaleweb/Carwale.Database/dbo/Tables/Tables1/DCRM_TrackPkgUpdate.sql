CREATE TABLE [dbo].[DCRM_TrackPkgUpdate] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ReqId]        NUMERIC (18) NOT NULL,
    [DealerId]     NUMERIC (18) NOT NULL,
    [PkgUpdatedOn] DATETIME     NOT NULL,
    [PkgUpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DCRM_TrackPkgUpdate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

