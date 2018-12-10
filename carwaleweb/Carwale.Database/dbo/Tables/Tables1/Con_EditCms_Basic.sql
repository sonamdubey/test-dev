CREATE TABLE [dbo].[Con_EditCms_Basic] (
    [Id]                        NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId]                NUMERIC (18)   NULL,
    [Title]                     VARCHAR (250)  NULL,
    [Url]                       VARCHAR (300)  NULL,
    [DisplayDate]               DATETIME       NULL,
    [AuthorName]                VARCHAR (100)  NULL,
    [AuthorId]                  NUMERIC (18)   CONSTRAINT [DF_Con_EditCms_Basic_AuthorId] DEFAULT ((-1)) NOT NULL,
    [Description]               VARCHAR (8000) NULL,
    [IsActive]                  BIT            CONSTRAINT [DF_Con_EditCms_Basic_IsActive] DEFAULT ((1)) NULL,
    [EnteredBy]                 NUMERIC (18)   CONSTRAINT [DF_Con_EditCms_Basic_EnteredBy] DEFAULT ((-1)) NOT NULL,
    [EntryDate]                 DATETIME       CONSTRAINT [DF_Con_EditCms_Basic_EntryDate] DEFAULT (getdate()) NOT NULL,
    [IsPublished]               BIT            CONSTRAINT [DF_Con_EditCms_Basic_IsPublished] DEFAULT ((0)) NULL,
    [PublishedDate]             DATETIME       NULL,
    [LastUpdatedTime]           DATETIME       CONSTRAINT [DF_Con_EditCms_Basic_LastUpdatedTime] DEFAULT (getdate()) NULL,
    [LastUpdatedBy]             NUMERIC (18)   NULL,
    [ShowGallery]               BIT            CONSTRAINT [DF_Con_EditCms_Basic_ShowGallery] DEFAULT ((1)) NULL,
    [ForumThreadId]             INT            NULL,
    [Views]                     INT            CONSTRAINT [DF_Con_EditCms_Basic_Views] DEFAULT ((0)) NOT NULL,
    [MainImageSet]              BIT            CONSTRAINT [DF_Con_EditCms_Basic_MainImageSet] DEFAULT ((0)) NOT NULL,
    [RoadTestId]                INT            NULL,
    [IsUpdated]                 BIT            CONSTRAINT [DF_Con_EditCms_Basic_IsUpdated] DEFAULT ((0)) NOT NULL,
    [HostURL]                   VARCHAR (100)  NULL,
    [IsReplicated]              BIT            NULL,
    [HasCustomImg]              BIT            CONSTRAINT [DF_Con_EditCms_Basic_HasCustomImg] DEFAULT ((0)) NULL,
    [IsDealerFriendly]          BIT            DEFAULT ((0)) NULL,
    [MainImgCaption]            VARCHAR (250)  NULL,
    [StickyFromDate]            DATETIME       NULL,
    [StickyToDate]              DATETIME       NULL,
    [IsSticky]                  BIT            NULL,
    [ReasonToUnpublish]         VARCHAR (500)  NULL,
    [UnPublishedBy]             BIGINT         NULL,
    [UnPublishedDate]           DATETIME       NULL,
    [IsFeatured]                BIT            DEFAULT ((0)) NULL,
    [IsCompatibleForNewsLetter] BIT            CONSTRAINT [DF_Con_EditCms_Basic_IsCompatibleForNewsLetter] DEFAULT ((0)) NULL,
    [SocialMediaLine]           VARCHAR (120)  NULL,
    [ApplicationID]             TINYINT        NULL,
    [BWMigratedId]              INT            NULL,
    [BWOldCategoryId]           INT            NULL,
    [PhotoCredit]               VARCHAR (250)  NULL,
    [IsNotified]                BIT            DEFAULT ((0)) NULL,
    [PushNotification]          BIT            NULL,
    [FacebookLikecount]         INT            NULL,
    [FacebookCommentCount]      INT            NULL,
    [MainImagePath]             VARCHAR (300)  NULL,
    CONSTRAINT [PK_Con_EditCms_Basic] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Basic_CategoryId_IsActive_IsPublished]
    ON [dbo].[Con_EditCms_Basic]([CategoryId] ASC, [IsActive] ASC, [IsPublished] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Basic_DisplayDate]
    ON [dbo].[Con_EditCms_Basic]([DisplayDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Basic_IsActive_IsPublished_DisplayDate]
    ON [dbo].[Con_EditCms_Basic]([IsActive] ASC, [IsPublished] ASC, [DisplayDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Basic_AuthorId]
    ON [dbo].[Con_EditCms_Basic]([AuthorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Con_EditCms_Basic_ApplicationID]
    ON [dbo].[Con_EditCms_Basic]([ApplicationID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationID_Con_EditCms_Basic]
    ON [dbo].[Con_EditCms_Basic]([ApplicationID] ASC, [IsActive] ASC, [IsPublished] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Con_EditCms_Basic__IsPublished__ApplicationID__CategoryId]
    ON [dbo].[Con_EditCms_Basic]([IsPublished] ASC, [ApplicationID] ASC, [CategoryId] ASC)
    INCLUDE([Id], [DisplayDate]);

