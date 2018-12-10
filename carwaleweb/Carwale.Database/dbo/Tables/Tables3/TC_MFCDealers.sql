CREATE TABLE [dbo].[TC_MFCDealers] (
    [DealerId]         NUMERIC (18) NOT NULL,
    [SendMixMatchLead] BIT          NULL,
    [LeadCntperDay]    INT          NULL,
    CONSTRAINT [PK_TC_MFCDealers] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

