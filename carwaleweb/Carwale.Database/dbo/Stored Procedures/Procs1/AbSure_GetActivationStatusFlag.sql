IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetActivationStatusFlag]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetActivationStatusFlag]
GO

	-- =============================================
-- Author      : Nilima More 
-- created on  : 29th Oct 2015
-- Description : To get warranty accepted cars.
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetActivationStatusFlag] 		
		@AbSure_CarDetailsId INT,
		@IsAlreadyAccepted BIT = NULL OUTPUT
AS
BEGIN
		DECLARE @OrigCarId INT=NULL,@RegNo VARCHAR(50)=NULL

		SELECT @RegNo = RegNumber FROM AbSure_CarDetails WITH(NOLOCK) WHERE ID=@AbSure_CarDetailsId
		SELECT @OrigCarId = dbo.Absure_GetMasterCarId(@RegNo,@AbSure_CarDetailsId)

		SELECT ID
		FROM AbSure_CarDetails WITH(NOLOCK) 
		WHERE Id = @OrigCarId AND STATUS =4 AND AbSureWarrantyActivationStatusesId = 2 AND IsActive = 1

		
		IF @@ROWCOUNT > 0 
		BEGIN
			SET @IsAlreadyAccepted = 1
		END
		ELSE
		BEGIN
			BEGIN
				SET @IsAlreadyAccepted = 0
			END
		END	
			
									
END

