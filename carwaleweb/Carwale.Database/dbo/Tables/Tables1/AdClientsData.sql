CREATE TABLE [dbo].[AdClientsData] (
    [Id]         NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AdClientId] NUMERIC (18)    NOT NULL,
    [Impression] NUMERIC (18)    NOT NULL,
    [Clicks]     NUMERIC (18)    NOT NULL,
    [Revenue]    NUMERIC (18, 2) NOT NULL,
    [InsertDate] DATETIME        NOT NULL,
    [isActive]   BIT             CONSTRAINT [DF_AdClientsData_isActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_AdClientsData] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

