CREATE TABLE [dbo].[dailylog30days] (
    [dailylogid]  INT           IDENTITY (1, 1) NOT NULL,
    [asondate]    DATE          NULL,
    [sellertype]  SMALLINT      NULL,
    [inquiryid]   INT           NULL,
    [price]       DECIMAL (18)  NULL,
    [cityid]      INT           NULL,
    [cityname]    VARCHAR (100) NULL,
    [dealerid]    INT           NULL,
    [packagetype] SMALLINT      NULL,
    PRIMARY KEY CLUSTERED ([dailylogid] ASC)
);

