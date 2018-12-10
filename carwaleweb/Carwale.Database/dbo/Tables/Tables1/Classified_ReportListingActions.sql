CREATE TABLE [dbo].[Classified_ReportListingActions] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ProfileId]     NUMERIC (18)  NOT NULL,
    [ActionTakenBy] NUMERIC (18)  NULL,
    [ActionTakenOn] DATETIME      NULL,
    [Comments]      VARCHAR (200) NULL,
    [Type]          SMALLINT      NULL,
    CONSTRAINT [PK_ClassReLisActions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

