IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetDealersTargetNotSet]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetDealersTargetNotSet]
GO

	-- Author		:	Tejashree Patil.
-- Create date	:	8 Oct 2013.
-- Description	:	This SP used to get al area dealers.
-- =============================================    
CREATE PROCEDURE [dbo].[TC_TMGetDealersTargetNotSet] 
 -- Add the parameters for the stored procedure here    
 @UserId BIGINT

AS    
BEGIN    
	DECLARE @Year SMALLINT 
	SELECT @Year = YEAR(GETDATE())

	SELECT	DISTINCT D.ID AS Value , D.Organization AS Text, D.DealerCode
    FROM	Dealers D  WITH (NOLOCK) 
	JOIN TC_BrandZone AS TCB  WITH (NOLOCK)   ON TCB.TC_BrandZoneId=D.TC_BrandZoneId AND TCB.MakeId=20
	LEFT  JOIN 	TC_DealersTarget DT  WITH (NOLOCK)  ON DT.DealerId = D.ID AND DT.[Year]= @Year
	WHERE D.TC_AMId = @UserId
	AND D.IsDealerActive=1
    AND DT.DealerId IS  NULL
	ORDER BY ID DESC
		
END
