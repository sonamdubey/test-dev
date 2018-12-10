CREATE TABLE [dbo].[CRM_SkodaLeadStatusLMS2] (
    [CreatedOnDatePart] DATETIME       CONSTRAINT [DF_CRM_SkodaLeadStatus_CreatedOnDatePart] DEFAULT (CONVERT([varchar],getdate(),(1))) NOT NULL,
    [LeadId]            NUMERIC (18)   NOT NULL,
    [TokenId]           VARCHAR (50)   NULL,
    [CreatedOn]         DATETIME       CONSTRAINT [DF_CRM_SkodaLeadStatus_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [LeadStatus]        VARCHAR (150)  NULL,
    [XMLStatus]         VARCHAR (150)  NULL,
    [PFInvoiceDate]     VARCHAR (50)   NULL,
    [TDDate]            VARCHAR (50)   NULL,
    [BookingDate]       VARCHAR (50)   NULL,
    [InvoiceDate]       VARCHAR (50)   NULL,
    [DeliveryDate]      VARCHAR (50)   NULL,
    [LastUpdated]       DATETIME       NULL,
    [ConsultantName]    VARCHAR (50)   NULL,
    [XMLString]         VARCHAR (8000) NULL
);

