CREATE TABLE [dbo].[Customers] (
    [Id]                   NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]                 VARCHAR (100)  NOT NULL,
    [email]                VARCHAR (100)  NOT NULL,
    [Address]              VARCHAR (250)  NULL,
    [StateId]              INT            NULL,
    [CityId]               NUMERIC (18)   NULL,
    [AreaId]               NUMERIC (18)   NULL,
    [phone1]               VARCHAR (20)   NULL,
    [phone2]               VARCHAR (20)   NULL,
    [Mobile]               VARCHAR (20)   NULL,
    [password]             VARCHAR (20)   NULL,
    [PrimaryPhone]         TINYINT        CONSTRAINT [DF_Customers_PrimaryPhone] DEFAULT (1) NOT NULL,
    [Industry]             VARCHAR (100)  NULL,
    [Designation]          VARCHAR (100)  NULL,
    [Organization]         VARCHAR (100)  NULL,
    [DOB]                  DATETIME       NULL,
    [ContactHours]         VARCHAR (50)   NULL,
    [ContactMode]          VARCHAR (10)   NULL,
    [CurrentVehicle]       VARCHAR (200)  NULL,
    [InternetUsePlace]     VARCHAR (20)   NULL,
    [CarwaleContact]       VARCHAR (200)  NULL,
    [InternetUseTime]      SMALLINT       NULL,
    [ReceiveNewsletters]   BIT            CONSTRAINT [DF_Customers_ReceiveNewsletters] DEFAULT (1) NULL,
    [RegistrationDateTime] DATETIME       NULL,
    [IsEmailVerified]      BIT            CONSTRAINT [DF_Customers_IsEmailVerified] DEFAULT (0) NOT NULL,
    [IsVerified]           BIT            CONSTRAINT [DF_Customers_IsVerificationDone] DEFAULT (0) NOT NULL,
    [TempId]               NUMERIC (18)   NULL,
    [IsFake]               BIT            CONSTRAINT [DF_Customers_IsFake] DEFAULT (0) NOT NULL,
    [IsTelephonic]         BIT            CONSTRAINT [DF_Customers_IsTelephonic] DEFAULT (0) NOT NULL,
    [Comment]              VARCHAR (1500) NULL,
    [EEmpType]             VARCHAR (50)   NULL,
    [SourceId]             SMALLINT       CONSTRAINT [DF_Customers_SourceId] DEFAULT ((1)) NOT NULL,
    [MailerCount]          NUMERIC (18)   CONSTRAINT [DF_Customers_MailerCount] DEFAULT ((0)) NOT NULL,
    [LastCampaign]         VARCHAR (100)  NULL,
    [LastEMailOn]          DATETIME       CONSTRAINT [DF_Customers_LastEMailOn] DEFAULT ('2000-01-01 00:00:00.000') NULL,
    [IsMailBounced]        BIT            DEFAULT ((0)) NULL,
    [PwdSaltHashStr]       VARCHAR (100)  NULL,
    [FbId]                 BIGINT         NULL,
    [GplusId]              NVARCHAR (50)  NULL,
    [OAuth]                VARCHAR (50)   NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Customers__email]
    ON [dbo].[Customers]([email] ASC)
    INCLUDE([IsFake]);


GO
CREATE NONCLUSTERED INDEX [ix_Customers__IsVerified__IsFake]
    ON [dbo].[Customers]([IsVerified] ASC, [IsFake] ASC)
    INCLUDE([Id], [Name], [email], [CityId], [Mobile], [RegistrationDateTime]);


GO
CREATE NONCLUSTERED INDEX [ix_Customers__Mobile]
    ON [dbo].[Customers]([Mobile] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Customers__RegistrationDateTime]
    ON [dbo].[Customers]([RegistrationDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Customers_IsFake_Email]
    ON [dbo].[Customers]([IsFake] ASC)
    INCLUDE([email]);


GO
CREATE NONCLUSTERED INDEX [IX_customers_cityid]
    ON [dbo].[Customers]([CityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Customers_OAuth]
    ON [dbo].[Customers]([OAuth] ASC);

