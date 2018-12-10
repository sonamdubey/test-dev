CREATE TABLE [Mobile].[GCMAlerts] (
    [GCMAlertId]  INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (200) NOT NULL,
    [AlertTypeId] INT           NOT NULL,
    [DetailURL]   VARCHAR (200) NOT NULL,
    [IsFeatured]  BIT           NULL,
    [SmallPicURL] VARCHAR (200) NULL,
    [LargePicURL] VARCHAR (200) NULL,
    [CreatedBy]   INT           NOT NULL,
    [CreatedOn]   DATETIME      NOT NULL
);

