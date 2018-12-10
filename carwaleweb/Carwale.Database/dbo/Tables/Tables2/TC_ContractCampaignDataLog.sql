CREATE TABLE [dbo].[TC_ContractCampaignDataLog] (
    [Id]                 BIGINT       IDENTITY (1, 1) NOT NULL,
    [TC_InquiryLeadId]   BIGINT       NULL,
    [TC_NewCarInquiryId] BIGINT       NULL,
    [CampaignId]         INT          NULL,
    [ContractId]         INT          NULL,
    [Status]             VARCHAR (50) NULL,
    [LogDate]            DATETIME     CONSTRAINT [DF_TC_ContractCampaignDataLog_LogDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TC_ContractCampaignDataLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

