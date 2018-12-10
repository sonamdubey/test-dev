CREATE TABLE [dbo].[TC_InsuranceCompany] (
    [TC_InsuranceCompany_Id] INT          IDENTITY (1, 1) NOT NULL,
    [DealerId]               INT          NOT NULL,
    [CompanyName]            VARCHAR (50) NOT NULL,
    [IsActive]               BIT          CONSTRAINT [DF_TC_InsuranceCompany_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]              DATETIME     CONSTRAINT [DF_TC_InsuranceCompany_EntryDate] DEFAULT (getdate()) NULL,
    [ModifiedDate]           DATETIME     NULL,
    [ModifiedBy]             INT          NULL,
    CONSTRAINT [PK_TC_InsuranceCompany] PRIMARY KEY CLUSTERED ([TC_InsuranceCompany_Id] ASC)
);

