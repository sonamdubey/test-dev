CREATE TABLE [dbo].[Nano_Confirmation] (
    [NC_Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]       NUMERIC (18) NULL,
    [ConfirmationDate] DATETIME     NULL,
    CONSTRAINT [PK_Nano_Confirmation] PRIMARY KEY CLUSTERED ([NC_Id] ASC) WITH (FILLFACTOR = 90)
);

