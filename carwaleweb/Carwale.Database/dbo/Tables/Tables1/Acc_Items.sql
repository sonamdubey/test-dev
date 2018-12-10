CREATE TABLE [dbo].[Acc_Items] (
    [Id]                   NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BrandId]              NUMERIC (18)   NOT NULL,
    [ProductId]            NUMERIC (18)   NOT NULL,
    [Title]                VARCHAR (100)  NOT NULL,
    [OEM_Number]           VARCHAR (50)   NULL,
    [Dimension_Length]     INT            NULL,
    [Dimension_Width]      INT            NULL,
    [Dimension_Height]     INT            NULL,
    [ShippingWeight]       VARCHAR (50)   NULL,
    [MRP]                  NUMERIC (18)   NOT NULL,
    [InclusiveOfTax]       SMALLINT       NOT NULL,
    [FeaturesLocation]     VARCHAR (50)   NULL,
    [DescriptionLocation]  VARCHAR (50)   NULL,
    [PrimaryImageLocation] VARCHAR (50)   NULL,
    [ReviewRating]         DECIMAL (9, 2) CONSTRAINT [DF_Acc_Items_ReviewRating] DEFAULT ((0)) NULL,
    [ReviewCount]          NUMERIC (9)    CONSTRAINT [DF_Acc_Items_ReviewCount] DEFAULT ((0)) NULL,
    [EntryDate]            DATETIME       NOT NULL,
    [IsActive]             BIT            CONSTRAINT [DF_Accessories_Items_IsActive] DEFAULT ((1)) NOT NULL,
    [IsReplicated]         BIT            CONSTRAINT [DF__Acc_Items__IsRep__5EAF9465] DEFAULT ((0)) NULL,
    [HostURL]              VARCHAR (100)  DEFAULT ('img.carwale.com') NULL,
    [IsDeleted]            BIT            CONSTRAINT [DF_Acc_Items_IsDeleted] DEFAULT ((0)) NULL,
    [ProductFeature]       VARCHAR (MAX)  NULL,
    [ProductDecription]    VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Accessories_Items] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [ix_Acc_Items_BrandId_ProductId_IsActive]
    ON [dbo].[Acc_Items]([BrandId] ASC, [ProductId] ASC, [IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_Acc_IsActive]
    ON [dbo].[Acc_Items]([IsActive] ASC);

