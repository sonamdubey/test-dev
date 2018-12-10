﻿CREATE TABLE [dbo].[TC_SoldRSAPackages] (
    [Id]                        INT           IDENTITY (1, 1) NOT NULL,
    [Name]                      VARCHAR (100) NULL,
    [MobileNo]                  VARCHAR (15)  NULL,
    [Email]                     VARCHAR (100) NULL,
    [BranchId]                  INT           NULL,
    [UserId]                    INT           NULL,
    [Quantity]                  INT           NULL,
    [TC_AvailableRSAPackagesId] INT           NULL,
    [EntryDate]                 DATETIME      DEFAULT (getdate()) NOT NULL,
    [MakeYear]                  DATETIME      DEFAULT ((0)) NOT NULL,
    [VersionId]                 INT           NULL,
    [RegistrationNo]            VARCHAR (20)  NULL,
    [IsAccepted]                BIT           NULL,
    [CBDId]                     NUMERIC (18)  NULL,
    [StartDate]                 DATETIME      NULL,
    [EndDate]                   DATETIME      NULL,
    [Comments]                  VARCHAR (250) NULL,
    [UpdatedOn]                 DATETIME      NULL,
    [UpdatedBy]                 INT           NULL,
    [Address]                   VARCHAR (500) NULL,
    [Kilometers]                INT           NULL,
    [RegistrationType]          INT           NULL,
    [ChassisNo]                 VARCHAR (50)  NULL,
    [EngineNo]                  VARCHAR (50)  NULL,
    [CarFittedWith]             TINYINT       NULL,
    [CityId]                    BIGINT        NULL,
    [AreaId]                    BIGINT        NULL,
    [Amount]                    FLOAT (53)    NULL,
    [ReqRSAPackageId]           INT           NULL,
    [PolicyNo]                  VARCHAR (50)  NULL,
    [IsActivated]               BIT           NULL,
    [ActivationDate]            DATETIME      NULL,
    [ActivatedBy]               INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

