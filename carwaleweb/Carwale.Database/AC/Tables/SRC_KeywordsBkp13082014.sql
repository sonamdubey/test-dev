CREATE TABLE [AC].[SRC_KeywordsBkp13082014] (
    [Id]            SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [KeywordTypeId] TINYINT       NULL,
    [ReferenceId]   SMALLINT      NULL,
    [DisplayName]   VARCHAR (100) NULL,
    [IsNew]         BIT           NULL,
    [IsUsed]        BIT           NULL,
    [IsPriceExist]  BIT           NULL,
    [Value]         VARCHAR (500) NULL,
    [IsAutomated]   BIT           NULL
);

