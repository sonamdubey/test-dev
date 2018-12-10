CREATE TABLE [dbo].[CV_MobileEmailPair] (
    [EmailId]          VARCHAR (100) NOT NULL,
    [MobileNo]         VARCHAR (20)  NOT NULL,
    [sourceId]         TINYINT       NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_CV_MobileEmailPair_CreatedOn] DEFAULT (getdate()) NULL,
    [CV_PendingListID] BIGINT        NULL,
    CONSTRAINT [PK_VCMobEmail] PRIMARY KEY CLUSTERED ([EmailId] ASC, [MobileNo] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_MobileCV_MobileEmailPair]
    ON [dbo].[CV_MobileEmailPair]([MobileNo] ASC);

