CREATE TABLE [dbo].[NCS_RManagersOld] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
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
    [IsActive]        BIT           CONSTRAINT [DF_NCS_RManagers_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedDate]     DATETIME      NULL,
    [UpdatedBy]       NUMERIC (18)  NULL,
    [IsExecutive]     BIT           CONSTRAINT [DF_NCS_RManagers_IsExecutive] DEFAULT ((0)) NOT NULL,
    [CityId]          NUMERIC (18)  NULL,
    CONSTRAINT [PK_NCS_RManagers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

