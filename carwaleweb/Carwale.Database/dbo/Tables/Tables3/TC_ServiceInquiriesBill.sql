CREATE TABLE [dbo].[TC_ServiceInquiriesBill] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [TC_ServiceInquiriesId] INT           NULL,
    [Discount]              INT           NULL,
    [ServiceTax]            FLOAT (53)    NULL,
    [TotalPrice]            INT           NULL,
    [TC_ServiceStatusId]    INT           NULL,
    [FilePath]              VARCHAR (200) NULL
);

