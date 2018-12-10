CREATE TABLE [dbo].[FuelSegments] (
    [PQId]            NUMERIC (18)  NOT NULL,
    [CustomerId]      NUMERIC (18)  NOT NULL,
    [NAME]            VARCHAR (100) NOT NULL,
    [email]           VARCHAR (100) NOT NULL,
    [City]            VARCHAR (50)  NOT NULL,
    [SubSegment]      VARCHAR (50)  NULL,
    [BodyStyle]       VARCHAR (50)  NOT NULL,
    [Make]            VARCHAR (30)  NOT NULL,
    [Model]           VARCHAR (30)  NOT NULL,
    [Version]         VARCHAR (50)  NULL,
    [CarSubSegmentId] NUMERIC (18)  NULL,
    [rownum]          BIGINT        NULL
);

