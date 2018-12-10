CREATE TABLE [dbo].[DetailedComparisons] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Model1Id]   NUMERIC (18)  NULL,
    [Model2Id]   NUMERIC (18)  NULL,
    [Version1Id] NUMERIC (18)  NULL,
    [Version2Id] NUMERIC (18)  NULL,
    [Path]       VARCHAR (100) NULL,
    [IsActive]   BIT           CONSTRAINT [DF_DetailedComparisons_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_DetailedComparisons] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

