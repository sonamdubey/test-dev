CREATE TABLE [dbo].[NCS_TDReq] (
    [Id]           NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ModelId]      NUMERIC (18)   NULL,
    [VersionId]    NUMERIC (18)   NULL,
    [Name]         VARCHAR (100)  NOT NULL,
    [Email]        VARCHAR (100)  NOT NULL,
    [Mobile]       VARCHAR (50)   NULL,
    [CreatedOn]    DATETIME       NULL,
    [CityId]       INT            NULL,
    [LeadType]     INT            NULL,
    [HDFCResponse] NVARCHAR (MAX) NULL,
    [PushedLeadId] NUMERIC (18)   NULL,
    [ApiResponse]  VARCHAR (MAX)  NULL,
    [utmsource]    VARCHAR (500)  NULL,
    CONSTRAINT [PK_NCS_TDReq] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

