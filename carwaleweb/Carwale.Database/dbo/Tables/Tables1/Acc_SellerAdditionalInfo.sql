CREATE TABLE [dbo].[Acc_SellerAdditionalInfo] (
    [Id]                  NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SellerId]            NUMERIC (18) NOT NULL,
    [SellerAdditionalsId] NUMERIC (18) NOT NULL,
    [IsActive]            BIT          CONSTRAINT [DF_Accessories_SellerAdditionalInfo_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Acc_SellerAdditionalInfo] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

