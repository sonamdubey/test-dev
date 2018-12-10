CREATE TABLE [dbo].[OLM_AudiBangalore] (
    [Id]         NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [FullName]   VARCHAR (100) NULL,
    [Mobile]     VARCHAR (15)  NULL,
    [City]       VARCHAR (50)  NULL,
    [Email]      VARCHAR (100) NULL,
    [Profession] VARCHAR (100) NULL,
    [CurrentCar] VARCHAR (100) NULL,
    [EntryDate]  DATETIME      CONSTRAINT [DF_OLM_AudiBangalore_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_AudiBangalore] PRIMARY KEY CLUSTERED ([Id] ASC)
);

