CREATE TABLE [dbo].[InquiryOnPhone] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]           VARCHAR (50) NOT NULL,
    [DealershipName] VARCHAR (50) NOT NULL,
    [City]           VARCHAR (50) NOT NULL,
    [MobileNo]       VARCHAR (15) NULL,
    [OfficeNo]       VARCHAR (15) NULL,
    CONSTRAINT [PK_InquiryOnPhone] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

