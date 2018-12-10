CREATE TABLE [dbo].[NanoDrive] (
    [Id]        NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Lattitude] DECIMAL (18, 6) NULL,
    [Longitude] DECIMAL (18, 6) NULL,
    [IsCurrent] BIT             CONSTRAINT [DF_NanoDrive_IsCurrent] DEFAULT ((1)) NULL,
    [EntryDate] DATETIME        CONSTRAINT [DF_NanoDrive_EntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_NanoDrive] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

