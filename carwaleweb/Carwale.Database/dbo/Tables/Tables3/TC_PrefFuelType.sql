CREATE TABLE [dbo].[TC_PrefFuelType] (
    [TC_PrefFuelTypeId]   INT      IDENTITY (1, 1) NOT NULL,
    [TC_BuyerInquiriesId] INT      NULL,
    [FuelType]            INT      NULL,
    [CreatedOn]           DATETIME CONSTRAINT [DF_TC_PrefFuelType_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [IsActive]            BIT      CONSTRAINT [DF_Tc_PrefFuelType_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_PrefFuelType_id] PRIMARY KEY NONCLUSTERED ([TC_PrefFuelTypeId] ASC)
);

