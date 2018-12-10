CREATE TABLE [dbo].[TC_MappingServiceInqBillDetails] (
    [Id]                  INT          IDENTITY (1, 1) NOT NULL,
    [TC_ServiceInqBillId] INT          NULL,
    [Category]            VARCHAR (50) NULL,
    [PartName]            VARCHAR (50) NULL,
    [Quantity]            TINYINT      NULL,
    [UnitPrice]           INT          NULL
);

