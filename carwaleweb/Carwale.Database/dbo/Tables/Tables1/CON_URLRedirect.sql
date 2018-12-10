CREATE TABLE [dbo].[CON_URLRedirect] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ServerId]     SMALLINT      NULL,
    [ModelId]      NUMERIC (18)  NULL,
    [ModelName]    VARCHAR (200) NULL,
    [CreatedOn]    DATETIME      NULL,
    [IsReplicated] BIT           CONSTRAINT [DF_CON_URLRedirect_IsReplicated] DEFAULT ((0)) NULL,
    [ReplicatedOn] DATETIME      NULL,
    CONSTRAINT [PK_CarModelsURL] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Model, 2-Accessory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CON_URLRedirect', @level2type = N'COLUMN', @level2name = N'ServerId';

