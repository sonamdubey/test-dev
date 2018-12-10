CREATE TABLE [dbo].[InquiryDistribution] (
    [ID]                NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]        NUMERIC (18) NOT NULL,
    [InquiryTypeid]     SMALLINT     NOT NULL,
    [InquiryId]         NUMERIC (18) NOT NULL,
    [ServiceProviderId] NUMERIC (18) NOT NULL,
    [EntryDate]         DATETIME     NOT NULL,
    CONSTRAINT [PK_Followup] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Followup_InquiryTypes] FOREIGN KEY ([InquiryTypeid]) REFERENCES [dbo].[InquiryTypes] ([ID])
);

