CREATE TABLE [dbo].[CRM_AutoAssignedLeads] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CRMLeadId]     NUMERIC (18)  NOT NULL,
    [CRMCustId]     NUMERIC (18)  NULL,
    [CWCustId]      NUMERIC (18)  NULL,
    [CBDId]         NUMERIC (18)  NULL,
    [CityId]        INT           NULL,
    [DealerId]      INT           NULL,
    [CustFirstName] VARCHAR (250) NULL,
    [CustMobile]    VARCHAR (50)  NULL,
    [CustEmail]     VARCHAR (250) NULL,
    [CarMakeId]     INT           NULL,
    [CarModelId]    INT           NULL,
    [CarName]       VARCHAR (250) NULL,
    [LeadSourceId]  INT           NULL,
    [IsTdRequest]   BIT           NULL,
    [UpdatedBy]     INT           NULL,
    [UpdatedOn]     DATETIME      NULL,
    [IsAccepted]    BIT           NULL,
    [IsRejected]    BIT           NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_CRM_AutoAssignedLeads_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_AutoAssignedLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

