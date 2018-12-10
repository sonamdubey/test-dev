CREATE TABLE [NCS].[RManagers] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (100) NOT NULL,
    [Designation]     VARCHAR (100) NULL,
    [MakeId]          NUMERIC (18)  NOT NULL,
    [MakeGroupId]     NUMERIC (18)  NULL,
    [ReportToCurrent] VARCHAR (50)  NULL,
    [ReportTo]        NUMERIC (18)  NULL,
    [EMail]           VARCHAR (50)  NULL,
    [MobileNo]        VARCHAR (50)  NULL,
    [LoginId]         VARCHAR (50)  NULL,
    [Password]        VARCHAR (50)  NULL,
    [NodeCode]        VARCHAR (100) NULL,
    [IsActive]        BIT           NOT NULL,
    [UpdatedDate]     DATETIME      NULL,
    [UpdatedBy]       NUMERIC (18)  NULL,
    [IsExecutive]     BIT           NOT NULL,
    [CityId]          NUMERIC (18)  NULL
);

