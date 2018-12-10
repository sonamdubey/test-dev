CREATE TABLE [dbo].[ExcelDataEntries] (
    [Id]         NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50)   NOT NULL,
    [Email]      VARCHAR (50)   NULL,
    [Mobile]     VARCHAR (50)   NULL,
    [City]       VARCHAR (50)   NULL,
    [CarMake]    VARCHAR (50)   NULL,
    [CarModel]   VARCHAR (50)   NULL,
    [IsValid]    BIT            NOT NULL,
    [IsDeleted]  BIT            NOT NULL,
    [UserId]     INT            NULL,
    [Comment]    VARCHAR (5000) NULL,
    [ImportDate] DATETIME       CONSTRAINT [DF_ExcelDataEntries_ImportDate] DEFAULT (getdate()) NULL,
    [LeadId]     NUMERIC (18)   NULL,
    [InquiryId]  BIGINT         NULL,
    CONSTRAINT [PK_ExcelDataEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
);

