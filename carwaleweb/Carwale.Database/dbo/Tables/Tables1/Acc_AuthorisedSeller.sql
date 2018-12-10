CREATE TABLE [dbo].[Acc_AuthorisedSeller] (
    [Id]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [SellerId]     NUMERIC (18) NOT NULL,
    [BrandId]      NUMERIC (18) NOT NULL,
    [IsAuthorised] BIT          NOT NULL,
    [IsActive]     CHAR (10)    CONSTRAINT [DF_Accessories_AuthorisedSeller_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Acc_AuthorisedSeller] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

