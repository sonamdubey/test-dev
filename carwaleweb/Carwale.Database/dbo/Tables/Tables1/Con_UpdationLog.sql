CREATE TABLE [dbo].[Con_UpdationLog] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [PageURL]       VARCHAR (500) NULL,
    [Section]       VARCHAR (150) NULL,
    [UpdatedBy]     NUMERIC (18)  NULL,
    [UpdatedOn]     DATETIME      NULL,
    [Comment]       VARCHAR (250) NULL,
    [UpdatedByName] VARCHAR (150) NULL,
    [UpdatedId]     VARCHAR (250) NULL,
    CONSTRAINT [PK_Con_UpdationLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

