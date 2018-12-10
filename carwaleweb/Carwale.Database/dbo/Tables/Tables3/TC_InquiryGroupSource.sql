CREATE TABLE [dbo].[TC_InquiryGroupSource] (
    [TC_InquiryGroupSourceId] SMALLINT     IDENTITY (1, 1) NOT NULL,
    [GroupSourceName]         VARCHAR (50) NULL,
    [IsActive]                BIT          CONSTRAINT [DF_TC_InquiryGroupSource_IsActive] DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([TC_InquiryGroupSourceId] ASC)
);

