CREATE TABLE [dbo].[TC_BookingInsurance_2011] (
    [TC_BookingInsurance_Id] INT          IDENTITY (1, 1) NOT NULL,
    [TC_CarBooking_Id]       INT          NOT NULL,
    [TC_InsuranceCompany_Id] INT          NULL,
    [InsuranceType]          CHAR (15)    NULL,
    [PolicyNumber]           VARCHAR (20) NULL,
    [PolicyStartDate]        DATE         NULL,
    [PolicyEndDate]          DATE         NULL,
    [SumInsured]             INT          NULL,
    [IsActive]               BIT          NULL,
    [EntryDate]              DATETIME     NULL,
    [ModifiedDate]           DATE         NULL,
    [ModifiedBy]             INT          NULL
);

