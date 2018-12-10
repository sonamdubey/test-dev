CREATE TABLE [dbo].[CRM_CarBasicData] (
    [ID]                    NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LeadId]                NUMERIC (18) NOT NULL,
    [VersionId]             NUMERIC (18) NOT NULL,
    [ExShowroom]            NUMERIC (18) NOT NULL,
    [Insurance]             NUMERIC (18) NOT NULL,
    [RTO]                   NUMERIC (18) NOT NULL,
    [PQId]                  NUMERIC (18) NOT NULL,
    [CityId]                NUMERIC (18) NOT NULL,
    [ExpectedBuyingDate]    DATETIME     NOT NULL,
    [IsProductExplained]    BIT          NOT NULL,
    [IsPQMailed]            BIT          NOT NULL,
    [IsPQMailedExternal]    BIT          CONSTRAINT [DF_CRM_CarBasicData_IsPQMailedExternal] DEFAULT ((0)) NOT NULL,
    [IsPQMailInternalReq]   BIT          CONSTRAINT [DF_CRM_CarBasicData_IsPQMailInternalReq] DEFAULT ((0)) NOT NULL,
    [IsPQMailExternalReq]   BIT          CONSTRAINT [DF_CRM_CarBasicData_IsPQMailExternalReq] DEFAULT ((0)) NOT NULL,
    [IsFinalized]           BIT          NOT NULL,
    [CreatedOn]             DATETIME     NOT NULL,
    [UpdatedOn]             DATETIME     NOT NULL,
    [UpdatedBy]             NUMERIC (18) NOT NULL,
    [GotTDFeedback]         BIT          CONSTRAINT [DF_CRM_CarBasicData_GotTDFeedback] DEFAULT ((0)) NOT NULL,
    [GotDealerFeedback]     BIT          CONSTRAINT [DF_CRM_CarBasicData_GotDealerFeedback] DEFAULT ((0)) NOT NULL,
    [PriceQuoteNotRequired] BIT          NULL,
    [IsPQRearrange]         BIT          NULL,
    [IsPENotRequired]       BIT          NULL,
    [IsVisitedDealer]       BIT          NULL,
    [SourceId]              INT          NULL,
    [SourceCategory]        INT          NULL,
    [IsNotInterested]       BIT          NULL,
    [IsDealerAssigned]      BIT          CONSTRAINT [DF_CRM_CarBasicData_IsDealerAssigned] DEFAULT ((0)) NULL,
    [IsDeleted]             BIT          CONSTRAINT [DF_CRM_CarBasicData_IsDeleted] DEFAULT ((0)) NULL,
    [DeleteReasonId]        INT          NULL,
    [FLCGroupId]            INT          NULL,
    CONSTRAINT [PK_CRM_CarBasicData] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarBasicData_CreatedOn]
    ON [dbo].[CRM_CarBasicData]([CreatedOn] ASC)
    INCLUDE([ID], [LeadId], [VersionId]);


GO
CREATE NONCLUSTERED INDEX [Idx_CRM_CarBasicData_VersionId]
    ON [dbo].[CRM_CarBasicData]([VersionId] ASC)
    INCLUDE([LeadId]);


GO
CREATE NONCLUSTERED INDEX [ix_CRM_CarBasicData]
    ON [dbo].[CRM_CarBasicData]([LeadId] ASC, [VersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_CarBasicData_PQId]
    ON [dbo].[CRM_CarBasicData]([PQId] ASC);

