CREATE TABLE [dbo].[NCS_DealerContactPoint] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]         NUMERIC (18)  NOT NULL,
    [ContactName]      VARCHAR (30)  NOT NULL,
    [Email]            VARCHAR (200) NOT NULL,
    [Mobile]           VARCHAR (50)  NOT NULL,
    [ModelId]          NUMERIC (18)  NOT NULL,
    [ContactPointType] INT           NULL,
    [MakeId]           INT           NULL,
    [Designation]      VARCHAR (100) NULL,
    [AlternateMobile]  VARCHAR (15)  NULL,
    [UpdatedBy]        NUMERIC (18)  NULL,
    [UpdatedOn]        DATETIME      NULL,
    CONSTRAINT [PK_NCS_DealerContactPoint] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_NCS_DealerContactPoint_DealerId]
    ON [dbo].[NCS_DealerContactPoint]([DealerId] ASC, [ModelId] ASC, [ContactPointType] ASC);

