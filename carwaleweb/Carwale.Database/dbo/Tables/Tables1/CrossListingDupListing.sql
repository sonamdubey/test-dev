CREATE TABLE [dbo].[CrossListingDupListing] (
    [Inquiryid]         NUMERIC (18) NOT NULL,
    [CustomerId]        NUMERIC (18) NOT NULL,
    [CarVersionId]      NUMERIC (18) NOT NULL,
    [EntryDate]         DATETIME     NOT NULL,
    [PackageId]         INT          NULL,
    [PackageType]       SMALLINT     NOT NULL,
    [PackageExpiryDate] DATETIME     NULL,
    [ROW]               BIGINT       NULL
);

