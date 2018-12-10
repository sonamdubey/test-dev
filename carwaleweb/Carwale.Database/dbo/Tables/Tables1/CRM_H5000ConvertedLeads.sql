CREATE TABLE [dbo].[CRM_H5000ConvertedLeads] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [LeadId]        NUMERIC (18) NOT NULL,
    [CBDId]         NUMERIC (18) NOT NULL,
    [CarGroupType]  INT          NOT NULL,
    [LeadGroupType] INT          NOT NULL,
    [CreatedOn]     DATETIME     CONSTRAINT [DF_CRM_H5000ConvertedLeads_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_CRM_H5000ConvertedLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

