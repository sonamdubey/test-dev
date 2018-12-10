CREATE TABLE [dbo].[TC_SpecialUsersOld] (
    [TC_SpecialUsersId]   INT                 IDENTITY (1, 1) NOT NULL,
    [UserName]            VARCHAR (50)        NOT NULL,
    [Email]               VARCHAR (100)       NOT NULL,
    [Password]            VARCHAR (20)        NOT NULL,
    [Mobile]              VARCHAR (10)        NULL,
    [Sex]                 VARCHAR (6)         NULL,
    [DOB]                 DATE                NULL,
    [Address]             VARCHAR (200)       NULL,
    [MakeId]              SMALLINT            NULL,
    [IsActive]            BIT                 CONSTRAINT [DF_TC_SpecialUsers_IsActive] DEFAULT ((1)) NOT NULL,
    [IsFirstTimeLoggedIn] BIT                 CONSTRAINT [DF_TC_SpecialUsers_IsFirstTimeLoggedIn] DEFAULT ((1)) NULL,
    [GCMRegistrationId]   VARCHAR (250)       NULL,
    [EntryDate]           DATETIME            CONSTRAINT [DF_TC_SpecialUsers_EntryDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]        DATETIME            NULL,
    [ModifiedBy]          INT                 NULL,
    [HierId]              [sys].[hierarchyid] NULL,
    [lvl]                 AS                  ([HierId].[GetLevel]()) PERSISTED,
    [NodeCode]            AS                  ([HierId].[ToString]()) PERSISTED,
    CONSTRAINT [PK_TC_SpecialUsers] PRIMARY KEY CLUSTERED ([TC_SpecialUsersId] ASC),
    CONSTRAINT [UQ__TC_SpecialUsers_Email] UNIQUE NONCLUSTERED ([Email] ASC)
);

