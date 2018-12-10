CREATE TABLE [dbo].[CRM_TempAudiLeadPush] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (250) NULL,
    [EMail]     VARCHAR (250) NULL,
    [Mobile]    VARCHAR (250) NULL,
    [CityId]    NUMERIC (18)  NULL,
    [VersionId] NUMERIC (18)  NULL,
    [IsPushed]  BIT           CONSTRAINT [DF_CRM_TempAudiLeadPush_IsPushed] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CRM_TempAudiLeadPush] PRIMARY KEY CLUSTERED ([Id] ASC)
);

