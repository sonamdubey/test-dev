CREATE TABLE [dbo].[Financers] (
    [Id]           INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]         VARCHAR (100) NOT NULL,
    [FinancerLogo] VARCHAR (100) NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Financers_IsActive] DEFAULT (1) NOT NULL,
    [IsReplicated] BIT           DEFAULT ((1)) NULL,
    [HostURL]      VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_Financers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

