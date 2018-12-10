CREATE TABLE [AC].[IntermediateSRC_Keywords] (
    [Id]            SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [KeywordTypeId] TINYINT       NULL,
    [ReferenceId]   SMALLINT      NULL,
    [DisplayName]   VARCHAR (100) NULL,
    [IsNew]         BIT           DEFAULT ((0)) NULL,
    [IsUsed]        BIT           DEFAULT ((0)) NULL,
    [IsPriceExist]  BIT           DEFAULT ((0)) NULL,
    [Value]         VARCHAR (500) NULL,
    [IsAutomated]   BIT           NULL,
    CONSTRAINT [PK_IntermediateSRC_Keywords] PRIMARY KEY CLUSTERED ([Id] ASC)
);

