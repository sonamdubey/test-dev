CREATE TABLE [dbo].[DataMigrn] (
    [Tablename]             VARCHAR (100) NOT NULL,
    [TableType]             VARCHAR (20)  NULL,
    [MigrationType]         VARCHAR (20)  NULL,
    [TableSize]             INT           NULL,
    [IndexSize]             INT           NULL,
    [Rowcnt]                BIGINT        NULL,
    [LastRecordInterserted] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Tablename] ASC),
    CHECK ([MigrationType]='Full Reload' OR [MigrationType]='Icremental' OR [MigrationType]='One time Migration'),
    CHECK ([TableType]='Not Needed' OR [TableType]='Log' OR [TableType]='Transaction' OR [TableType]='Master')
);

