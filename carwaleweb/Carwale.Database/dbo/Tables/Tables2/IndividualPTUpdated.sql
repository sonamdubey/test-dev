CREATE TABLE [dbo].[IndividualPTUpdated] (
    [Id]                 NUMERIC (18) NOT NULL,
    [SourceId]           SMALLINT     NOT NULL,
    [PackageType]        SMALLINT     NULL,
    [EntryDate]          DATETIME     NOT NULL,
    [PaymentMode]        TINYINT      NULL,
    [IsPremium]          BIT          NULL,
    [ActualAmount]       NUMERIC (18) NULL,
    [IsListingCompleted] BIT          NULL,
    [CurrentStep]        TINYINT      NULL,
    [StatusId]           SMALLINT     NULL
);

