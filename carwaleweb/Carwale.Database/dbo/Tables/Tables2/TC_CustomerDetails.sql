CREATE TABLE [dbo].[TC_CustomerDetails] (
    [id]                      INT           IDENTITY (1, 1) NOT NULL,
    [CustomerName]            VARCHAR (100) NOT NULL,
    [Email]                   VARCHAR (100) NULL,
    [Mobile]                  VARCHAR (15)  NULL,
    [IsActive]                BIT           CONSTRAINT [DF_TC_CustomerDetails_IsActive] DEFAULT ((1)) NOT NULL,
    [Address]                 VARCHAR (200) NULL,
    [City]                    INT           NULL,
    [Pincode]                 VARCHAR (50)  NULL,
    [Dob]                     DATE          NULL,
    [Anniversary]             DATE          NULL,
    [BranchId]                NUMERIC (18)  NULL,
    [ModifiedDate]            DATETIME      NULL,
    [EntryDate]               DATETIME      CONSTRAINT [DF__TC_Custom__Entry__2D1838D1] DEFAULT (getdate()) NULL,
    [ModifiedBy]              INT           NULL,
    [Buytime]                 VARCHAR (20)  NULL,
    [Location]                VARCHAR (50)  NULL,
    [Comments]                VARCHAR (400) NULL,
    [CreatedBy]               BIGINT        NULL,
    [CW_CustomerId]           BIGINT        NULL,
    [ToBeDeleted]             BIT           NULL,
    [FinalCustomerId]         BIGINT        NULL,
    [IsleadActive]            BIT           CONSTRAINT [DF_TC_CustomerDetails_IsleadActive] DEFAULT ((1)) NULL,
    [ActiveLeadId]            INT           NULL,
    [IsVerified]              BIT           CONSTRAINT [DF_TC_CustomerDetails_IsVerified] DEFAULT ((0)) NULL,
    [Isfake]                  BIT           CONSTRAINT [DF_TC_CustomerDetails_Isfake] DEFAULT ((0)) NULL,
    [NCS_CustomerId]          INT           NULL,
    [TC_InquirySourceId]      INT           NULL,
    [Salutation]              VARCHAR (15)  NULL,
    [LastName]                VARCHAR (100) NULL,
    [UniqueCustomerId]        VARCHAR (12)  NULL,
    [TC_CampaignSchedulingId] INT           NULL,
    [AlternateNumber]         VARCHAR (15)  NULL,
    CONSTRAINT [PK_TC_CustomerDetails] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [DF_TC_CustomerDetails_TC_InquirySource] FOREIGN KEY ([TC_InquirySourceId]) REFERENCES [dbo].[TC_InquirySource] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CustomerDetails_Mobile]
    ON [dbo].[TC_CustomerDetails]([Mobile] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CustomerDetails_BranchId]
    ON [dbo].[TC_CustomerDetails]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CustomerDetails_ActiveLeadId]
    ON [dbo].[TC_CustomerDetails]([ActiveLeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CUSTOMERDETAILS_Email]
    ON [dbo].[TC_CustomerDetails]([Email] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CustomerDetails_UniqueCustomerId]
    ON [dbo].[TC_CustomerDetails]([UniqueCustomerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CustomerDetails_IsleadActive]
    ON [dbo].[TC_CustomerDetails]([IsleadActive] ASC);

