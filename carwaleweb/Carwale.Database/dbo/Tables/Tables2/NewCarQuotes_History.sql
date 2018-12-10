CREATE TABLE [dbo].[NewCarQuotes_History] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [NewCarInqId]     NUMERIC (18) NOT NULL,
    [ExShowroomPrice] NUMERIC (18) NOT NULL,
    [RTO]             NUMERIC (18) NOT NULL,
    [Insurance]       NUMERIC (18) NOT NULL,
    [IsBooked]        BIT          CONSTRAINT [DF_NewCarQuotes_IsBooked] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_NewCarQuotes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

