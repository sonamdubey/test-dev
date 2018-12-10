CREATE TABLE [Mobile].[MobileUsers_Till2015] (
    [MobileUserId]  INT           IDENTITY (1, 1) NOT NULL,
    [IMEICode]      VARCHAR (50)  NOT NULL,
    [Name]          VARCHAR (100) NULL,
    [EMailId]       VARCHAR (50)  NULL,
    [ContactNo]     VARCHAR (50)  NULL,
    [OSType]        INT           NOT NULL,
    [GCMRegId]      VARCHAR (250) NULL,
    [CreatedOn]     DATETIME      NULL,
    [LastUpdatedOn] DATETIME      NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [ix_MobileUsers_Till2015_MobileUserId]
    ON [Mobile].[MobileUsers_Till2015]([MobileUserId] ASC);

