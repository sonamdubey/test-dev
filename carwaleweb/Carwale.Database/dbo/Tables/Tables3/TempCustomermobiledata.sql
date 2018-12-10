CREATE TABLE [dbo].[TempCustomermobiledata] (
    [CustomerName]    VARCHAR (100) NOT NULL,
    [Email]           VARCHAR (100) NOT NULL,
    [Mobile]          VARCHAR (20)  NULL,
    [phoneNo]         VARCHAR (20)  NULL,
    [CarYear]         INT           NULL,
    [State]           VARCHAR (30)  NOT NULL,
    [City]            VARCHAR (50)  NOT NULL,
    [CarName]         VARCHAR (112) NULL,
    [CityId]          NUMERIC (18)  NOT NULL,
    [MakeYear]        DATETIME      NOT NULL,
    [Kilometers]      NUMERIC (18)  NOT NULL,
    [Price]           DECIMAL (18)  NOT NULL,
    [RequestDateTIme] DATETIME      NOT NULL,
    [Type]            VARCHAR (10)  NOT NULL,
    [VersionId]       NUMERIC (18)  NOT NULL,
    [RMonth]          INT           NULL,
    [RYear]           INT           NULL,
    [DuplicateCount]  BIGINT        NULL
);

