CREATE TABLE [dbo].[OLM_SkodaCampaign] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ServiceCenterId] NUMERIC (18)  NOT NULL,
    [FullName]        VARCHAR (150) NOT NULL,
    [Email]           VARCHAR (100) NOT NULL,
    [Mobile]          VARCHAR (15)  NOT NULL,
    [City]            NUMERIC (18)  NOT NULL,
    [VehicleRegNum]   VARCHAR (15)  NULL,
    [IPAddress]       VARCHAR (50)  NULL,
    [Source]          VARCHAR (50)  NOT NULL,
    [IsMailSent]      BIT           NOT NULL,
    [EntryDate]       DATETIME      NULL,
    CONSTRAINT [PK_OLM_SkodaCampaign] PRIMARY KEY CLUSTERED ([Id] ASC)
);

