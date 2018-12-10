CREATE TABLE [Mobile].[MobileNotificationLog] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [SubsMasterId]     INT           NOT NULL,
    [StartDate]        DATETIME      NULL,
    [enddate]          DATETIME      NULL,
    [Remarks]          VARCHAR (80)  NULL,
    [Title]            VARCHAR (250) NULL,
    [AndroidUserCount] INT           NULL,
    [IOSUserCount]     INT           NULL,
    [MakeId]           INT           NULL,
    [ModelId]          INT           NULL,
    [CityId]           INT           NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_Update_MobileNotificationLog_SubsMasterId_EndDate]
    ON [Mobile].[MobileNotificationLog]([SubsMasterId] ASC, [enddate] ASC);

