CREATE TABLE [dbo].[TC_SellerInquiries] (
    [TC_SellerInquiriesId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesId]          BIGINT         NULL,
    [Price]                   BIGINT         NULL,
    [Kms]                     BIGINT         NULL,
    [MakeYear]                DATE           NULL,
    [Colour]                  VARCHAR (100)  NULL,
    [RegNo]                   VARCHAR (50)   NULL,
    [Comments]                VARCHAR (MAX)  NULL,
    [RegistrationPlace]       VARCHAR (50)   NULL,
    [Insurance]               VARCHAR (50)   NULL,
    [InsuranceExpiry]         DATETIME       NULL,
    [Owners]                  VARCHAR (20)   NULL,
    [CarDriven]               VARCHAR (20)   NULL,
    [Tax]                     VARCHAR (20)   NULL,
    [CityMileage]             VARCHAR (20)   NULL,
    [AdditionalFuel]          VARCHAR (20)   NULL,
    [Accidental]              BIT            NULL,
    [FloodAffected]           BIT            NULL,
    [InteriorColor]           VARCHAR (100)  NULL,
    [CWInquiryId]             BIGINT         NULL,
    [Warranties]              VARCHAR (500)  DEFAULT (NULL) NULL,
    [Modifications]           VARCHAR (500)  DEFAULT (NULL) NULL,
    [ACCondition]             VARCHAR (50)   DEFAULT (NULL) NULL,
    [BatteryCondition]        VARCHAR (50)   DEFAULT (NULL) NULL,
    [BrakesCondition]         VARCHAR (50)   DEFAULT (NULL) NULL,
    [ElectricalsCondition]    VARCHAR (50)   DEFAULT (NULL) NULL,
    [EngineCondition]         VARCHAR (50)   DEFAULT (NULL) NULL,
    [ExteriorCondition]       VARCHAR (50)   DEFAULT (NULL) NULL,
    [InteriorCondition]       VARCHAR (50)   DEFAULT (NULL) NULL,
    [SeatsCondition]          VARCHAR (50)   DEFAULT (NULL) NULL,
    [SuspensionsCondition]    VARCHAR (50)   DEFAULT (NULL) NULL,
    [TyresCondition]          VARCHAR (50)   DEFAULT (NULL) NULL,
    [OverallCondition]        VARCHAR (50)   DEFAULT (NULL) NULL,
    [Features_SafetySecurity] VARCHAR (200)  DEFAULT (NULL) NULL,
    [Features_Comfort]        VARCHAR (200)  DEFAULT (NULL) NULL,
    [Features_Others]         VARCHAR (200)  DEFAULT (NULL) NULL,
    [ModifiedBy]              BIGINT         DEFAULT (NULL) NULL,
    [LastUpdatedDate]         DATETIME       DEFAULT (NULL) NULL,
    [IsPurchased]             BIT            DEFAULT ((0)) NULL,
    [TC_InquiriesLeadId]      BIGINT         NULL,
    [TC_InquirySourceId]      INT            NULL,
    [CreatedOn]               DATETIME       CONSTRAINT [DF_TC_SellerInquiries_CreatedOn] DEFAULT (getdate()) NULL,
    [CreatedBy]               INT            NULL,
    [TC_LeadDispositionID]    TINYINT        NULL,
    [CarVersionId]            NUMERIC (18)   NULL,
    [PurchasedDate]           DATETIME       NULL,
    [PurchasedStatus]         TINYINT        NULL,
    [TC_InquiryOtherSourceId] TINYINT        NULL,
    [TC_ActionApplicationId]  INT            NULL,
    [cteapiresponse]          VARCHAR (2000) NULL,
    [cteinquiryid]            INT            NULL,
    CONSTRAINT [PK_TC_CustomerStock_Id] PRIMARY KEY NONCLUSTERED ([TC_SellerInquiriesId] ASC),
    CONSTRAINT [DF_TC_SellerInquiries_TC_InquiriesLead] FOREIGN KEY ([TC_InquiriesLeadId]) REFERENCES [dbo].[TC_InquiriesLead] ([TC_InquiriesLeadId]),
    CONSTRAINT [DF_TC_SellerInquiries_TC_InquirySourceId] FOREIGN KEY ([TC_InquirySourceId]) REFERENCES [dbo].[TC_InquirySource] ([Id]),
    CONSTRAINT [DF_TC_SellerInquiries_TC_LeadDisposition] FOREIGN KEY ([TC_LeadDispositionID]) REFERENCES [dbo].[TC_LeadDisposition] ([TC_LeadDispositionId]),
    CONSTRAINT [DF_TC_tc_inquirieslead_carversions] FOREIGN KEY ([CarVersionId]) REFERENCES [dbo].[CarVersions] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_SellerInquiries_TC_InquirySourceId]
    ON [dbo].[TC_SellerInquiries]([TC_InquirySourceId] ASC)
    INCLUDE([TC_InquiriesLeadId]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_SellerInquiries_TC_InquiriesLeadId]
    ON [dbo].[TC_SellerInquiries]([TC_InquiriesLeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_SellerInquiries_TC_InquirySourceId2]
    ON [dbo].[TC_SellerInquiries]([TC_InquirySourceId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_SellerInquiries_TC_LeadDispositionID]
    ON [dbo].[TC_SellerInquiries]([TC_LeadDispositionID] ASC)
    INCLUDE([TC_SellerInquiriesId], [TC_InquiriesLeadId], [CreatedOn], [CarVersionId], [PurchasedDate], [PurchasedStatus]);


GO
CREATE NONCLUSTERED INDEX [ix_TC_SellerInquiries_CreatedOn]
    ON [dbo].[TC_SellerInquiries]([CreatedOn] DESC);

