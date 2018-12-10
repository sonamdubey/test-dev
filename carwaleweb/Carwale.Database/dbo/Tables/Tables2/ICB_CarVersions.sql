CREATE TABLE [dbo].[ICB_CarVersions] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId] NUMERIC (18) NOT NULL,
    [IsActive]     BIT          CONSTRAINT [DF_ICB_CarVersions_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn]    DATETIME     CONSTRAINT [DF_ICB_CarVersions_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ICB_CarVersions_1] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

