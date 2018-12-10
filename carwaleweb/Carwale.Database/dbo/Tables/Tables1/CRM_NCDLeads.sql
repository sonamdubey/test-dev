CREATE TABLE [dbo].[CRM_NCDLeads] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CBDId]          NUMERIC (18) NOT NULL,
    [DealerId]       NUMERIC (18) NOT NULL,
    [CityId]         NUMERIC (18) NOT NULL,
    [VersionId]      NUMERIC (18) NOT NULL,
    [InquiryType]    TINYINT      NOT NULL,
    [InquirySource]  TINYINT      NOT NULL,
    [BuyTime]        VARCHAR (50) NULL,
    [CustomerName]   VARCHAR (50) NOT NULL,
    [CustomerEmail]  VARCHAR (80) NOT NULL,
    [CustomerMobile] VARCHAR (15) NOT NULL,
    [TCQuoteId]      NUMERIC (18) CONSTRAINT [DF_CRM_NCDLeads_TCQuoteId] DEFAULT ((-1)) NULL,
    [InquiryDate]    DATETIME     CONSTRAINT [DF_CRM_NCDLeads_InquiryDate] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]      DATETIME     NULL,
    [ForwardedBy]    INT          NOT NULL,
    CONSTRAINT [PK_CRM_NCDLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
);

