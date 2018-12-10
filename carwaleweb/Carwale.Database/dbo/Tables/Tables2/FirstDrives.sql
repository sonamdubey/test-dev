CREATE TABLE [dbo].[FirstDrives] (
    [ID]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]   NUMERIC (18)  NULL,
    [VersionId] NUMERIC (18)  NULL,
    [Path]      VARCHAR (100) NULL,
    [IsActive]  BIT           CONSTRAINT [DF_FirstDrives_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_FirstDrives] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

