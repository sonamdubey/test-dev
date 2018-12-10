CREATE TABLE [dbo].[PriceLog] (
    [ID]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]    NUMERIC (18) NOT NULL,
    [CityId]       NUMERIC (18) NOT NULL,
    [Price]        NUMERIC (18) NOT NULL,
    [Insurance]    NUMERIC (18) NOT NULL,
    [RTO]          NUMERIC (18) NOT NULL,
    [MetPrice]     NUMERIC (18) NULL,
    [MetInsurance] NUMERIC (18) NULL,
    [MetRTO]       NUMERIC (18) NULL,
    [UpdatedOn]    DATETIME     NOT NULL,
    [UpdatedBy]    INT          NULL,
    CONSTRAINT [PK_PriceLog] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

