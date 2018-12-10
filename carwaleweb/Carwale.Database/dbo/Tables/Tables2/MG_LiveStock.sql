CREATE TABLE [dbo].[MG_LiveStock] (
    [MGS_Id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [InquiryId]       NUMERIC (18)  NOT NULL,
    [Car]             VARCHAR (100) NOT NULL,
    [MakeId]          NUMERIC (18)  NULL,
    [ModelId]         NUMERIC (18)  NULL,
    [VersionId]       NUMERIC (18)  NULL,
    [Color]           VARCHAR (50)  NOT NULL,
    [Price]           NUMERIC (18)  NOT NULL,
    [Kilometers]      NUMERIC (18)  NOT NULL,
    [MakeYear]        DATETIME      NOT NULL,
    [Owner]           VARCHAR (100) NOT NULL,
    [Mobile]          VARCHAR (50)  NULL,
    [Phone]           VARCHAR (50)  NULL,
    [IsDealer]        BIT           NOT NULL,
    [City]            VARCHAR (50)  NOT NULL,
    [CityId]          NUMERIC (18)  NULL,
    [State]           VARCHAR (100) NULL,
    [StateId]         NUMERIC (18)  NULL,
    [UpdatedDateTime] DATETIME      NULL,
    CONSTRAINT [PK_MG_LiveStock] PRIMARY KEY CLUSTERED ([MGS_Id] ASC) WITH (FILLFACTOR = 90)
);

