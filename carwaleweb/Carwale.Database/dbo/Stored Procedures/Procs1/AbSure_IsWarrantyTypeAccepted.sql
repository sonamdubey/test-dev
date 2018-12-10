IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_IsWarrantyTypeAccepted]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_IsWarrantyTypeAccepted]
GO

	
-- ===============================================================================================
-- Create date: 14/01/2015
-- Description:	This SP checks whether warranty type is assigned or not
-- [AbSure_IsWarrantyTypeAccepted] 4
-- Modified By : Nilima More on 21th sept 2015
-- Description : status = 9 OR car with EXPIRED CERTIFICATE should not have Approve/Reject option.
--			     status = 9 but doubtful reason in 3 or 4 should have Approve/Reject option.
-- Modified By : Nilima More on 3rd November 2015
-- Description : Status = 3 cancelled car should not have Approve/Reject option.
-- ==================================================================================================
CREATE PROCEDURE [dbo].[AbSure_IsWarrantyTypeAccepted] @AbSure_CarDetailsId INT
AS
BEGIN
	DECLARE @CertificateExpiryDays INT = NULL
		    ,@SurveyDate DATETIME = NULL

	SELECT @SurveyDate = SurveyDate
	FROM   AbSure_CarDetails WITH(NOLOCK)
	WHERE  Id = @AbSure_CarDetailsId

	SELECT @CertificateExpiryDays = DATEDIFF(DD, @SurveyDate, GETDATE())

	SELECT FinalWarrantyTypeId
		   ,IsRejected
	FROM   AbSure_CarDetails ACD WITH(NOLOCK) LEFT JOIN AbSure_DoubtfulCarReasons ADC WITH(NOLOCK) ON ACD.Id = ADC.AbSure_CarDetailsId
	WHERE  ACD.Id = @AbSure_CarDetailsId
		AND (
			IsRejected = 1						  --Rejected
			OR FinalWarrantyTypeId IS NOT NULL    --Approved
			OR (ACD.STATUS NOT IN (1) AND ((DoubtfulReason = 1 OR DoubtfulReason = 2)AND ADC.IsActive = 1) )          -- Reason in 1 or 2
			OR (ACD.Status = 9 AND ((ADC.DoubtfulReason = 1 OR ADC.DoubtfulReason = 2) AND ADC.IsActive = 1) )    --Onhold
			OR @CertificateExpiryDays > 30     --Expired certificate
			OR (ACD.STATUS = 3)     --Cancelled car
			)
		
END



------------------------------------------------------------------------------------------------------------------------------------




