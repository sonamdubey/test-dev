CREATE TABLE [dbo].[MG_Scheduled] (
    [CustomerId] NUMERIC (18) NOT NULL,
    [VolumeId]   NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_MG_Scheduled] PRIMARY KEY CLUSTERED ([CustomerId] ASC, [VolumeId] ASC) WITH (FILLFACTOR = 90)
);

