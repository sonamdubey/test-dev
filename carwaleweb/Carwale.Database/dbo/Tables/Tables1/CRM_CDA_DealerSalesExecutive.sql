CREATE TABLE [dbo].[CRM_CDA_DealerSalesExecutive] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [OrgId]       NUMERIC (18)  NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Contact]     VARCHAR (15)  NULL,
    [Email]       VARCHAR (100) NULL,
    [Designation] VARCHAR (50)  NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_CRM_CDA_DealerSalesExecutive_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_CDA_DealerSalesExecutive] PRIMARY KEY CLUSTERED ([Id] ASC)
);

