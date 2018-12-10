CREATE TABLE [dbo].[InquiryStatus] (
    [ID]       SMALLINT     NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_InquiryStatus_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_InquiryStatus] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

