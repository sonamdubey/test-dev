CREATE TABLE [dbo].[UCS_SearchCriteria] (
    [ID]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SessionId]    VARCHAR (100)  NULL,
    [CustomerId]   NUMERIC (18)   NULL,
    [Model]        VARCHAR (1000) NULL,
    [Make]         NUMERIC (18)   NULL,
    [PriceFrom]    NUMERIC (18)   NULL,
    [PriceTo]      NUMERIC (18)   NULL,
    [YearFrom]     NUMERIC (18)   NULL,
    [YearTo]       NUMERIC (18)   NULL,
    [KmFrom]       NUMERIC (18)   NULL,
    [KmTo]         NUMERIC (18)   NULL,
    [City]         NUMERIC (18)   NULL,
    [Dist]         NUMERIC (18)   NULL,
    [St]           NUMERIC (18)   NULL,
    [Li]           NUMERIC (18)   NULL,
    [SearchedDate] DATETIME       NULL,
    CONSTRAINT [PK_NCS_SearchCriteria] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

