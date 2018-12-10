CREATE TABLE [dbo].[Microsite_EventImages] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [EventId]         INT           NULL,
    [HostUrl]         VARCHAR (50)  NULL,
    [OriginalImgPath] VARCHAR (300) NULL,
    [AttachFileName]  VARCHAR (500) NULL,
    [CreatedOn]       DATETIME      CONSTRAINT [DF_Microsite_EventImages_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]       DATETIME      NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Microsite_EventImages_IsActive] DEFAULT ((0)) NULL
);

