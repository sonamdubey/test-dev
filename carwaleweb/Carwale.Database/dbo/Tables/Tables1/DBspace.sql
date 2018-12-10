CREATE TABLE [dbo].[DBspace] (
    [name]      NVARCHAR (100) NULL,
    [rows]      CHAR (11)      NULL,
    [reserved]  NVARCHAR (15)  NULL,
    [data]      NVARCHAR (18)  NULL,
    [indexes]   NVARCHAR (18)  NULL,
    [unused]    NVARCHAR (18)  NULL,
    [tablesize] BIGINT         NULL,
    [Indexsize] BIGINT         NULL
);

