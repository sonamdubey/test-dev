CREATE TABLE [dbo].[Dealer_NewCar_Bkp25112015] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MakeId]            NUMERIC (18)  NULL,
    [CityId]            NUMERIC (18)  NULL,
    [Name]              VARCHAR (100) NULL,
    [Address]           VARCHAR (500) NULL,
    [Pincode]           VARCHAR (50)  NULL,
    [ContactNo]         VARCHAR (200) NULL,
    [FaxNo]             VARCHAR (50)  NULL,
    [EMailId]           VARCHAR (100) NULL,
    [WebSite]           VARCHAR (100) NULL,
    [WorkingHours]      VARCHAR (50)  NULL,
    [LastUpdated]       DATETIME      NULL,
    [IsActive]          BIT           CONSTRAINT [DF_Dealer_NewCar_IsActive] DEFAULT ((1)) NULL,
    [IsNCD]             BIT           CONSTRAINT [DF_Dealer_NewCar_IsNCD] DEFAULT ((0)) NOT NULL,
    [IsReplicated]      BIT           CONSTRAINT [DF__Dealer_Ne__IsRep__4618F0C5] DEFAULT ((1)) NULL,
    [HostURL]           VARCHAR (100) CONSTRAINT [DF__Dealer_Ne__HostU__686E08C9] DEFAULT ('img.carwale.com') NULL,
    [TcDealerId]        NUMERIC (18)  NULL,
    [ContactPerson]     VARCHAR (200) NULL,
    [DealerMobileNo]    VARCHAR (50)  NULL,
    [NCSId]             NUMERIC (18)  NULL,
    [Mobile]            VARCHAR (10)  NULL,
    [IsPriority]        BIT           CONSTRAINT [DF_Dealer_NewCar_IsPriority] DEFAULT ((0)) NULL,
    [IsNewDealer]       BIT           NULL,
    [PackageStartDate]  DATETIME      NULL,
    [PackageEndDate]    DATETIME      NULL,
    [CampaignId]        INT           NULL,
    [IsPremium]         BIT           NULL,
    [Latitude]          FLOAT (53)    NULL,
    [Longitude]         FLOAT (53)    NULL,
    [LeadPanelDealerId] INT           NULL,
    [ShowroomStartTime] VARCHAR (30)  NULL,
    [ShowroomEndTime]   VARCHAR (30)  NULL,
    [PrimaryMobileNo]   VARCHAR (20)  NULL,
    [SecondaryMobileNo] VARCHAR (20)  NULL,
    [LandLineNo]        VARCHAR (30)  NULL,
    [DealerArea]        VARCHAR (100) NULL,
    [IsCampaignActive]  BIT           NULL,
    [PqCampaignId]      INT           NULL,
    CONSTRAINT [PK_Dealer_NewCar] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_Dealer_NewCar_CityId]
    ON [dbo].[Dealer_NewCar_Bkp25112015]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Dealer_NewCar_TcDealerId]
    ON [dbo].[Dealer_NewCar_Bkp25112015]([TcDealerId] ASC);

