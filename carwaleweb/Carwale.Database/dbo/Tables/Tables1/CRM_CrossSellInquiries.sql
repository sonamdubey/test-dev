CREATE TABLE [dbo].[CRM_CrossSellInquiries] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [SelInquiryId] BIGINT       NOT NULL,
    [LeadId]       BIGINT       NOT NULL,
    [CreatedBy]    BIGINT       NOT NULL,
    [CreatedOn]    DATETIME     CONSTRAINT [DF_CRM_CrossSellInquiries_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CRM_CrossSellInquiries] PRIMARY KEY CLUSTERED ([Id] ASC)
);

