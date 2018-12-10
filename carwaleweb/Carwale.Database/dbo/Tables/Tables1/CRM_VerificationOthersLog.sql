CREATE TABLE [dbo].[CRM_VerificationOthersLog] (
    [Id]                   NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [LeadId]               NUMERIC (18)   NOT NULL,
    [DealerId]             NUMERIC (18)   NULL,
    [DealerName]           VARCHAR (250)  NULL,
    [PurchaseTime]         INT            NULL,
    [UpdatedOn]            DATETIME       NOT NULL,
    [UpdatedBy]            NUMERIC (18)   NOT NULL,
    [Eagerness]            INT            NULL,
    [IsPEDone]             BIT            CONSTRAINT [DF_CRM_VerificationOthersLog_IsPEDone] DEFAULT ((0)) NOT NULL,
    [PurchaseMode]         SMALLINT       NULL,
    [PurchaseOnNameType]   SMALLINT       NULL,
    [PurchaseOnName]       VARCHAR (50)   NULL,
    [CurrentCarOwned]      VARCHAR (250)  NULL,
    [CallConnected]        BIT            NULL,
    [GoodTimeToTalk]       BIT            NULL,
    [Language]             VARCHAR (15)   NULL,
    [LookingForCar]        BIT            NULL,
    [IsSameMake]           BIT            NULL,
    [BuyingSpan]           INT            NULL,
    [NotConnectedReason]   INT            NULL,
    [UnavailabilityReason] INT            NULL,
    [CallBackRequest]      BIT            NULL,
    [NotIntReason]         INT            NULL,
    [IsCarBookedAlready]   BIT            NULL,
    [BookedCarVersion]     INT            NULL,
    [BookedCarDate]        DATETIME       NULL,
    [SpecialComments]      VARCHAR (5000) NULL,
    [CreatedOn]            DATETIME       NULL,
    [Occasion]             VARCHAR (50)   NULL,
    [Usage]                VARCHAR (50)   NULL,
    [UsageType]            VARCHAR (50)   NULL,
    [MonthlyUsageCity]     VARCHAR (50)   NULL,
    [CarOwnership]         VARCHAR (50)   NULL,
    [IsPriorityCall]       BIT            NULL,
    [MonthlyUsageHighway]  VARCHAR (50)   NULL,
    [BookedCarProblem]     VARCHAR (100)  NULL,
    [FuturePurchaseDate]   DATETIME       NULL,
    [IsFuturePlanToBuy]    BIT            NULL,
    [PurchaseContact]      VARCHAR (15)   NULL,
    [CompanyName]          VARCHAR (50)   NULL,
    [IsMultipleBooking]    BIT            NULL,
    [BookingCount]         SMALLINT       NULL,
    [IsCarCompare]         BIT            CONSTRAINT [DF_CRM_VerificationOthersLog_IsCarCompare] DEFAULT ((0)) NULL,
    [IsHDFC]               BIT            NULL,
    CONSTRAINT [PK_CRM_VerificationOthersLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_VerificationOthersLog_LeadId]
    ON [dbo].[CRM_VerificationOthersLog]([LeadId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_VerificationOthersLogUpdatedOn]
    ON [dbo].[CRM_VerificationOthersLog]([UpdatedOn] ASC)
    INCLUDE([LeadId], [UnavailabilityReason], [NotIntReason]);


GO

CREATE TRIGGER [dbo].[TRG_CRM_VerificationOthersLog] 
ON [dbo].[CRM_VerificationOthersLog]
AFTER INSERT 
AS
IF update(PurchaseTime)
BEGIN
declare @LeadId bigint
select @LeadId=LeadId
from inserted 
EXEC CRM.LSUpdateLeadScore 2, @LeadId, -1
END;


GO
DISABLE TRIGGER [dbo].[TRG_CRM_VerificationOthersLog]
    ON [dbo].[CRM_VerificationOthersLog];

