IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveWarrantyType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveWarrantyType]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Jan 9,2015
-- Description:	To save Type of Warranty
-- Modified By Tejashree Patil on 3 March 2015 , Eligibility checked for LPG and CNG.
-- Modified By Tejashree Patil on 3 March 2015, Updated CarScore and Warranty in livelisting.
-- Modified By Ruchira Patil on 28th Aug 2015, Added Try Catch
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveWarrantyType] 
		@FinalWarrantyTypeId INT,
		@WarrantyGivenBy BIGINT,
		@AbSure_CarDetailsId BIGINT,
		@IsApproved BIT,
		@DealerId BIGINT = NULL,		
		@IsAlreadyNotApprovedOrRejected BIT = NULL OUTPUT
AS
BEGIN
	BEGIN TRY

        BEGIN TRANSACTION ProcessAbSure_SaveWarrantyType
		SELECT ACD.ID
		FROM   AbSure_CarDetails ACD LEFT JOIN AbSure_DoubtfulCarReasons ADC ON ACD.Id = ADC.AbSure_CarDetailsId
		WHERE ACD.Id = @AbSure_CarDetailsId AND (Status IN(2,3,4,8) OR (Status = 9 AND ( (ADC.DoubtfulReason = 1 OR ADC.DoubtfulReason = 2) AND ADC.IsActive =1 )))
		
		IF @@ROWCOUNT > 0 
		BEGIN
		--PRINT '2'		
		SET @IsAlreadyNotApprovedOrRejected = 0
		END

		ELSE
		BEGIN
			UPDATE	AbSure_CarDetails 
			SET	FinalWarrantyTypeId = @FinalWarrantyTypeId,
				WarrantyGivenBy = @WarrantyGivenBy,
				IsRejected = CASE WHEN @IsApproved = 1 THEN 0 ELSE 1 END,
				RejectedDateTime = CASE WHEN @IsApproved = 1 THEN NULL ELSE GETDATE() END,
				FinalWarrantyDate = GETDATE()
			WHERE	Id = @AbSure_CarDetailsId
			SET @IsAlreadyNotApprovedOrRejected = 1
		END
		
			
		/*************************** Modified By Tejashree Patil on 3 March 2015 to update Absure certification based on criteria *****************************/
		
		EXECUTE AbSure_ChangeCertification NULL, NULL, @AbSure_CarDetailsId

		/********************************************************/

        COMMIT TRANSACTION ProcessAbSure_SaveWarrantyType
    END TRY
	
	BEGIN CATCH
        ROLLBACK TRANSACTION ProcessAbSure_SaveWarrantyType
         INSERT INTO TC_Exceptions
                      (Programme_Name,
                       TC_Exception,
                       TC_Exception_Date,
                       InputParameters)
         VALUES('AbSure_SaveWarrantyType',
         (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),
         GETDATE(),
            ',        @FinalWarrantyTypeId :'           + ISNULL(CAST( @FinalWarrantyTypeId AS VARCHAR(50)),'NULL')+
            ',        @WarrantyGivenBy :'               + ISNULL(CAST( @WarrantyGivenBy AS VARCHAR(50)),'NULL')+
            ',        @AbSure_CarDetailsId :'           + ISNULL(CAST( @AbSure_CarDetailsId AS VARCHAR(50)),'NULL')+
            ',        @IsApproved :'					+ ISNULL(CAST( @IsApproved AS VARCHAR(50)),'NULL')+
            ',        @DealerId :'						+ ISNULL(CAST( @DealerId AS VARCHAR(50)),'NULL')+
            ',        @IsAlreadyNotApprovedOrRejected :'+ ISNULL(CAST( @IsAlreadyNotApprovedOrRejected AS VARCHAR(50)),'NULL')
         )
    END CATCH;						
END
-------------------------------------------------------------------------------------------------------------------------

