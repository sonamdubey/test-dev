CREATE TABLE [dbo].[OLM_AudiLeMans] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CustomerName]       VARCHAR (100) NULL,
    [ContactNumber]      VARCHAR (15)  NULL,
    [CurrentCar]         VARCHAR (50)  NULL,
    [AudiDealershipId]   INT           NULL,
    [AudiDealershipName] VARCHAR (50)  NULL,
    [PreferredTime]      VARCHAR (50)  NULL,
    [Comments]           VARCHAR (500) NULL,
    [EntryDate]          DATETIME      CONSTRAINT [DF_OLM_AudiLeMans_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_AudiLeMans] PRIMARY KEY CLUSTERED ([Id] ASC)
);

