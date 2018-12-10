CREATE TABLE [dbo].[FOB_BookingCancellation] (
    [Id]                NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BRId]              NUMERIC (18)    NULL,
    [Amount]            NUMERIC (18, 2) NULL,
    [CancelledDate]     DATETIME        NULL,
    [CancelRequestDate] DATETIME        NULL,
    [Reason]            VARCHAR (500)   NULL,
    [IsCancellation]    BIT             CONSTRAINT [DF_FOB_BookingCancellation_IsCancellation] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_FOB_BookingCancellation] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

