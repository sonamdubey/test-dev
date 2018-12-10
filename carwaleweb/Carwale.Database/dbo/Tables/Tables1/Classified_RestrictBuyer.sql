CREATE TABLE [dbo].[Classified_RestrictBuyer] (
    [RequestDate]        DATE      NOT NULL,
    [Mobileno]           CHAR (10) NOT NULL,
    [InquiryViewedCount] SMALLINT  NOT NULL,
    CONSTRAINT [PK_Classified_RestrictBuyer] PRIMARY KEY CLUSTERED ([RequestDate] ASC, [Mobileno] ASC)
);

