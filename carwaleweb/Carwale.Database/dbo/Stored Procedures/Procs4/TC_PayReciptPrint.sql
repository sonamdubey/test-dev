IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PayReciptPrint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PayReciptPrint]
GO

	-- Author: Surendra
-- Create date: 12th Oct,2011
-- Description: This procedure will return all the details to print receipt
-- =============================================
CREATE PROCEDURE [dbo].[TC_PayReciptPrint]
(
@TC_PaymentReceived_Id BIGINT,
@RegNo VARCHAR(20) OUTPUT,
@CustName VARCHAR(50) OUTPUT,
@CustAddress VARCHAR(100) OUTPUT,
@CustMobile VARCHAR(100) OUTPUT,
@CustPin VARCHAR(10) OUTPUT,
@PayDate DATE OUTPUT,
@ChequeNo VARCHAR(20) OUTPUT,
@ChequeDate DATE OUTPUT,
@BankName VARCHAR(50) OUTPUT,
@RecAmount INT OUTPUT,
@PayMode VARCHAR(10) OUTPUT,
@DealerAddress VARCHAR(100)OUTPUT,
@DealerPhone VARCHAR(20)OUTPUT,
@DealerPin VARCHAR(10)OUTPUT,
@DealerFax VARCHAR(10)OUTPUT,
@DealerWebsite VARCHAR(50)OUTPUT,
@CarName VARCHAR(100)OUTPUT,
@StockId INT OUTPUT
)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

SELECT @RegNo=ST.RegNo,@CustName=CD.CustomerName,@CustMobile=CD.Mobile,@CustPin=CD.Pincode,
@CustAddress=CD.Address ,@RecAmount=PR.AmountReceived,
@PayDate=PR.PayDate,@ChequeNo=PR.ChequeNo,@ChequeDate=PR.ChequeDate,@BankName=PR.BankName,
@DealerAddress=DL.Address1,@DealerFax=DL.FaxNo,@DealerPhone=DL.PhoneNo,@DealerPin=DL.Pincode,
@DealerWebsite=DL.WebsiteUrl,@PayMode=PO.OptionName,@CarName=( CM.Name + ' ' + MO.Name + ' ' + VR.Name ),@StockId=ST.Id
FROM TC_CarBooking CB
INNER JOIN TC_PaymentReceived PR ON CB.TC_CarBookingId=PR.TC_CarBooking_Id
INNER JOIN TC_PaymentOptions PO ON PR.TC_PaymentOptions_Id=PO.TC_PaymentOptions_Id
INNER JOIN TC_CustomerDetails CD ON CB.CustomerId=CD.Id
INNER JOIN TC_Stock ST ON CB.StockId=ST.Id
INNER JOIN Dealers DL ON ST.BranchId=DL.ID
LEFT JOIN CarVersions VR On VR.Id=ST.VersionId
LEFT JOIN CarModels MO On Mo.Id=VR.CarModelId
LEFT JOIN CarMakes CM On CM.Id=Mo.CarMakeId
WHERE PR.TC_PaymentReceived_Id=@TC_PaymentReceived_Id
--AND ST.StatusId=1
END
