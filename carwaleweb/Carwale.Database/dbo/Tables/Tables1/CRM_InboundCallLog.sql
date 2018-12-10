CREATE TABLE [dbo].[CRM_InboundCallLog] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CusName]   VARCHAR (50) NOT NULL,
    [CusMobile] VARCHAR (15) NOT NULL,
    [CusEmail]  VARCHAR (50) NOT NULL,
    [VersionId] INT          NOT NULL,
    [CityId]    INT          NOT NULL,
    [PurposeId] SMALLINT     NOT NULL,
    [LeadId]    BIGINT       NOT NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_CRM_InboundCallLog_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy] INT          NOT NULL,
    CONSTRAINT [PK_CRM_IndoundCallLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

