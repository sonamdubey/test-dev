CREATE TABLE [dbo].[MyGarageFuelLogger] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MyGarageId]      NUMERIC (18)   NOT NULL,
    [OdometerReading] NUMERIC (18)   NOT NULL,
    [FilledOn]        DATETIME       NOT NULL,
    [Quantity]        DECIMAL (5, 2) NOT NULL,
    [TotalCost]       DECIMAL (9, 2) NOT NULL,
    [FuelPumpName]    VARCHAR (100)  NULL,
    [FullTank]        BIT            CONSTRAINT [DF_MyGarageFuelLogger_FullTank] DEFAULT ((0)) NOT NULL,
    [CreatedOn]       DATETIME       NOT NULL,
    [IsActive]        BIT            CONSTRAINT [DF_MyGarageFuelLogger_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_MyGarageFuelLogger] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

