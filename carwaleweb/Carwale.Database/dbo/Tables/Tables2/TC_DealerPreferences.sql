CREATE TABLE [dbo].[TC_DealerPreferences] (
    [TC_DealerPreferencesId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [BranchId]               NUMERIC (18) NULL,
    [TC_LeadInquiryType]     TINYINT      NOT NULL,
    [PrefOrder]              TINYINT      NOT NULL,
    CONSTRAINT [PK_TC_DealerPreferences] PRIMARY KEY CLUSTERED ([TC_DealerPreferencesId] ASC),
    CONSTRAINT [DF_TC_DealerPreferences_Dealers] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Dealers] ([ID]),
    CONSTRAINT [DF_TC_DealerPreferences_TC_LeadInquiryType] FOREIGN KEY ([TC_LeadInquiryType]) REFERENCES [dbo].[TC_LeadInquiryType] ([TC_LeadInquiryTypeId])
);

