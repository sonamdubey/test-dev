CREATE TABLE [dbo].[CRM_PushedLeadLog] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CustId]      NUMERIC (18) NOT NULL,
    [PushedDate]  DATETIME     NOT NULL,
    [UpdatedBy]   NUMERIC (18) NOT NULL,
    [LeadId]      NUMERIC (18) NOT NULL,
    [InquiryDate] DATETIME     NULL,
    CONSTRAINT [PK_CRM_PushedLeadLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

