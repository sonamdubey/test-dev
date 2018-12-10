CREATE TABLE [dbo].[Absure_Appointments] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [AbsureCarId]   INT           NULL,
    [EntryDate]     DATETIME      NULL,
    [UserId]        INT           NULL,
    [ScheduledDate] DATE          NULL,
    [ScheduledTime] VARCHAR (100) NULL,
    [Reason]        VARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

