CREATE TABLE [dbo].[CRM_SkodaLeadStatus] (
    [CreatedOnDatePart]  DATETIME      CONSTRAINT [DF_CRM_SkodaLeadStatus_CreatedOnDatePart_1] DEFAULT (CONVERT([varchar],getdate(),(1))) NOT NULL,
    [LeadId]             NUMERIC (18)  NOT NULL,
    [DMSId]              VARCHAR (20)  CONSTRAINT [DF_CRM_SkodaLeadStatus_DMSId] DEFAULT ((-1)) NOT NULL,
    [TokenId]            VARCHAR (50)  NULL,
    [LeadStatus]         VARCHAR (150) NULL,
    [CodeStatus]         VARCHAR (50)  NULL,
    [Dealer]             VARCHAR (50)  NULL,
    [DMSLeadDate]        VARCHAR (50)  NULL,
    [DMSConsultant]      VARCHAR (50)  NULL,
    [InvoiceNo]          VARCHAR (50)  NULL,
    [BookingNo]          VARCHAR (50)  NULL,
    [BookingDate]        VARCHAR (50)  NULL,
    [InvoiceDate]        VARCHAR (50)  NULL,
    [DeliveryDate]       VARCHAR (50)  NULL,
    [DeliveryModel]      VARCHAR (50)  NULL,
    [LeadCurrentStatus]  VARCHAR (50)  NULL,
    [LeadCurrentMessage] VARCHAR (150) NULL,
    [LastUpdated]        DATETIME      NULL,
    CONSTRAINT [PK_CRM_SkodaLeadStatus] PRIMARY KEY CLUSTERED ([CreatedOnDatePart] ASC, [LeadId] ASC, [DMSId] ASC)
);

