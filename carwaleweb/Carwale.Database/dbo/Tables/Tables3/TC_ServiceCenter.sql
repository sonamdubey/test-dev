CREATE TABLE [dbo].[TC_ServiceCenter] (
    [TC_ServiceCenterId] INT           IDENTITY (1, 1) NOT NULL,
    [ServiceCenterName]  VARCHAR (50)  NOT NULL,
    [EmailId]            VARCHAR (60)  NOT NULL,
    [Address]            VARCHAR (250) NOT NULL,
    [AreaId]             INT           NULL,
    [CityId]             INT           NOT NULL,
    [ZipCode]            VARCHAR (10)  NULL,
    [PhoneNo]            VARCHAR (15)  NULL,
    [BranchId]           INT           NULL,
    [CreatedBy]          INT           NULL,
    [CreatedDate]        DATETIME      NULL,
    [LastModifiedDate]   DATETIME      NULL,
    [LastModifiedBy]     INT           NULL,
    [IsActive]           BIT           NULL
);

