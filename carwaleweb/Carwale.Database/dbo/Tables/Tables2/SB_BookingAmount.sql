CREATE TABLE [dbo].[SB_BookingAmount] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarId]         NUMERIC (18) NULL,
    [BookingAmount] NUMERIC (18) NULL,
    CONSTRAINT [PK_SB_BookingAmount] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

