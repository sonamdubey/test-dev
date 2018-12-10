CREATE TABLE [dbo].[NCS_DealerContactPointLog] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ContactPtId]      NUMERIC (18)  NOT NULL,
    [DealerId]         NUMERIC (18)  NOT NULL,
    [ContactName]      VARCHAR (30)  NOT NULL,
    [Email]            VARCHAR (200) NULL,
    [Mobile]           VARCHAR (50)  NOT NULL,
    [ModelId]          NUMERIC (18)  NOT NULL,
    [ContactPointType] INT           NULL,
    [MakeId]           NUMERIC (18)  NULL,
    [Designation]      VARCHAR (100) NULL,
    [AlternateMobile]  VARCHAR (15)  NULL,
    [UpdatedBy]        NUMERIC (18)  NOT NULL,
    [UpdatedOn]        DATETIME      NOT NULL,
    CONSTRAINT [PK_NCS_DealerContactPointLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

