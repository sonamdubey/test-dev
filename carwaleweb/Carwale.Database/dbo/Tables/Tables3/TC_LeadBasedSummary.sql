CREATE TABLE [dbo].[TC_LeadBasedSummary] (
    [DealerId]                   INT           NULL,
    [TC_LeadId]                  BIGINT        NULL,
    [CreatedDate]                DATETIME      NULL,
    [Organization]               VARCHAR (100) NULL,
    [Eagerness]                  VARCHAR (50)  NULL,
    [EagernessId]                TINYINT       NULL,
    [Source]                     VARCHAR (100) NULL,
    [SourceId]                   TINYINT       NULL,
    [TC_LeadStageId]             TINYINT       NULL,
    [TC_LeadDispositionID]       SMALLINT      NULL,
    [CarModel]                   VARCHAR (80)  NULL,
    [CarModelId]                 INT           NULL,
    [ScheduledOn]                DATETIME      NULL,
    [TestDriveDate]              DATE          NULL,
    [TestDriveStatus]            TINYINT       NULL,
    [TC_NewCarInquiriesId]       BIGINT        NULL,
    [BookingStatus]              TINYINT       NULL,
    [BookingDate]                DATETIME      NULL,
    [LeadClosedDate]             DATETIME      NULL,
    [InquiryDispositionId]       SMALLINT      NULL,
    [InquirySubDispositionId]    SMALLINT      NULL,
    [LostVersionId]              NUMERIC (18)  NULL,
    [CarDeliveryStatus]          SMALLINT      NULL,
    [CarDeliveryDate]            DATETIME      NULL,
    [TC_UsersId]                 INT           NULL,
    [LatestCarName]              VARCHAR (130) NULL,
    [LastCallComment]            VARCHAR (MAX) NULL,
    [InquiryCreationDate]        DATETIME      NULL,
    [BookingEntryDate]           DATETIME      NULL,
    [BookingAmt]                 DECIMAL (18)  NULL,
    [PromisedDeliveryDate]       DATETIME      NULL,
    [DeliveryEntryDate]          DATETIME      NULL,
    [PanNo]                      VARCHAR (50)  NULL,
    [VinNO]                      VARCHAR (50)  NULL,
    [CompanyName]                VARCHAR (150) NULL,
    [EngineNumber]               VARCHAR (100) NULL,
    [InsuranceCoverNumber]       VARCHAR (100) NULL,
    [InvoiceNumber]              VARCHAR (100) NULL,
    [RegistrationNo]             VARCHAR (100) NULL,
    [BookingCancelDate]          DATETIME      NULL,
    [InvoiceDate]                DATE          NULL,
    [CarVersionId]               INT           NULL,
    [InquiryCityId]              INT           NULL,
    [BookedCarColourId]          INT           NULL,
    [BookedCarMakeYear]          INT           NULL,
    [BookingName]                VARCHAR (50)  NULL,
    [BookingMobile]              VARCHAR (50)  NULL,
    [BookingEmail]               VARCHAR (100) NULL,
    [InquiryDispositionDate]     DATETIME      NULL,
    [ExchangeCarVersionId]       INT           NULL,
    [ExchangeCarKms]             INT           NULL,
    [ExchangeCarMakeYear]        DATE          NULL,
    [ExchangeCarExpectedPrice]   INT           NULL,
    [DealerCampaignSchedulingId] INT           NULL,
    [NSCCampaignSchedulingId]    INT           NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_CreatedDate_DealerId]
    ON [dbo].[TC_LeadBasedSummary]([CreatedDate] ASC)
    INCLUDE([DealerId], [TC_LeadId], [Eagerness], [EagernessId], [Source], [TC_LeadStageId], [TC_LeadDispositionID], [ScheduledOn], [TestDriveDate], [TestDriveStatus]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_DealerId_CreatedDate]
    ON [dbo].[TC_LeadBasedSummary]([DealerId] ASC, [CreatedDate] ASC)
    INCLUDE([TC_LeadId], [Eagerness], [EagernessId], [Source], [TC_LeadStageId], [TC_LeadDispositionID], [ScheduledOn], [TestDriveDate], [TestDriveStatus]);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_CarDeliveryDate]
    ON [dbo].[TC_LeadBasedSummary]([CarDeliveryDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_ScheduledOn]
    ON [dbo].[TC_LeadBasedSummary]([ScheduledOn] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_BookingDate]
    ON [dbo].[TC_LeadBasedSummary]([BookingDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_TC_LeadId]
    ON [dbo].[TC_LeadBasedSummary]([TC_LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_TC_NewCarInquiriesId]
    ON [dbo].[TC_LeadBasedSummary]([TC_NewCarInquiriesId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_TestDriveDate]
    ON [dbo].[TC_LeadBasedSummary]([TestDriveDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_InvoiceDate]
    ON [dbo].[TC_LeadBasedSummary]([InvoiceDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_InquiryCreationDate]
    ON [dbo].[TC_LeadBasedSummary]([InquiryCreationDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_DealerId]
    ON [dbo].[TC_LeadBasedSummary]([DealerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_CarDeliveryStatus]
    ON [dbo].[TC_LeadBasedSummary]([CarDeliveryStatus] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_LeadBasedSummary_Organization]
    ON [dbo].[TC_LeadBasedSummary]([Organization] ASC);

