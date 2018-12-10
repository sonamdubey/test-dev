CREATE TABLE [dbo].[ReviewAbusedDetails] (
    [ID]               NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerReviewId] NUMERIC (18)  NULL,
    [Comments]         VARCHAR (500) NULL,
    [ReportedBy]       NUMERIC (18)  NULL,
    [ReportedDateTime] DATETIME      NULL,
    [IsVerified]       BIT           CONSTRAINT [DF_ReviewAbusedDetails_IsVerified] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ReviewAbusedDetails] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

