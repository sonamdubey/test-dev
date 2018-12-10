CREATE TABLE [dbo].[IClDialerCallUploadDeatails] (
    [DownloadDatetime] DATETIME NOT NULL,
    [FromDate]         DATETIME NULL,
    [ToDate]           DATETIME NULL,
    [Records]          BIGINT   NULL,
    CONSTRAINT [PK_IClDialerCallUploadDeatails] PRIMARY KEY CLUSTERED ([DownloadDatetime] ASC)
);

