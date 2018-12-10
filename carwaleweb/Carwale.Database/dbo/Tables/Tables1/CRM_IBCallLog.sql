CREATE TABLE [dbo].[CRM_IBCallLog] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [MobileNumber]  VARCHAR (20) NOT NULL,
    [Source]        NUMERIC (18) NOT NULL,
    [IsUsedCar]     BIT          NULL,
    [IsNewCar]      BIT          NULL,
    [NCLeadId]      NUMERIC (18) NULL,
    [SellInquiryId] NUMERIC (18) NULL,
    [LeadDate]      DATETIME     CONSTRAINT [DF_CRM_IBCallLog_LeadDate] DEFAULT (getdate()) NULL,
    [LeadBy]        NUMERIC (18) NOT NULL,
    [IsOutbound]    BIT          CONSTRAINT [DF_CRM_IBCallLog_IsOutbound] DEFAULT ((0)) NOT NULL,
    [SubSource]     INT          NULL,
    CONSTRAINT [PK_CRM_IBCallLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

