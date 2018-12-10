CREATE TABLE [dbo].[MySQLTables] (
    [Tablename]             VARCHAR (100) NOT NULL,
    [TableType]             VARCHAR (20)  NULL,
    [MigrationType]         VARCHAR (20)  NULL,
    [TableSize]             INT           NULL,
    [IndexSize]             INT           NULL,
    [Rowcnt]                BIGINT        NULL,
    [LastRecordInterserted] DATETIME      NULL
);

