CREATE TABLE [dbo].[TC_CustomerDetailsLog] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]         INT           NULL,
    [BranchId]           NUMERIC (18)  NULL,
    [CustomerName]       VARCHAR (100) NULL,
    [Email]              VARCHAR (100) NULL,
    [Mobile]             VARCHAR (15)  NULL,
    [Location]           VARCHAR (50)  NULL,
    [Address]            VARCHAR (200) NULL,
    [EntryDate]          DATETIME      NULL,
    [ModifiedDate]       DATETIME      NULL,
    [ModifiedBy]         INT           NULL,
    [Buytime]            VARCHAR (20)  NULL,
    [Comments]           VARCHAR (MAX) NULL,
    [CreatedBy]          BIGINT        NULL,
    [CW_CustomerId]      BIGINT        NULL,
    [TC_InquirySourceId] INT           NULL,
    [Salutation]         VARCHAR (15)  NULL,
    [LastName]           VARCHAR (100) NULL,
    [AlternateNumber]    VARCHAR (10)  NULL,
    [City]               INT           NULL,
    CONSTRAINT [PK_TC_CustomerDetailsLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

