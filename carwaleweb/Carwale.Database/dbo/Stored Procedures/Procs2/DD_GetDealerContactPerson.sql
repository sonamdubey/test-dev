IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetDealerContactPerson]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetDealerContactPerson]
GO

	



-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <5/11/2014>
-- Description:	<Get Dealer Contact Person>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetDealerContactPerson] 
	@DD_OutletId	INT
AS
BEGIN
	SELECT CP.Id ,CP.Salutation ,CP.FirstName , CP.LastName , DG.Designation , CP.EmailId , OU.UserName , CP.CreatedOn
	FROM DD_ContactPerson CP 
	LEFT JOIN DD_Designations DG WITH(NOLOCK) ON DG.Id = CP.DD_DesignationsId
	INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CP.CreatedBy
	WHERE CP.DD_OutletId = @DD_OutletId
	ORDER BY CP.Id DESC
END

