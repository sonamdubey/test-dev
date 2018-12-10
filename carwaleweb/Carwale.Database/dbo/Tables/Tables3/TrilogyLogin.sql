CREATE TABLE [dbo].[TrilogyLogin] (
    [Id]        NUMERIC (18) NOT NULL,
    [LoginId]   VARCHAR (50) NOT NULL,
    [Password]  VARCHAR (20) NOT NULL,
    [isActive]  BIT          CONSTRAINT [DF_TrilogyLogin_isActive] DEFAULT (1) NOT NULL,
    [LastLogin] DATETIME     NULL,
    [LDTakerId] NUMERIC (18) NULL,
    CONSTRAINT [PK_TrilogyLogin] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

