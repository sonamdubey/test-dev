CREATE TABLE [dbo].[TC_BookingWarranties] (
    [TC_BookingWarranties_Id] INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]                INT          NOT NULL,
    [WarrantyName]            VARCHAR (50) NOT NULL,
    [IsActive]                BIT          CONSTRAINT [DF_TC_BookingWarranties_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]               DATETIME     CONSTRAINT [DF_TC_BookingWarranties_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]            DATETIME     NULL,
    [ModifiedBy]              INT          NULL,
    CONSTRAINT [PK_TC_BookingWarranties] PRIMARY KEY CLUSTERED ([TC_BookingWarranties_Id] ASC)
);

