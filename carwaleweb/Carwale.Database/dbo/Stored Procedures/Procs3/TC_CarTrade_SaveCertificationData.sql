IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_SaveCertificationData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_SaveCertificationData]
GO

	
-- =============================================
-- Author      : Chetan Navin
-- Create date : 24th Dec 2015
-- Description : To save car certification data returned from cartrade
-- Modified By : Chetan Navin on 14th Mar 2016 (Added condition to handle duplicate insertion)
-- =============================================
CREATE PROCEDURE [dbo].[TC_CarTrade_SaveCertificationData] 
	@TC_CarTradeCertificationData  dbo.TC_CarTradeCertificationDetails_v1 READONLY,
	@RetId INT = NULL OUTPUT
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION ProcessCertificationData
	BEGIN
	DECLARE @StockId INT
	SELECT @StockId = ListingId FROM @TC_CarTradeCertificationData 
	IF NOT EXISTS(SELECT 1 FROM TC_CarTradeCertificationData TC WITH (NOLOCK) 
	              INNER JOIN TC_CarTradeCertificationLiveListing TL WITH (NOLOCK) ON  TL.ListingId = TC.ListingId
				  WHERE TC.ListingId =  @StockId)
		BEGIN
			INSERT INTO TC_CarTradeCertificationData 
			SELECT * FROM @TC_CarTradeCertificationData AS nc;

			SET @RetId = SCOPE_IDENTITY()
		END
	ELSE
	   BEGIN
			UPDATE TC_CarTradeCertificationData 
			SET IsWarranty = TD.IsWarranty 
			FROM @TC_CarTradeCertificationData TD
			WHERE TC_CarTradeCertificationData.ListingId = TD.ListingId 

			SET @RetId = 0
	   END
		--Update certification live listing
		--IF NOT EXISTS(SELECT 1 FROM TC_CarTradeCertificationLiveListing WITH(NOLOCK) WHERE ListingId = @StockId)
		--BEGIN
		--	INSERT INTO TC_CarTradeCertificationLiveListing (ListingId,TC_CarTradeCertificationRequestId)
		--	VALUES(@StockId,(SELECT TOP 1 TC_CarTradeCertificationRequestId 
		--	                 FROM TC_CarTradeCertificationRequests WITH(NOLOCK) WHERE ListingId = @StockId
		--					 ORDER BY EntryDate DESC ))
		--END	
		--Change status
		EXEC AbSure_ChangeCertification @StockId
	END
	COMMIT TRANSACTION ProcessAbSure_SaveCarDetails
    END TRY
	BEGIN CATCH
        ROLLBACK TRANSACTION ProcessCertificationData
		INSERT INTO TC_Exceptions
                      (Programme_Name,
                       TC_Exception,
                       TC_Exception_Date,
                       InputParameters)
         VALUES('TC_CarTrade_SaveCertificationData',
         (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),
         GETDATE(),NULL
         )                        
	END CATCH;
END
-----------------------------------------------------------------------------------------------
