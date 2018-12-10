CREATE TABLE [dbo].[TC_ServiceStatus] (
    [TC_ServiceStatusId] INT          IDENTITY (1, 1) NOT NULL,
    [ServiceStatus]      VARCHAR (25) NULL,
    [IsActive]           BIT          NULL
);

