CREATE TABLE [dbo].[AbSure_ActivatedWarrantyLog] (
    [AbSure_ActivatedWarrantyId] INT           IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId]        INT           NULL,
    [CustName]                   VARCHAR (150) NULL,
    [Address]                    VARCHAR (500) NULL,
    [Mobile]                     VARCHAR (20)  NULL,
    [AlternatePhone]             VARCHAR (20)  NULL,
    [Email]                      VARCHAR (50)  NULL,
    [EngineNo]                   VARCHAR (50)  NULL,
    [ChassisNo]                  VARCHAR (50)  NULL,
    [IsActive]                   BIT           NULL,
    PRIMARY KEY CLUSTERED ([AbSure_ActivatedWarrantyId] ASC)
);

