CREATE TABLE [dbo].[NI_PDF] (
    [ID]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [FirstName]      VARCHAR (50) NULL,
    [LastName]       VARCHAR (50) NULL,
    [Email]          VARCHAR (50) NULL,
    [ContactNo]      VARCHAR (50) NULL,
    [CityId]         NUMERIC (18) NULL,
    [VersionId]      NUMERIC (18) NULL,
    [DateOfPurchase] DATETIME     NULL,
    [KilometersDone] NUMERIC (18) NULL,
    CONSTRAINT [PK_NI_PDF] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

