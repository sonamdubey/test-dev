CREATE TABLE [dbo].[BA_BrokerRegistrationReq] (
    [ID]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [BrokerMobile]     VARCHAR (50)   NOT NULL,
    [BrokerName]       NVARCHAR (50)  NULL,
    [Email]            NVARCHAR (50)  NULL,
    [CityId]           INT            NULL,
    [DateOfJoining]    DATETIME       NULL,
    [DateOfActivation] DATETIME       NULL,
    [Comments]         NVARCHAR (500) NULL,
    [RegisterBrokerID] BIGINT         NULL,
    [IsActive]         BIT            CONSTRAINT [DF_BA_BrokerRegistrationReq_IsActive] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BA_BrokerRegistrationReq] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [Mobile_BA_BrokerRegistrationReq] UNIQUE NONCLUSTERED ([BrokerMobile] ASC)
);

