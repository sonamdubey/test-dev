CREATE TABLE [dbo].[MG_Requests] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]      NUMERIC (18)  NOT NULL,
    [VolumeId]        NUMERIC (18)  NOT NULL,
    [PaymentMode]     SMALLINT      NOT NULL,
    [Copies]          SMALLINT      CONSTRAINT [DF_MG_Requests_Copies] DEFAULT ((1)) NOT NULL,
    [Paid]            BIT           CONSTRAINT [DF_MG_Requests_Paid] DEFAULT ((0)) NOT NULL,
    [DeliveryStatus]  SMALLINT      CONSTRAINT [DF_MG_Requests_DeliveryStatus] DEFAULT ((-1)) NOT NULL,
    [RequestDateTime] DATETIME      NULL,
    [Amount]          NUMERIC (18)  CONSTRAINT [DF_MG_Requests_Amount] DEFAULT ((0)) NOT NULL,
    [CancelReason]    VARCHAR (500) NULL,
    CONSTRAINT [PK_MG_Requests] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

