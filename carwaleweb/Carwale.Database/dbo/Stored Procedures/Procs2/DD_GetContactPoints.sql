IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetContactPoints]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetContactPoints]
GO

	


-------------------------------------------------------------------------------------------

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <10/11/2014>
-- Description:	<Get Contact Points>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetContactPoints] 
	@DealerOutletId	INT
AS
BEGIN
	SELECT ContactNumber ,DD_dealeroutletId as OutletId,IsPrimary,ContactType,DD_ContactPersonId FROM DD_ContactPoints 
	WHERE DD_DealerOutletId = CASE WHEN @DealerOutletId = -1 THEN DD_DealerOutletId ELSE @DealerOutletId END 	
END

