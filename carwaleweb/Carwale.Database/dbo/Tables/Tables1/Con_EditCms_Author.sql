CREATE TABLE [dbo].[Con_EditCms_Author] (
    [Authorid]          NUMERIC (18)  NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [Designation]       VARCHAR (50)  NULL,
    [PicName]           VARCHAR (50)  NULL,
    [BriefProfile]      VARCHAR (300) NULL,
    [FullProfile]       VARCHAR (MAX) NULL,
    [SequenceOrder]     TINYINT       NULL,
    [IsReplicated]      BIT           CONSTRAINT [DF__Con_EditC__IsRep__3E77CEFD] DEFAULT ((0)) NULL,
    [HostURL]           VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    [AuthorName]        VARCHAR (50)  NULL,
    [EmailId]           VARCHAR (50)  NULL,
    [FacebookProfile]   VARCHAR (100) NULL,
    [GooglePlusProfile] VARCHAR (100) NULL,
    [LinkedInProfile]   VARCHAR (100) NULL,
    [TwitterProfile]    VARCHAR (100) NULL,
    [ProfileImgUrl]     VARCHAR (50)  NULL,
    [MaskingName]       VARCHAR (50)  NULL,
    [ImageName]         VARCHAR (50)  NULL,
    CONSTRAINT [PK_Con_EditCms_Author] PRIMARY KEY CLUSTERED ([Authorid] ASC)
);

