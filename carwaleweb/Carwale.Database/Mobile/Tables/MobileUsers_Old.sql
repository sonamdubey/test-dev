CREATE TABLE [Mobile].[MobileUsers_Old] (
    [MobileUserId]  INT           IDENTITY (1, 1) NOT NULL,
    [IMEICode]      VARCHAR (50)  NOT NULL,
    [Name]          VARCHAR (100) NULL,
    [EMailId]       VARCHAR (50)  NULL,
    [ContactNo]     VARCHAR (50)  NULL,
    [OSType]        INT           NOT NULL,
    [GCMRegId]      VARCHAR (250) NULL,
    [CreatedOn]     DATETIME      NULL,
    [LastUpdatedOn] DATETIME      CONSTRAINT [DF_MobileUsers_LastUpdatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MobileUsers] PRIMARY KEY CLUSTERED ([MobileUserId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_MobileUsers_IMEICode]
    ON [Mobile].[MobileUsers_Old]([IMEICode] ASC);

