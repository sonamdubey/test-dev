CREATE TABLE [dbo].[CRM_TATABulkLeadPush] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ReferenceId]  INT           NULL,
    [CustomerName] VARCHAR (250) NULL,
    [MobileNumber] VARCHAR (50)  NULL,
    [Email]        VARCHAR (250) NULL,
    [State]        VARCHAR (5)   NULL,
    [City]         VARCHAR (50)  NULL,
    [Pincode]      VARCHAR (10)  NULL,
    [DealerDivId]  VARCHAR (50)  NULL,
    [Model]        VARCHAR (20)  NULL,
    [Version]      VARCHAR (200) NULL,
    [CreatedOn]    DATETIME      CONSTRAINT [DF_CRM_TATABulkLeadPush_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [IsPushed]     BIT           CONSTRAINT [DF_CRM_TATABulkLeadPush_IsPushed] DEFAULT ((0)) NOT NULL,
    [PushedOn]     DATETIME      NULL,
    CONSTRAINT [PK_CRM_TATABulkLeadPush] PRIMARY KEY CLUSTERED ([Id] ASC)
);

