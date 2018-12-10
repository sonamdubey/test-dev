CREATE TABLE [dbo].[UsedCarCertification] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]  INT          NOT NULL,
    [CustomerId] NUMERIC (18) NOT NULL,
    [ProfileId]  VARCHAR (50) NULL,
    [EntryDate]  DATETIME     NOT NULL,
    [IsActive]   BIT          CONSTRAINT [DF_UsedCarCertification_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_UsedCarCertification] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

