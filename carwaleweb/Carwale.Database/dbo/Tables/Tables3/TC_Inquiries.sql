CREATE TABLE [dbo].[TC_Inquiries] (
    [TC_InquiriesId]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [BranchId]             INT           NULL,
    [TC_CustomerId]        BIGINT        NULL,
    [VersionId]            INT           NULL,
    [MakeId]               INT           NULL,
    [ModelId]              INT           NULL,
    [CarName]              VARCHAR (110) NULL,
    [SourceId]             INT           NULL,
    [InquiryType]          TINYINT       NULL,
    [CreatedBy]            INT           NULL,
    [CreatedDate]          DATETIME      NULL,
    [IsActive]             BIT           NULL,
    [OldStockId]           INT           NULL,
    [CW_Inq_id]            BIGINT        NULL,
    [TC_LeadTypeId]        TINYINT       NULL,
    [Old_TC_CustomerId]    BIGINT        NULL,
    [Temp_InquiriesLeadId] BIGINT        NULL,
    CONSTRAINT [PK_TC_Inquiry_Id] PRIMARY KEY CLUSTERED ([TC_InquiriesId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Inquiries]
    ON [dbo].[TC_Inquiries]([InquiryType] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Inquiries_TC_CustomerId]
    ON [dbo].[TC_Inquiries]([TC_CustomerId] ASC);

