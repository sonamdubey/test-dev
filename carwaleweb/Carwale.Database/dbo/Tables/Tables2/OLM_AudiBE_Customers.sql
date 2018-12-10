CREATE TABLE [dbo].[OLM_AudiBE_Customers] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [Mobile]      VARCHAR (50)  NULL,
    [StateId]     NUMERIC (18)  NOT NULL,
    [CityId]      NUMERIC (18)  NOT NULL,
    [Email]       VARCHAR (50)  NULL,
    [SourceId]    INT           NULL,
    [CreatedDate] DATETIME      NULL,
    CONSTRAINT [PK_OLM_AudiBE_Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

