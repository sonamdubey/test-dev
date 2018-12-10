CREATE TABLE [AC].[SRC_Keywords] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [KeywordTypeId] TINYINT       NULL,
    [ReferenceId]   INT           NULL,
    [DisplayName]   VARCHAR (100) NULL,
    [IsNew]         BIT           CONSTRAINT [DF__SRC_Keywo__IsNew__6E87A029] DEFAULT ((0)) NULL,
    [IsUsed]        BIT           CONSTRAINT [DF__SRC_Keywo__IsUse__6F7BC462] DEFAULT ((0)) NULL,
    [IsPriceExist]  BIT           CONSTRAINT [DF__SRC_Keywo__IsPri__706FE89B] DEFAULT ((0)) NULL,
    [Value]         VARCHAR (500) NULL,
    [IsAutomated]   BIT           NULL,
    CONSTRAINT [PK_SRC_Keywords] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_SRC_Keywords_KeywordTypeId]
    ON [AC].[SRC_Keywords]([KeywordTypeId] ASC)
    INCLUDE([Id], [ReferenceId]);

