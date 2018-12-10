CREATE TABLE [dbo].[MM_SellerMobileMaskingLogBkp] (
    [ConsumerId]    NUMERIC (18) NULL,
    [ConsumerType]  TINYINT      NULL,
    [MaskingNumber] VARCHAR (20) NULL,
    [Mobile]        VARCHAR (20) NULL,
    [CreatedOn]     DATETIME     CONSTRAINT [DF_MM_SellerMobileMaskingLog_CreatedOn] DEFAULT (getdate()) NULL,
    [DeletedOn]     DATETIME     NULL,
    [DeletedType]   VARCHAR (50) NULL,
    [IsDeleted]     BIT          NULL,
    [ProductTypeId] SMALLINT     NULL,
    [NCDBrandId]    INT          NULL
);

