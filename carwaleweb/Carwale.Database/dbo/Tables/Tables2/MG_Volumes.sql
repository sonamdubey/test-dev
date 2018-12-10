CREATE TABLE [dbo].[MG_Volumes] (
    [VolumeId]      NUMERIC (18) NOT NULL,
    [MagMonth]      DATETIME     NULL,
    [CurrentVolume] BIT          CONSTRAINT [DF_MG_Volumes_CurrentVolume] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_MG_Volumes] PRIMARY KEY CLUSTERED ([VolumeId] ASC) WITH (FILLFACTOR = 90)
);

