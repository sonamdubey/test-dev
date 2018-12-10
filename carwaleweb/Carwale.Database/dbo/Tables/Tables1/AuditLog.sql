CREATE TABLE [dbo].[AuditLog] (
    [sessionid]               SMALLINT       NOT NULL,
    [login_time]              DATETIME       NOT NULL,
    [host_name]               NVARCHAR (128) NULL,
    [program_name]            NCHAR (128)    NOT NULL,
    [login_name]              NVARCHAR (128) NOT NULL,
    [nt_domain]               NCHAR (128)    NOT NULL,
    [nt_user_name]            NVARCHAR (128) NULL,
    [last_request_start_time] DATETIME       NOT NULL,
    [last_request_end_time]   DATETIME       NULL,
    [Database_Name]           NVARCHAR (128) NULL,
    [sqlstatement]            NVARCHAR (MAX) NULL,
    [Command_Executed]        NCHAR (16)     NOT NULL,
    [comments]                VARCHAR (100)  NULL
);

