CREATE TABLE [dbo].[OLM_SummerCampaign] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [Mobile]        VARCHAR (15)  NOT NULL,
    [Email]         VARCHAR (100) NOT NULL,
    [PreferredDate] DATE          NOT NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_OLM_SummerCampaign_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_SummerCampaign] PRIMARY KEY CLUSTERED ([Id] ASC)
);

