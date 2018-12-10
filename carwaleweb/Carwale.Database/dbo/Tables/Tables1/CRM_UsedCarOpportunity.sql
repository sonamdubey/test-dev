CREATE TABLE [dbo].[CRM_UsedCarOpportunity] (
    [Id]        NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [LeadId]    NUMERIC (18) NOT NULL,
    [Budget]    VARCHAR (10) NULL,
    [MakeId]    INT          NULL,
    [ModelId]   INT          NULL,
    [CreatedBy] INT          NULL,
    [CreatedOn] DATETIME     CONSTRAINT [DF_CRM_UsedCarOpportunity_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_CRM_UsedCarOpportunity] PRIMARY KEY CLUSTERED ([Id] ASC)
);

