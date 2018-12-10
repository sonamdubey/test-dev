CREATE TABLE [dbo].[OLM_SocialFeed] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [NameOfUser]      VARCHAR (50)   NOT NULL,
    [SocialHandle]    VARCHAR (20)   NULL,
    [SourceOfChannel] SMALLINT       CONSTRAINT [DF_OLM__IsActive] DEFAULT ((1)) NOT NULL,
    [Post]            VARCHAR (1500) NOT NULL,
    [PostDate]        DATETIME       NOT NULL,
    [UploadDate]      DATETIME       CONSTRAINT [DF_OLM_OctaviaComments_UploadDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]       DATETIME       NOT NULL,
    [UrlOfUser]       VARCHAR (200)  NOT NULL,
    [UrlOfUserPic]    VARCHAR (200)  NULL,
    [IsActive]        BIT            NOT NULL,
    [Type]            INT            NOT NULL,
    [ModelId]         INT            NULL,
    CONSTRAINT [PK_OLM_SocialFeed] PRIMARY KEY CLUSTERED ([Id] ASC)
);

