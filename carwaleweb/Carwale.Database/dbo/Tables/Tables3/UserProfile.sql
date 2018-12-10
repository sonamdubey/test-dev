CREATE TABLE [dbo].[UserProfile] (
    [Id]                  NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [UserId]              NUMERIC (18)   NOT NULL,
    [AboutMe]             VARCHAR (1000) NULL,
    [AvtarPhoto]          VARCHAR (50)   NULL,
    [ThumbNail]           VARCHAR (50)   NULL,
    [RealPhoto]           VARCHAR (50)   NULL,
    [IsAvtarApproved]     BIT            CONSTRAINT [DF_UserProfile_IsAvtarApproved] DEFAULT (0) NOT NULL,
    [IsRealApproved]      BIT            CONSTRAINT [DF_UserProfile_IsRealApproved] DEFAULT (0) NOT NULL,
    [ForumLastLogin]      DATETIME       NULL,
    [Signature]           VARCHAR (500)  NULL,
    [HandleName]          VARCHAR (50)   NULL,
    [IsUpdated]           BIT            CONSTRAINT [DF_UserProfile_IsUpdated] DEFAULT ((0)) NOT NULL,
    [DOB]                 DATETIME       NULL,
    [JoiningDate]         DATETIME       CONSTRAINT [DF_UserProfile_JoiningDate] DEFAULT (getdate()) NULL,
    [ThanksReceived]      NUMERIC (18)   NULL,
    [IsReplicated]        BIT            CONSTRAINT [DF__UserProfi__IsRep__5837A100] DEFAULT ((0)) NULL,
    [HostURL]             VARCHAR (100)  DEFAULT ('img.carwale.com') NULL,
    [ForumPosts]          NUMERIC (18)   NULL,
    [StatusId]            SMALLINT       CONSTRAINT [DF_UserProfileStatusId] DEFAULT ((1)) NULL,
    [SmallUrl]            VARCHAR (100)  NULL,
    [ThumbnailUrl]        VARCHAR (100)  NULL,
    [MediumUrl]           VARCHAR (100)  NULL,
    [DirectoryPath]       VARCHAR (150)  NULL,
    [AvtOriginalImgPath]  VARCHAR (150)  NULL,
    [RealOriginalImgPath] VARCHAR (150)  NULL,
    [originalImgPath]     VARCHAR (250)  NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UserProfile]([UserId] ASC)
    INCLUDE([HandleName]);

