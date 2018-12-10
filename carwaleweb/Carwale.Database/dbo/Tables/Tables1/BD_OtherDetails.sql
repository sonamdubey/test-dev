CREATE TABLE [dbo].[BD_OtherDetails] (
    [NCPInquiryId]        NUMERIC (18)  NOT NULL,
    [InterestedInFinance] BIT           NULL,
    [EmploymentType]      VARCHAR (50)  NULL,
    [Address]             VARCHAR (250) NULL,
    [Pincode]             VARCHAR (50)  NULL,
    [AltContactNo]        VARCHAR (50)  NULL,
    [Testdrive]           VARCHAR (50)  NULL,
    [EntryDateTime]       DATETIME      CONSTRAINT [DF_BD_OtherDetails_EntryDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CS_OtherDetails] PRIMARY KEY CLUSTERED ([NCPInquiryId] ASC) WITH (FILLFACTOR = 90)
);

