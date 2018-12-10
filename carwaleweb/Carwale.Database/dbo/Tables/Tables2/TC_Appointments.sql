CREATE TABLE [dbo].[TC_Appointments] (
    [TC_AppointmentsId]    INT           IDENTITY (1, 1) NOT NULL,
    [TC_LeadId]            INT           NULL,
    [EntryDate]            DATETIME      CONSTRAINT [DF_TC_Appointments_EntryDate] DEFAULT (getdate()) NOT NULL,
    [VisitDate]            DATETIME      NULL,
    [VisitTime]            TIME (7)      NULL,
    [Purpose]              VARCHAR (MAX) NULL,
    [LastUpdatedDate]      DATETIME      CONSTRAINT [DF_TC_Appointments_LastUpdatedDate] DEFAULT (getdate()) NOT NULL,
    [PqCompletedDate]      DATETIME      NULL,
    [TC_UsersId]           INT           NULL,
    [TC_LeadDispositionId] TINYINT       NULL,
    CONSTRAINT [PK_TC_AppointmentsId] PRIMARY KEY NONCLUSTERED ([TC_AppointmentsId] ASC),
    CONSTRAINT [DF_TC_TC_Appointments_TC_Users] FOREIGN KEY ([TC_UsersId]) REFERENCES [dbo].[TC_Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_DispositionLog_TC_UsersId]
    ON [dbo].[TC_Appointments]([TC_UsersId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Appointments_Tc_leadId]
    ON [dbo].[TC_Appointments]([TC_LeadId] ASC);

