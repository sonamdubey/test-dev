CREATE TABLE [dbo].[TC_BookingInsurance] (
    [TC_BookingInsurance_Id] INT          IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]       INT          NOT NULL,
    [TC_InsuranceCompany_Id] INT          NULL,
    [InsuranceType]          CHAR (15)    NULL,
    [PolicyNumber]           VARCHAR (20) NULL,
    [PolicyStartDate]        DATE         NULL,
    [PolicyEndDate]          DATE         NULL,
    [SumInsured]             INT          NULL,
    [IsActive]               BIT          CONSTRAINT [DF_Table_1_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]              DATETIME     CONSTRAINT [DF_TC_BookingInsurance_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]           DATE         NULL,
    [ModifiedBy]             INT          NULL,
    CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED ([TC_BookingInsurance_Id] ASC),
    CONSTRAINT [UNQ_InsuranceCarBookingId] UNIQUE NONCLUSTERED ([TC_CarBooking_Id] ASC)
);

