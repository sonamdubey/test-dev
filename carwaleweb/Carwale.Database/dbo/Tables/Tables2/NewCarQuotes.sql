CREATE TABLE [dbo].[NewCarQuotes] (
    [Id]              NUMERIC (18) IDENTITY (14925504, 1) NOT FOR REPLICATION NOT NULL,
    [NewCarInqId]     NUMERIC (18) NOT NULL,
    [ExShowroomPrice] NUMERIC (18) NOT NULL,
    [RTO]             NUMERIC (18) NOT NULL,
    [Insurance]       NUMERIC (18) NOT NULL,
    [IsBooked]        BIT          CONSTRAINT [DF_New_NewCarQuotes_IsBooked] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_NewCarQuotes_New] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_NewCarQuotes__NewCarInqId]
    ON [dbo].[NewCarQuotes]([NewCarInqId] ASC)
    INCLUDE([ExShowroomPrice], [RTO], [Insurance]);

