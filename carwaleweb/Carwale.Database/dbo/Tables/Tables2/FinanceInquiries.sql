CREATE TABLE [dbo].[FinanceInquiries] (
    [Id]              NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]      NUMERIC (18)   NOT NULL,
    [DOB]             DATETIME       NOT NULL,
    [LengthResidence] INT            NOT NULL,
    [Employer]        VARCHAR (50)   NOT NULL,
    [JobTitle]        VARCHAR (50)   NOT NULL,
    [MonthlyIncome]   VARCHAR (50)   NOT NULL,
    [OfficePhone]     VARCHAR (20)   NOT NULL,
    [PhoneExtension]  VARCHAR (5)    NULL,
    [LengthService]   INT            NOT NULL,
    [PanCardNo]       VARCHAR (50)   NULL,
    [Comments]        VARCHAR (2000) NULL,
    [RequestDateTime] DATETIME       NOT NULL,
    [IsApproved]      BIT            CONSTRAINT [DF_FinanceInquiries_IsApproved] DEFAULT (0) NOT NULL,
    [IsFake]          BIT            CONSTRAINT [DF_FinanceInquiries_IsFake] DEFAULT (0) NOT NULL,
    [StatusId]        SMALLINT       CONSTRAINT [DF_FinanceInquiries_StatusId] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_FinanceInquiries] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

