CREATE TABLE [dbo].[LTS_ConversionTracking_Hist] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [StdId]         NUMERIC (18)  NULL,
    [Type]          BIGINT        NULL,
    [CustomerId]    BIGINT        NULL,
    [InquiryId]     NUMERIC (18)  NULL,
    [EntryDateTime] DATETIME      NULL,
    [Tag]           VARCHAR (100) NULL,
    [IPAddress]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_Conversion_tracking] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

