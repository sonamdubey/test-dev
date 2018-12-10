CREATE TABLE [dbo].[CV_PendingList] (
    [ID]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CWICode]       VARCHAR (50)  NOT NULL,
    [CUICode]       VARCHAR (50)  NOT NULL,
    [Email]         VARCHAR (100) NOT NULL,
    [Mobile]        VARCHAR (50)  NOT NULL,
    [EntryDateTime] DATETIME      NOT NULL,
    [SourceId]      TINYINT       NULL,
    [PlatformId]    SMALLINT      NULL,
    CONSTRAINT [PK_CV_PendingList_0616] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CWICodeCV_PendingList_0616]
    ON [dbo].[CV_PendingList]([CWICode] ASC, [Email] ASC, [Mobile] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CV_PendingList_0616_Email]
    ON [dbo].[CV_PendingList]([Email] ASC, [Mobile] ASC, [EntryDateTime] ASC);

