CREATE TABLE [dbo].[NanoDriveEncodedData] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EncodedPoint] VARCHAR (8000) NOT NULL,
    [EncodedLevel] VARCHAR (3000) NOT NULL,
    [UpdatedOn]    DATETIME       CONSTRAINT [DF_NanoDrivePoints_UpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_NanoDriveEncodedData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

