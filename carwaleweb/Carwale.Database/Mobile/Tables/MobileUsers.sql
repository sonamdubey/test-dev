CREATE TABLE [Mobile].[MobileUsers] (
    [MobileUserId]              INT           IDENTITY (1, 1) NOT NULL,
    [IMEICode]                  VARCHAR (50)  NOT NULL,
    [Name]                      VARCHAR (100) NULL,
    [EMailId]                   VARCHAR (50)  NULL,
    [ContactNo]                 VARCHAR (50)  NULL,
    [OSType]                    INT           NOT NULL,
    [GCMRegId]                  VARCHAR (250) NULL,
    [CreatedOn]                 DATETIME      NULL,
    [LastUpdatedOn]             DATETIME      CONSTRAINT [DF_MobileUsers_LastUpdatedOn_1] DEFAULT (getdate()) NOT NULL,
    [FCMTokenId]                VARCHAR (250) NULL,
    [UserActiveLastCheckedDate] DATETIME      NULL,
    CONSTRAINT [PK_MobileUsers_2016] PRIMARY KEY CLUSTERED ([MobileUserId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_MobileUsers_IMEICode]
    ON [Mobile].[MobileUsers]([IMEICode] ASC);

