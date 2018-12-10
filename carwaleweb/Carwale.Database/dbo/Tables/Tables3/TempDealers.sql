CREATE TABLE [dbo].[TempDealers] (
    [ID]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LoginId]            VARCHAR (30)  NULL,
    [FirstName]          VARCHAR (50)  NULL,
    [LastName]           VARCHAR (50)  NULL,
    [EmailId]            VARCHAR (60)  NULL,
    [Organization]       VARCHAR (60)  NOT NULL,
    [Address1]           VARCHAR (100) NULL,
    [Address2]           VARCHAR (100) NULL,
    [AreaId]             NUMERIC (18)  NULL,
    [CityId]             NUMERIC (18)  NULL,
    [StateId]            NUMERIC (18)  NULL,
    [Pincode]            VARCHAR (6)   NULL,
    [PhoneNo]            VARCHAR (15)  NULL,
    [FaxNo]              VARCHAR (15)  NULL,
    [MobileNo]           VARCHAR (15)  NULL,
    [EntryDate]          DATETIME      CONSTRAINT [DF_TempDealers_EntryDate] DEFAULT (getdate()) NOT NULL,
    [WebsiteUrl]         VARCHAR (60)  NULL,
    [ContactPerson]      VARCHAR (60)  NULL,
    [ContactHours]       VARCHAR (60)  NULL,
    [ContactEmail]       VARCHAR (60)  NULL,
    [LogoUrl]            VARCHAR (100) NULL,
    [ROLE]               VARCHAR (50)  CONSTRAINT [DF_TempDealers_ROLE] DEFAULT ('DEALERS') NULL,
    [IsReplicated]       BIT           CONSTRAINT [DF__TempDeale__IsRep__5467101C] DEFAULT ((1)) NULL,
    [HostURL]            VARCHAR (100) CONSTRAINT [DF__TempDeale__HostU__76BC2820] DEFAULT ('img.carwale.com') NULL,
    [UpdatedBy]          NUMERIC (18)  NULL,
    [Source]             NUMERIC (18)  CONSTRAINT [DF_TempDealers_Source] DEFAULT ((1)) NULL,
    [IsDeleted]          BIT           CONSTRAINT [DF_TempDealers_IsDeleted] DEFAULT ((0)) NULL,
    [LeadSource]         SMALLINT      CONSTRAINT [DF_TempDealers_LeadSource] DEFAULT ((1)) NULL,
    [ReferredBy]         INT           NULL,
    [VerificationPoolId] BIGINT        NULL,
    [DeletedOn]          DATETIME      NULL,
    [IsExistingCW]       BIT           NULL,
    [DeletedReason]      INT           NULL,
    [DeleteComment]      VARCHAR (100) NULL,
    [TC_DealerTypeId]    TINYINT       NULL,
    [ApplicationId]      INT           DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TempDealers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_TempDealers_EmailId]
    ON [dbo].[TempDealers]([EmailId] ASC);

