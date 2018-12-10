CREATE TABLE [dbo].[CRM_InboundCallData] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CRMCustId]       NUMERIC (18) NULL,
    [CWCustId]        NUMERIC (18) NULL,
    [UCInquiryId]     NUMERIC (18) NULL,
    [CRM_LeadId]      NUMERIC (18) NULL,
    [MobileNumber]    VARCHAR (15) NULL,
    [InterestedIn]    SMALLINT     NULL,
    [CreatedOn]       DATETIME     NULL,
    [CreatedBy]       NUMERIC (18) NULL,
    [PurposeOfCallId] INT          NULL,
    [OfferId]         INT          NULL,
    CONSTRAINT [PK_CRM_InboundCallData] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-New Car, 2- Used Car', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CRM_InboundCallData', @level2type = N'COLUMN', @level2name = N'InterestedIn';

