CREATE TABLE [dbo].[TC_OrganizationListPayPerCar] (
    [TC_OrganizationListPayPerCarId] INT           IDENTITY (1, 1) NOT NULL,
    [Organization]                   VARCHAR (150) NULL,
    [IsActive]                       BIT           CONSTRAINT [TC_OrganizationListPayPerCar_IsActive] DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([TC_OrganizationListPayPerCarId] ASC),
    CONSTRAINT [TC_OrganizationListPayPerCar_Organization] UNIQUE NONCLUSTERED ([Organization] ASC)
);

