CREATE TABLE [dbo].[TC_Stock] (
    [Id]                     NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [BranchId]               INT           NOT NULL,
    [VersionId]              NUMERIC (18)  NOT NULL,
    [StatusId]               INT           NOT NULL,
    [Price]                  NUMERIC (18)  NOT NULL,
    [Kms]                    NUMERIC (18)  NOT NULL,
    [MakeYear]               DATETIME      NOT NULL,
    [Colour]                 VARCHAR (50)  NOT NULL,
    [RegNo]                  VARCHAR (50)  NOT NULL,
    [EntryDate]              DATETIME      NOT NULL,
    [LastUpdatedDate]        DATETIME      NOT NULL,
    [IsActive]               BIT           CONSTRAINT [DF_TC_Stock_IsActive] DEFAULT ((1)) NOT NULL,
    [IsSychronizedCW]        BIT           CONSTRAINT [DF_TC_Stock_IsSychronizedCW] DEFAULT ((0)) NOT NULL,
    [CertificationId]        SMALLINT      NULL,
    [IsFeatured]             BIT           CONSTRAINT [DF_TC_Stock_IsFeatured] DEFAULT ((0)) NULL,
    [IsBooked]               BIT           CONSTRAINT [DF_TC_Stock_IsBooked] DEFAULT ((0)) NULL,
    [ModifiedBy]             INT           NULL,
    [IsApproved]             BIT           CONSTRAINT [DF_TC_Stock_IsApproved] DEFAULT ((1)) NULL,
    [IsParkNSale]            BIT           DEFAULT ((0)) NULL,
    [TC_SellerInquiriesId]   BIGINT        NULL,
    [CertifiedLogoUrl]       VARCHAR (200) NULL,
    [AlternatePrice]         BIGINT        NULL,
    [StockRating]            FLOAT (53)    NULL,
    [LastStockRatingUpdate]  DATETIME      NULL,
    [SoldToCustomerFrom]     VARCHAR (200) NULL,
    [ShowEMIOnCarwale]       BIT           NULL,
    [InterestRate]           FLOAT (53)    NULL,
    [LoanToValue]            FLOAT (53)    NULL,
    [LoanAmount]             INT           NULL,
    [Tenure]                 SMALLINT      NULL,
    [OtherCharges]           INT           NULL,
    [EMI]                    INT           NULL,
    [BranchLocationId]       INT           NULL,
    [MFCSourceId]            INT           NULL,
    [IsWarrantyAccepted]     BIT           NULL,
    [WarrantyAcceptDate]     DATETIME      NULL,
    [IsWarrantyRequested]    BIT           NULL,
    [TC_ActionApplicationId] INT           NULL,
    [ISPointsGiven]          BIT           NULL,
    [PurchaseCost]           INT           NULL,
    [RefurbishmentCost]      INT           NULL,
    [SoldToCustomerId]       BIGINT        NULL,
    [VIN]                    VARCHAR (19)  NULL,
    [SuspendedDate]          DATETIME      NULL,
    CONSTRAINT [PK_TC_Stock] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Stock_StatusId]
    ON [dbo].[TC_Stock]([StatusId] ASC, [IsActive] ASC)
    INCLUDE([Id], [BranchId]);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Stock_BranchId]
    ON [dbo].[TC_Stock]([BranchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Stock_TC_SellerInquiriesId]
    ON [dbo].[TC_Stock]([TC_SellerInquiriesId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_Stock_VersionId]
    ON [dbo].[TC_Stock]([VersionId] ASC)
    INCLUDE([StatusId]);


GO
CREATE NONCLUSTERED INDEX [ix_TC_Stock__BranchId__StatusId__IsActive__IsApproved__Price]
    ON [dbo].[TC_Stock]([BranchId] ASC, [StatusId] ASC, [IsActive] ASC, [IsApproved] ASC, [Price] ASC)
    INCLUDE([Id], [VersionId], [Kms], [MakeYear], [Colour], [RegNo], [EntryDate], [LastUpdatedDate], [IsFeatured], [IsBooked]);


GO

-- =============================================
-- Author:		Vikas
-- Create date: 31/05/2011
-- Description:	Update TC_StockAnalysis.StockId as soon  as value  is inserted into TC_Stock
-- =============================================
CREATE TRIGGER [dbo].[TR_UpdateTCStockId] 
   ON  [dbo].[TC_Stock]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
    Declare @Id bigint
    Select Top 1 @Id = Id from Inserted
    Insert Into TC_StockAnalysis ( StockId ) Values ( @Id )

END


GO
DISABLE TRIGGER [dbo].[TR_UpdateTCStockId]
    ON [dbo].[TC_Stock];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'This field became true if car is booked else will be false', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_Stock', @level2type = N'COLUMN', @level2name = N'IsBooked';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cars will be shown in stock page if they are approved(1).', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TC_Stock', @level2type = N'COLUMN', @level2name = N'IsApproved';

