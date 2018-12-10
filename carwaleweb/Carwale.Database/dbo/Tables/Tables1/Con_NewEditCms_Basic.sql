﻿CREATE TABLE [dbo].[Con_NewEditCms_Basic] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId]      NUMERIC (18)   NULL,
    [Title]           VARCHAR (250)  NULL,
    [Url]             VARCHAR (200)  NULL,
    [DisplayDate]     DATETIME       NULL,
    [AuthorName]      VARCHAR (100)  NULL,
    [AuthorId]        NUMERIC (18)   CONSTRAINT [DF_Con_NewEditCms_Basic_AuthorId] DEFAULT ((-1)) NOT NULL,
    [Description]     VARCHAR (8000) NULL,
    [IsActive]        BIT            CONSTRAINT [DF_Con_NewEditCms_Basic_IsActive] DEFAULT ((1)) NULL,
    [EnteredBy]       NUMERIC (18)   CONSTRAINT [DF_Con_NewEditCms_Basic_EnteredBy] DEFAULT ((-1)) NOT NULL,
    [EntryDate]       DATETIME       CONSTRAINT [DF_Con_NewEditCms_Basic_EntryDate] DEFAULT (getdate()) NOT NULL,
    [IsPublished]     BIT            CONSTRAINT [DF_Con_NewEditCms_Basic_IsPublished] DEFAULT ((0)) NULL,
    [PublishedDate]   DATETIME       NULL,
    [LastUpdatedTime] DATETIME       CONSTRAINT [DF_Con_NewEditCms_Basic_LastUpdatedTime] DEFAULT (getdate()) NULL,
    [LastUpdatedBy]   NUMERIC (18)   NULL,
    [ShowGallery]     BIT            CONSTRAINT [DF_Con_NewEditCms_Basic_ShowGallery] DEFAULT ((1)) NULL,
    [ForumThreadId]   INT            NULL,
    [Views]           INT            CONSTRAINT [DF_Con_NewEditCms_Basic_Views] DEFAULT ((0)) NOT NULL,
    [MainImageSet]    BIT            CONSTRAINT [DF_Con_NewEditCms_Basic_MainImageSet] DEFAULT ((0)) NOT NULL,
    [RoadTestId]      INT            NULL,
    [IsUpdated]       BIT            CONSTRAINT [DF_Con_NewEditCms_Basic_IsUpdated] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Con_NewEditCms_Basic] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

