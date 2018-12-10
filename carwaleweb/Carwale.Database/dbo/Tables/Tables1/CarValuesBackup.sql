CREATE TABLE [dbo].[CarValuesBackup] (
    [CCV_Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId]   NUMERIC (18) NOT NULL,
    [CarYear]        INT          NOT NULL,
    [CarValue]       NUMERIC (18) NOT NULL,
    [GuideId]        INT          NOT NULL,
    [UpdateDateTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_CarValuesBackup] PRIMARY KEY CLUSTERED ([CCV_Id] ASC) WITH (FILLFACTOR = 90)
);

