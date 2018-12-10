CREATE TABLE [UCAlert].[RecommendUsedCarAlert] (
    [CustomerId]             INT           NULL,
    [CustomerName]           VARCHAR (150) NULL,
    [CustomerAlert_Email]    VARCHAR (100) NULL,
    [ProfileId]              VARCHAR (50)  NULL,
    [Car_SellerId]           INT           NULL,
    [Car_SellerType]         TINYINT       NULL,
    [Car_CityId]             INT           NULL,
    [Car_City]               VARCHAR (50)  NULL,
    [Car_Price]              INT           NULL,
    [Car_MakeId]             INT           NULL,
    [Car_Make]               VARCHAR (50)  NULL,
    [Car_ModelId]            INT           NULL,
    [Car_Model]              VARCHAR (60)  NULL,
    [Car_Kms]                INT           NULL,
    [Car_Version]            VARCHAR (60)  NULL,
    [Car_Year]               DATETIME      NULL,
    [Car_Color]              VARCHAR (50)  NULL,
    [Car_HasPhoto]           BIT           NULL,
    [Car_LastUpdated]        DATETIME      NULL,
    [Is_Mailed]              BIT           NULL,
    [alertUrl]               VARCHAR (100) NULL,
    [CreatedOn]              DATETIME      NULL,
    [Rank]                   SMALLINT      NULL,
    [ImageUrl]               VARCHAR (200) NULL,
    [CustomerAlert_Make]     VARCHAR (80)  NULL,
    [CustomerAlert_Model]    VARCHAR (100) NULL,
    [UsedCarAlertAlgoTypeId] TINYINT       NULL,
    [IsFirstMail]            BIT           NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_RecommendUsedCarAlert_CustomerId]
    ON [UCAlert].[RecommendUsedCarAlert]([CustomerId] ASC);

