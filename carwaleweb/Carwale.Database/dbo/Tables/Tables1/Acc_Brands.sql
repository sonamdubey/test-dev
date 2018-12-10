CREATE TABLE [dbo].[Acc_Brands] (
    [Id]           NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BrandName]    VARCHAR (100) NOT NULL,
    [LogoUrl]      VARCHAR (100) NULL,
    [WebUrl]       VARCHAR (100) NULL,
    [TollFreeNo]   VARCHAR (15)  NULL,
    [IsActive]     BIT           CONSTRAINT [DF_Accessories_Brands_IsActive] DEFAULT (1) NOT NULL,
    [IsReplicated] BIT           DEFAULT ((1)) NULL,
    [HostURL]      VARCHAR (100) DEFAULT ('img.carwale.com') NULL,
    CONSTRAINT [PK_Accessories_Brands] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

