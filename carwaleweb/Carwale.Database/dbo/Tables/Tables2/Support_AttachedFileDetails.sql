CREATE TABLE [dbo].[Support_AttachedFileDetails] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [TicketLogId]      INT           NOT NULL,
    [AttachedFileName] VARCHAR (500) NULL,
    [UploadedOn]       DATETIME      NULL,
    [UploadedBy]       INT           NULL,
    [HostUrl]          VARCHAR (50)  NULL,
    [OriginalImgPath]  VARCHAR (300) NULL,
    [IsActive]         BIT           CONSTRAINT [DF__Support_IsActive] DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

