CREATE TABLE [dbo].[MyCarwaleServiceDetails] (
    [ID]             NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]     NUMERIC (18)    NOT NULL,
    [MyCarwaleCarId] NUMERIC (18)    NOT NULL,
    [ServiceTypeId]  NUMERIC (18)    NOT NULL,
    [ServiceDate]    DATETIME        NOT NULL,
    [ServiceKm]      NUMERIC (18)    NULL,
    [ServiceAmount]  DECIMAL (18, 2) CONSTRAINT [DF_MyCarwaleServiceDetails_ServiceAmount] DEFAULT (0) NULL,
    [Workshop]       VARCHAR (50)    NULL,
    [BillNo]         VARCHAR (50)    NULL,
    [Comments]       VARCHAR (300)   NULL,
    [IsActive]       BIT             CONSTRAINT [DF_MyCarwaleServiceDetails_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_MyCarwaleServiceDetails] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

