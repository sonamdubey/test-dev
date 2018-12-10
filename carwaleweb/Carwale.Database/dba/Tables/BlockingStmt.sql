CREATE TABLE [dba].[BlockingStmt] (
    [ID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [lock type]    NVARCHAR (60)  NOT NULL,
    [database]     NVARCHAR (128) NULL,
    [blk object]   BIGINT         NULL,
    [lock req]     NVARCHAR (60)  NOT NULL,
    [waiter sid]   INT            NOT NULL,
    [wait time]    BIGINT         NULL,
    [waiter_batch] NVARCHAR (MAX) NULL,
    [waiter_stmt]  NVARCHAR (MAX) NULL,
    [blocker sid]  SMALLINT       NULL,
    [blocker_stmt] NVARCHAR (MAX) NULL,
    [CreatedOn]    DATETIME       CONSTRAINT [DF__BlockingS__Creat__315F61AF] DEFAULT (getdate()) NULL
);

