CREATE TABLE [dbo].[NCD_RejectedLeads] (
    [CustomerId]  NUMERIC (18) NOT NULL,
    [InquiryDate] DATETIME     NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_NCD_RejectedLeadsCustomerId]
    ON [dbo].[NCD_RejectedLeads]([CustomerId] ASC)
    INCLUDE([InquiryDate]);

