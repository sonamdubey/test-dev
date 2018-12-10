CREATE TABLE [dbo].[Acc_SellerAdditionals] (
    [Id]             NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AdditionalInfo] VARCHAR (50) NOT NULL,
    [IsActive]       BIT          CONSTRAINT [DF_Accessories_SellerAdditionals_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_Accessories_SellerAdditionals] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

