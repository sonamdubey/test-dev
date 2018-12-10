IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TempDeleteLiveListingsOnPackageExpiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TempDeleteLiveListingsOnPackageExpiry]
GO

	CREATE PROCEDURE [dbo].[TempDeleteLiveListingsOnPackageExpiry]
(@Dealerid INT)
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
	    BEGIN TRANSACTION DeleteLiveListings
	    
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
											  AND  CSI.DealerId=@Dealerid

	

		DELETE LiveListings FROM LiveListings AS LL 
		JOIN SellInquiries AS CSI on CSI.ID = LL.InquiryId
		WHERE LL.SellerType = 1
		 AND  CSI.DealerId=@Dealerid
		
			
		-- fOLLOWING QUERY WILL UPDATE tc_stock table and make them suspend(status=4)
		UPDATE TC_Stock 
		SET StatusId=4,IsSychronizedCW=0
		FROM TC_Stock ST
		  JOIN SellInquiries SI ON ST.Id=SI.TC_StockId
		WHERE     SI.DealerId=@Dealerid AND SI.StatusId=1
		
		
		-- Decativate Dealers Where Package has been expired.
		-- Added by Deepak on 3rd Spet 2013
		UPDATE Dealers SET Status = 1 
		WHERE ID =@Dealerid
		AND  Status = 0		

		

	COMMIT TRANSACTION DeleteLiveListings
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION DeleteLiveListings
		 INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date,InputParameters)
         VALUES('DeleteLiveListingsOnPackageExpiry',(ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),GETDATE(),'No Params')			
	END CATCH;
	
END
