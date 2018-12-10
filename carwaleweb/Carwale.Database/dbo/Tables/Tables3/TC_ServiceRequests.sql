CREATE TABLE [dbo].[TC_ServiceRequests] (
    [TC_ServiceRequest_Id] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesId]       BIGINT        NULL,
    [RegNo]                VARCHAR (50)  NULL,
    [PreferredDate]        DATETIME      NULL,
    [TypeOfService]        SMALLINT      NULL,
    [Comments]             VARCHAR (250) NULL,
    [VersionId]            INT           NULL,
    [TC_CustomerId]        BIGINT        NULL,
    [InquirySourceId]      TINYINT       NULL,
    [CreatedDate]          DATETIME      DEFAULT (NULL) NULL
);

