CREATE TABLE [dbo].[CRM_TataLeads] (
    [Id]               NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [FirstName]        VARCHAR (100) NULL,
    [LastName]         VARCHAR (50)  NULL,
    [Mobile]           VARCHAR (15)  NULL,
    [City]             VARCHAR (20)  NULL,
    [State]            VARCHAR (20)  NULL,
    [Address]          VARCHAR (500) NULL,
    [Pincode]          VARCHAR (10)  NULL,
    [Model]            VARCHAR (20)  NULL,
    [Variant]          VARCHAR (100) NULL,
    [DealerDivisionId] VARCHAR (100) NULL,
    [Email]            VARCHAR (50)  NULL,
    [IsPushSuccess]    BIT           NULL,
    [MessageReturned]  VARCHAR (400) NULL,
    [CreatedOn]        DATETIME      CONSTRAINT [DF_CRM_TataLeads_CreatedOn] DEFAULT (getdate()) NULL,
    [UpdatedOn]        DATETIME      NULL,
    [UpdatedBy]        INT           NULL,
    [CBDID]            NUMERIC (18)  NULL,
    CONSTRAINT [PK_CRM_TataLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

