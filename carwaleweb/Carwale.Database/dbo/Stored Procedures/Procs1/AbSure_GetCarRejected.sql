IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarRejected]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarRejected]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: Aug 17,2015
-- Description:	Get if car is rejected or not
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCarRejected] 		
		@AbSure_CarDetailsId BIGINT,	
		@DealerId BIGINT = NULL,	
		@IsAlreadyNotRejected BIT = NULL OUTPUT
AS
BEGIN

		SELECT ACD.ID
		FROM   AbSure_CarDetails ACD LEFT JOIN AbSure_DoubtfulCarReasons ADC ON ACD.Id = ADC.AbSure_CarDetailsId
		WHERE ACD.Id = @AbSure_CarDetailsId AND (Status IN(2,3,4,8) OR (Status = 9 AND (ADC.DoubtfulReason = 1 OR ADC.DoubtfulReason = 2) AND ADC.IsActive =1 ))
		
		IF @@ROWCOUNT > 0 
		BEGIN
		--PRINT '1'
			SET @IsAlreadyNotRejected = 0
		END
		ELSE
		BEGIN
		--PRINT '2'
			IF ISNULL(@DealerId, 0) > 0
			BEGIN
			--PRINT '3'
				SET @IsAlreadyNotRejected = 1
			END
		END	
			
									
END