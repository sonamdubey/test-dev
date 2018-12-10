CREATE TABLE [dbo].[TempFollowup] (
    [ID]             NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [InquiryType]    SMALLINT       NULL,
    [InquiryId]      NUMERIC (18)   NULL,
    [StatusId]       SMALLINT       NULL,
    [DealerResponse] VARCHAR (1500) NULL,
    [Comments]       VARCHAR (1500) NULL,
    [EntryDate]      DATETIME       NOT NULL,
    CONSTRAINT [PK_TempFollowup] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

