CREATE TABLE [dbo].[ClassifiedLeads] (
    [Id]                      NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [InquiryId]               NUMERIC (18)  NOT NULL,
    [CustName]                VARCHAR (50)  NULL,
    [CustEmail]               VARCHAR (100) NULL,
    [CustMobile]              VARCHAR (15)  NULL,
    [IsVerified]              BIT           DEFAULT ((0)) NOT NULL,
    [EntryDateTime]           DATETIME      CONSTRAINT [DF_classifiedLeads_EntryDateTime] DEFAULT (getdate()) NULL,
    [CarwaleInternalClientId] NUMERIC (18)  NULL,
    [sellerType]              VARCHAR (50)  NULL,
    [UtmaCookie]              VARCHAR (250) NULL,
    [UtmzCookie]              VARCHAR (500) NULL,
    [IpAddress]               VARCHAR (60)  NULL,
    [IsSentToSource]          BIT           CONSTRAINT [df_ClassifiedLeads_IsSentToSource] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_classifiedLeads_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ClassifiedLeads_CustEmail]
    ON [dbo].[ClassifiedLeads]([CustEmail] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ClassifiedLeads_CustMobile]
    ON [dbo].[ClassifiedLeads]([CustMobile] ASC);

