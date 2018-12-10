CREATE TABLE [dbo].[TC_InquirySourceOthers] (
    [TC_InquirySourceOtherId] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Source]                  VARCHAR (100) NULL,
    [IsActive]                BIT           CONSTRAINT [DF_TC_InquirySourceOthers_IsActive] DEFAULT ((1)) NOT NULL
);

