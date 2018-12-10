CREATE TABLE [NCAlert].[MatchedCarVersions] (
    [MatchedCarVersionsId] INT           IDENTITY (1, 1) NOT NULL,
    [PQVersionId]          INT           NULL,
    [MTVersionId]          INT           NULL,
    [MTMakeId]             INT           NULL,
    [MTMakeName]           VARCHAR (50)  NULL,
    [MTModelId]            INT           NULL,
    [MTModelName]          VARCHAR (50)  NULL,
    [MTModelMaskingName]   VARCHAR (50)  NULL,
    [MTVersionName]        VARCHAR (50)  NULL,
    [MTImageUrl]           VARCHAR (250) NULL,
    [MTShowroomPrice]      INT           NULL,
    [MTRTO]                INT           NULL,
    [MTInsurance]          INT           NULL,
    [MTDepotCharges]       INT           NULL,
    [MatchingOrder]        TINYINT       NULL,
    CONSTRAINT [PK_MatchedCarVersionsId] PRIMARY KEY NONCLUSTERED ([MatchedCarVersionsId] ASC)
);

