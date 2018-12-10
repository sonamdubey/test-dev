CREATE TABLE [dbo].[TC_BucketLeadType] (
    [TC_BucketLeadTypeId]  SMALLINT      NOT NULL,
    [BucketName]           VARCHAR (50)  NULL,
    [Description]          VARCHAR (500) NULL,
    [IsActive]             BIT           NULL,
    [PriorityOrder]        TINYINT       NULL,
    [TC_LeadInquiryTypeId] SMALLINT      NULL,
    [TC_BusinessTypeId]    TINYINT       NULL,
    CONSTRAINT [PK_TC_BucketLeadType] PRIMARY KEY CLUSTERED ([TC_BucketLeadTypeId] ASC)
);

