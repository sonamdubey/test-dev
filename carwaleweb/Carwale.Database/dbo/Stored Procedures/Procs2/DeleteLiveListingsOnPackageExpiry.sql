IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteLiveListingsOnPackageExpiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteLiveListingsOnPackageExpiry]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author: Surendra
-- Create date: 13/12/2011
-- Description: This procedure will be used when dealer's package will expire
-- Modified By:  Manish on 26-07-2013 for maintaining log of the removed car
-- Modified By: Deepak on 3rd Spet 2013  Decativate Dealers Where Package has been expired.
-- Modified By: Avishkar on 10-4-2014  Added SP ComputeLLSortScore To set lead score for all live listings
-- Modified by: Manish on changed the sp ComputeLLSortScore to ComputeLLSortScoreDailyExecution.
-- Modified by: Vaibhav K 22 Sept 2016 Update delaer status on package expiry only in case of non-migrated dealers
-- Modified by: Manish for used car migration on 11-11-2016 04:55 am
-- =============================================
CREATE PROCEDURE [dbo].[DeleteLiveListingsOnPackageExpiry]

-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--BEGIN TRY
	    --BEGIN TRANSACTION DeleteLiveListings

		-- Following query will delete all record form livelisting table for those record packege expiry 
		-- date is less then cureent datetime(FOR SELLER TYPE =2 =INDIVIDUAL)
		
		----  Added By Avishkar(13-11-2013) to set IsPremium=0 for Dealers whos Package has been expired.
		-- NOT Needed
		--UPDATE CustomerSellInquiries
		--SET IsPremium=0
		--FROM  LiveListings AS LL WITH (NOLOCK)
		--JOIN CustomerSellInquiries as CSI on CSI.ID = LL.InquiryId
		--WHERE LL.SellerType = 2
		--AND CONVERT(VARCHAR(8),CSI.ClassifiedExpiryDate,112) < CONVERT(VARCHAR(8),GETDATE(),112)
		
		UPDATE CSI
		SET CSI.ClassifiedExpiryDate = DATEADD(day,P.RenewValidity,GETDATE())
		,CSI.IsPremium = 0
		FROM CustomerSellInquiries CSI WITH(NOLOCK)
		JOIN Packages P WITH(NOLOCK) ON CSI.PackageId = P.ID
		WHERE CONVERT(VARCHAR(8),CSI.ClassifiedExpiryDate,112) < CONVERT(VARCHAR(8),GETDATE(),112)
		AND CSI.IsPremium = 1
		
		UPDATE LL
		SET LL.IsPremium = 0
		FROM livelistings LL WITH(NOLOCK)
		JOIN CustomerSellInquiries CSI WITH(NOLOCK) ON LL.Inquiryid = CSI.ID
		WHERE LL.SellerType = 2 AND CSI.PackageType = 31 AND CSI.IsPremium = 0

		DELETE LiveListings 
		FROM LiveListings AS LL WITH(NOLOCK) 
		JOIN CustomerSellInquiries as CSI WITH(NOLOCK)  on CSI.ID = LL.InquiryId
		WHERE LL.SellerType = 2
		AND CONVERT(VARCHAR(8),CSI.ClassifiedExpiryDate,112) < CONVERT(VARCHAR(8),GETDATE(),112)

		

	-------------------------Below insert statement add by Manish on 26-07-2013 for maintaining log of the removed car-----------
					INSERT INTO TC_StockUploadedLog(SellInquiriesId, 
													DealerId,
													IsCarUploaded,
													CreatedOn)
											SELECT  CSI.ID,
													CSI.DealerId,
													0,
													GETDATE()
											 FROM  LiveListings AS LL WITH (NOLOCK)
											 JOIN  SellInquiries AS CSI WITH (NOLOCK)  on CSI.ID = LL.InquiryId
											 WHERE LL.SellerType = 1
											  AND  CONVERT(VARCHAR(8),CSI.PackageExpiryDate,112)< CONVERT(VARCHAR(8),GETDATE(),112)




	   ---------------------------------------------------------------------------------------------------------------------------------------------	
		
		-- Following query will delete all record form livelisting table for those record packege 
		-- expiry date is less then current datetime(FOR SELLER TYPE =1 =DEALER)

		----  Added By Avishkar(13-11-2013) to set IsPremium=0 for Dealers whos Package has been expired.
		-- NOT Needed
		--UPDATE SellInquiries
		--SET IsPremium=0
		--FROM  LiveListings AS LL WITH (NOLOCK)
		--JOIN  SellInquiries AS CSI WITH (NOLOCK)  on CSI.ID = LL.InquiryId
		--WHERE LL.IsPremium=1
		--AND LL.SellerType = 1
		--AND  CONVERT(VARCHAR(8),CSI.PackageExpiryDate,112)< CONVERT(VARCHAR(8),GETDATE(),112)


		DELETE LiveListings FROM LiveListings AS LL 
		JOIN SellInquiries AS CSI with(NOLOCK) on CSI.ID = LL.InquiryId
		WHERE LL.SellerType = 1 AND CSI.SourceId=2
		AND  CONVERT(VARCHAR(8),CSI.PackageExpiryDate,112)< CONVERT(VARCHAR(8),GETDATE(),112)
		
			
		-- fOLLOWING QUERY WILL UPDATE tc_stock table and make them suspend(status=4)
		UPDATE TC_Stock 
		SET  --StatusId=4,
		IsSychronizedCW=0
		FROM TC_Stock ST  with(NOLOCK)
		Join ConsumerCreditPoints as cc with (nolock) on cc.ConsumerId=st.BranchId and cc.ConsumerType=1
		 -- JOIN SellInquiries SI  with(NOLOCK) ON ST.Id=SI.TC_StockId
		WHERE  CONVERT(VARCHAR(8),cc.ExpiryDate,112) = CONVERT(VARCHAR(8),GETDATE()-1,112) 
		
		
		
		
		-- Decativate Dealers Where Package has been expired.
		-- Added by Deepak on 3rd Spet 2013
		-- Vaibhav K 22 Sept 2016 left join CWCTDealerMapping table and checked for IsMigrated flag
		UPDATE Dealers SET Status = 1 
		WHERE ID IN(
		SELECT ConsumerId FROM ConsumerCreditPoints CCP with(NOLOCK) 
		LEFT JOIN CWCTDealerMapping CMP with(NOLOCK) ON CCP.ConsumerId = CMP.CWDealerID
		WHERE ConsumerType = 1 AND DATEDIFF(DD, ExpiryDate, GETDATE()) >= 1
		AND ISNULL(CMP.IsMigrated,0) = 0
		)
		AND  Status = 0 AND TC_DealerTypeId = 1
		

		--Modified By: Avishkar on 10-4-2014  To set lead score for all live listings

		--EXEC [dbo].[ComputeLLSortScoreDailyExecution] 

	--COMMIT TRANSACTION DeleteLiveListings
	--END TRY
	--BEGIN CATCH
	--	ROLLBACK TRANSACTION DeleteLiveListings
	--	 INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date,InputParameters)
 --        VALUES('DeleteLiveListingsOnPackageExpiry',(ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),GETDATE(),'No Params')			
	--END CATCH;
	
END
