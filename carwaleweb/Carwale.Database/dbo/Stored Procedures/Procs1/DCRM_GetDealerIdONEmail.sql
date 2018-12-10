IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealerIdONEmail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealerIdONEmail]
GO
	
CREATE PROCEDURE [dbo].[DCRM_GetDealerIdONEmail]

	@EmailId		VARCHAR(100),
	@UserId			NUMERIC,
	@DealerId 		NUMERIC = NULL OUTPUT,
	@DealerCallId	NUMERIC OUTPUT,
	@CallType       NUMERIC OUTPUT
	AS
		BEGIN
			SELECT  @DealerId = ID FROM Dealers WHERE EmailId = @EmailId
			--SELECT  @DealerCallId = ID , @CallType = CallType FROM DCRM_Calls WHERE DealerId = @DealerId
			
			SELECT @DealerCallId = DC.Id , @CallType = DC.CallType 	FROM DCRM_Calls DC
			INNER JOIN DCRM_ADM_UserDealers DAU ON DC.DealerId = DAU.DealerId 
			WHERE DC.DealerId = @DealerId  AND DAU.RoleId = 4 AND DC.ActionTakenId = 2 AND DC.UserId = @USerId
			PRINT	@DealerId
			PRINT	@DealerCallId
		END