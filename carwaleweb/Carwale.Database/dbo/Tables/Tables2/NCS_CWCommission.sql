CREATE TABLE [dbo].[NCS_CWCommission] (
    [Id]         NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CityId]     NUMERIC (18)    NULL,
    [ModelId]    NUMERIC (18)    NULL,
    [VersionId]  NUMERIC (18)    NULL,
    [Commission] NUMERIC (18, 2) NULL,
    CONSTRAINT [PK_Ncs_CWCommission] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

