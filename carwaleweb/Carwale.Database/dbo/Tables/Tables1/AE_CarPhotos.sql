CREATE TABLE [dbo].[AE_CarPhotos] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]       NUMERIC (18)  NOT NULL,
    [IsMainPhoto] BIT           CONSTRAINT [DF_AE_CarPhotos_IsMainPhoto] DEFAULT ((0)) NOT NULL,
    [Caption]     VARCHAR (150) NULL,
    [UpdatedOn]   DATETIME      NULL,
    [UpdatedBy]   NUMERIC (18)  NULL,
    [HostUrl]     VARCHAR (100) NULL,
    CONSTRAINT [PK_AE_CarPhotos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

