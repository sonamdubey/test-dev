CREATE TABLE [dbo].[Aug15Survey] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [Email]         VARCHAR (50)  NOT NULL,
    [Phone]         VARCHAR (50)  NOT NULL,
    [Address]       VARCHAR (200) NULL,
    [City]          VARCHAR (50)  NULL,
    [BestSmall]     VARCHAR (50)  NULL,
    [BestMid]       VARCHAR (50)  NULL,
    [BestLuxury]    VARCHAR (50)  NULL,
    [BestUV]        VARCHAR (50)  NULL,
    [EntryDateTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_Aug15Survey] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

