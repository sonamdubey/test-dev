CREATE TABLE [dbo].[CRM_SkodaDMSPendingLeads] (
    [Id]        NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]    NUMERIC (18)  NOT NULL,
    [Reason]    VARCHAR (250) NULL,
    [CreatedOn] DATETIME      NOT NULL,
    [UpdatedOn] DATETIME      NULL,
    [UpdatedBy] NUMERIC (18)  NULL,
    [IsPushed]  BIT           CONSTRAINT [DF_CRM_SkodaDMSPendingLeads_IsPushed] DEFAULT ((0)) NOT NULL,
    [IsActive]  BIT           CONSTRAINT [DF_CRM_SkodaDMSPendingLeads_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CRM_SkodaDMSPendingLeads] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

