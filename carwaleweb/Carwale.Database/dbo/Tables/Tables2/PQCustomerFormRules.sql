CREATE TABLE [dbo].[PQCustomerFormRules] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [MakeId]    INT          NOT NULL,
    [MakeName]  VARCHAR (40) NULL,
    [CityId]    INT          NOT NULL,
    [EntryDate] DATE         CONSTRAINT [DF_PQCustomerFormRules_EntryDate] DEFAULT (CONVERT([varchar](10),getdate(),(110))) NULL,
    [IsActive]  BIT          NOT NULL,
    CONSTRAINT [PK_PQCustomerFormRules] PRIMARY KEY CLUSTERED ([Id] ASC)
);

