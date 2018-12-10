CREATE TABLE [dbo].[M_AttachedLpaDetails] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [SalesDealerId]    INT           NOT NULL,
    [AttachedFileName] VARCHAR (500) NOT NULL,
    [FileHostUrl]      VARCHAR (100) NOT NULL,
    [UploadedOn]       DATETIME      NULL,
    [UploadedBy]       INT           NOT NULL,
    [HostUrl]          VARCHAR (50)  NULL,
    [OriginalImgPath]  VARCHAR (300) NULL,
    [IsActive]         BIT           DEFAULT ((0)) NULL
);

