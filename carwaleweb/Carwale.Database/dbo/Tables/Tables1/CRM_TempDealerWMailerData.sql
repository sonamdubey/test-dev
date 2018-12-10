CREATE TABLE [dbo].[CRM_TempDealerWMailerData] (
    [DealerId] NUMERIC (18)  NOT NULL,
    [Name]     VARCHAR (150) NULL,
    [EMail]    VARCHAR (500) NULL,
    [LoginId]  VARCHAR (50)  NULL,
    [Password] VARCHAR (50)  NULL,
    [City]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_CRM_TempDealerWMailerData] PRIMARY KEY CLUSTERED ([DealerId] ASC)
);

