IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockInventoryExcelDownload]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockInventoryExcelDownload]
GO

	

-- =============================================
-- Author:		VISHAL SRIVASTAVA AE 1830
-- Create date: 3 JANUARY 2014 1144 HRS IST
-- Description:	This Proc RETURNS DETAILED REPORT IN EXCEL FORMAT.
-- TC_StockInventoryExcelDownload 9,null,1
-- =============================================
create PROCEDURE [dbo].[TC_StockInventoryExcelDownload]
	@UserId    BIGINT,
	@BranchId  BIGINT = NULL,
	@IsSpecialUser BIT = 0  --From dealership and 1 for special user.
AS
BEGIN
	DECLARE @SpecialUserId INT = @UserId
	IF(@IsSpecialUser=0)
	BEGIN
		SET @IsSpecialUser = 0
		SET @SpecialUserId = NULL
	END
	ELSE
	BEGIN
		SET @IsSpecialUser = 1
	END 

	SELECT
			I.ModelCode AS [Model Code],
			V.Model AS [Model Name],
			V.[Version] AS [Variant Name],
			I.ColourCode AS [Color Code],
			VCC.Color AS [Color Name],
			I.PrCodes AS [PrCodes],
			I.Region AS Region,
			I.ChassisNumber AS [Chassis Number],
			I.DealerCompanyName AS [Dealer: Company Name],
			D.Organization AS [Selling Dealer],
			I.DealerLocation AS [Dealer: Location],
			ISNULL(I.PaymentDealerInvoiceDate,'--') AS [Payment: Dealer Invoice Date],
			I.ModelYear AS [Model Year],
			ISNULL(I.CheckpointDate,'--') AS [Checkpoint 8 Date (long)],
			ISNULL(I.WholesaleDate,'--') AS [Wholesale Date],
			CASE 
				WHEN (N.ChassisNumber IS NULL OR N.BookingStatus=31) THEN '--' 
				--WHEN N.BookingStatus=32 AND N.InvoiceDate IS NULL THEN 'Booked'
				WHEN (N.InvoiceDate IS NOT NULL AND N.DeliveryDate IS NULL) THEN 'Retailed' ELSE 'Delivered' 
			END AS [Status],
			CASE
				WHEN (CONVERT(VARCHAR(11), I.EntryDate, 106) IS NULL) THEN '--'
				ELSE CONVERT(VARCHAR(11), I.EntryDate, 106) 
			END AS [Stock Uploaded date],
			ISNULL(CONVERT(VARCHAR(11), I.EntryDate, 106),'--') AS [Stock Uploaded date],
			ISNULL(CONVERT(VARCHAR(11), N.InvoiceDate, 106),'--') AS [Autobiz Retail Date],
			ISNULL(CONVERT(VARCHAR(11), N.DeliveryDate, 106),'--') AS [Autobiz Delivery Date]
	FROM	TC_StockInventory I WITH(NOLOCK)
			INNER JOIN	TC_vwVersionColorCode	VCC WITH (NOLOCK)
												ON VCC.CarVersionCode=I.ModelCode AND VCC.ColorCode=I.ColourCode
			INNER JOIN	vwMMV	V WITH (NOLOCK)
								ON V.VersionId=VCC.CarVersionId
			INNER JOIN	Dealers D WITH (NOLOCK)
								ON I.BranchId = D.Id
			LEFT JOIN	TC_NewCarBooking AS N  WITH (NOLOCK)
											ON I.ChassisNumber=N.ChassisNumber
	WHERE  				
			(((@IsSpecialUser = 1) AND(I.IsSpecialUser=@IsSpecialUser))
			OR ((@IsSpecialUser = 0) AND(I.BranchId=@BranchId)))
	ORDER BY N.BookingStatus, (N.DeliveryEntryDate-N.InvoiceDate) 
END

