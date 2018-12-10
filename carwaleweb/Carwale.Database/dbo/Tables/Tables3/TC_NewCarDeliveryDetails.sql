CREATE TABLE [dbo].[TC_NewCarDeliveryDetails] (
    [TC_NewCarDeliveryDetailsId] INT           IDENTITY (1, 1) NOT NULL,
    [TC_NewCarInquiriesId]       INT           NULL,
    [PanNumber]                  VARCHAR (100) NULL,
    [ChassisNumber]              VARCHAR (100) NULL,
    [EngineNumber]               VARCHAR (100) NULL,
    [InsuranceCoverNumber]       VARCHAR (100) NULL,
    [InvoiceNumber]              VARCHAR (100) NULL,
    [TC_UserId]                  INT           NULL,
    [DeliveryEventDate]          DATETIME      CONSTRAINT [TC_NewCarDeliveryDetails_DeliveryDate] DEFAULT (getdate()) NULL,
    [RegistrationNo]             VARCHAR (100) NULL,
    [DeliveryDate]               DATETIME      NULL,
    CONSTRAINT [PK_TC_NewCarDeliveryDetailsId] PRIMARY KEY NONCLUSTERED ([TC_NewCarDeliveryDetailsId] ASC)
);

