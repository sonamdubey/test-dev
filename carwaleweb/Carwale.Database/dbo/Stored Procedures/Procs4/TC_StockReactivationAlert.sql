IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockReactivationAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockReactivationAlert]
GO

	-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 14 Dec,2011
-- Description:	This procedure is used for checking suspended stock and their is no active stock
-- Modified By: Tejashree Patil On 5 July 2012: WITH (NOLOCK)implementation 
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockReactivationAlert]
(
@BranchId NUMERIC,
@ActiveStockCount INT OUTPUT,--record count in livelistings table
--@UploadLimit INT=0 OUTPUT,
@SuspendedCount INT OUTPUT -- Record count in Tc_stock table with status =4
)	
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @ActiveStockCount=COUNT(LL.Inquiryid) 
	FROM LiveListings LL WITH (NOLOCK)
	INNER JOIN SellInquiries SI WITH (NOLOCK) ON LL.Inquiryid=SI.ID
	JOIN TC_StockStatus as TSS WITH (NOLOCK) ON SI.StatusId=TSS.Id and TSS.Status='Available' 
	WHERE SI.DealerId=@BranchId 
	--AND SI.StatusId=1 
	AND LL.SellerType=1
	
	--if(@ActiveStockCount=0)
	--BEGIN
		SELECT @SuspendedCount=COUNT(ST.Id) 
		FROM TC_Stock as ST WITH (NOLOCK)
		    JOIN TC_StockStatus as TSS WITH (NOLOCK) ON ST.StatusId=TSS.Id and TSS.Status='Suspended' 
		WHERE BranchId=@BranchId
		--AND SI.StatusId=4		
		AND ST.IsActive=1
	--END	

END


