CREATE TABLE [dbo].[LTS_ConversionTracking] (
    [Id]            NUMERIC (18)  IDENTITY (11460788, 1) NOT FOR REPLICATION NOT NULL,
    [StdId]         NUMERIC (18)  NULL,
    [Type]          BIGINT        NULL,
    [CustomerId]    BIGINT        NULL,
    [InquiryId]     NUMERIC (18)  NULL,
    [EntryDateTime] DATETIME      NULL,
    [Tag]           VARCHAR (100) NULL,
    [IPAddress]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_Conversion_tracking2] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_LTS_ConversionTracking__CustomerId]
    ON [dbo].[LTS_ConversionTracking]([CustomerId] ASC);

