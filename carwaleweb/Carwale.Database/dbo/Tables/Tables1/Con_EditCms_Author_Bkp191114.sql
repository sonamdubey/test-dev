CREATE TABLE [dbo].[Con_EditCms_Author_Bkp191114] (
    [Authorid]          NUMERIC (18)  NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [Designation]       VARCHAR (50)  NULL,
    [PicName]           VARCHAR (50)  NULL,
    [BriefProfile]      VARCHAR (300) NULL,
    [FullProfile]       VARCHAR (MAX) NULL,
    [SequenceOrder]     TINYINT       NULL,
    [IsReplicated]      BIT           NULL,
    [HostURL]           VARCHAR (100) NULL,
    [AuthorName]        VARCHAR (50)  NULL,
    [EmailId]           VARCHAR (50)  NULL,
    [FacebookProfile]   VARCHAR (100) NULL,
    [GooglePlusProfile] VARCHAR (100) NULL,
    [LinkedInProfile]   VARCHAR (100) NULL,
    [TwitterProfile]    VARCHAR (100) NULL,
    [ProfileImgUrl]     VARCHAR (50)  NULL,
    [MaskingName]       VARCHAR (50)  NULL,
    [ImageName]         VARCHAR (50)  NULL
);

