CREATE TABLE [dba].[DBServerHWStats] (
    [Logical CPU Count]    INT      NOT NULL,
    [Hyperthread Ratio]    INT      NOT NULL,
    [Physical CPU Count]   INT      NULL,
    [Physical Memory (MB)] BIGINT   NULL,
    [sqlserver_start_time] DATETIME NOT NULL
);

