IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetUserDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetUserDealers]
GO

	
-- =============================================
--	Author	:	Sachin Bharti(11th Dec 2013)
--	Description	:	To get all daelers assigned to user
--					for opr mobile website
--	Modifier	:	Sachin Bharti(1st December 2014)
--	Purpose		:	Commented user dealers condition . Now we will
--					get all the Dealers Name
-- =============================================
CREATE PROCEDURE [dbo].[M_GetUserDealers] 
	@UserId		NUMERIC(18,0)
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT DISTINCT D.Organization AS Text FROM Dealers D(NOLOCK)
	INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.DealerId = D.ID
	WHERE DAU.UserId = @UserId
	--WHERE IsDealerActive=1
	ORDER BY Organization 
END


