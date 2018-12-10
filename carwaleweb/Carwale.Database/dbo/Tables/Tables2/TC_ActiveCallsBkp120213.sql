CREATE TABLE [dbo].[TC_ActiveCallsBkp120213] (
    [TC_CallsId]      INT           NOT NULL,
    [TC_LeadId]       INT           NULL,
    [CallType]        TINYINT       NOT NULL,
    [TC_UsersId]      INT           NOT NULL,
    [ScheduledOn]     DATETIME      NOT NULL,
    [AlertId]         INT           NULL,
    [LastCallDate]    DATETIME      NULL,
    [LastCallComment] VARCHAR (MAX) NULL
);

