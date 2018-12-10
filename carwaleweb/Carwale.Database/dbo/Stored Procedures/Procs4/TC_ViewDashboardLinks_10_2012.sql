IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ViewDashboardLinks_10_2012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ViewDashboardLinks_10_2012]
GO

	
-- ========================================================================
-- Author:		TEJASHREE PATIL
-- Create date: 18 April 2012
-- Description:	Retrieve Oraganization name and allows access to dashboad 
-- depends upon logged Dealer's DealerAdminId.
-- ========================================================================

 CREATE PROCEDURE [dbo].[TC_ViewDashboardLinks_10_2012]
	-- Add the parameters for the stored procedure here
	@DealerId int,-- DealerId Passed from QueryString
	@DealerIdLogged int,--DealerId of Logged Dealer
	@OrganizationName varchar(60) OUTPUT
AS
 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS ( SELECT DealerId FROM TC_DealerAdminMapping
				WHERE DealerId=@DealerId AND DealerAdminId IN(SELECT DealerAdminId 
				FROM TC_DealerAdminMapping WHERE DealerId=@DealerIdLogged))  
				--Check for DealerId from list of SuperAdmin's outlet is belongs to Logged Dealer(i.e super Admin's
				--DealerAdminId) or not.
		BEGIN
			SET @OrganizationName =(SELECT DB.Organization 'Branch' FROM Dealers DB 
				WHERE DB.ID IN(SELECT DealerId 	FROM TC_DealerAdminMapping 
				WHERE DealerId=@DealerId)GROUP BY DB.Id,DB.Organization)
				--Retrieve Organization name of logged dealer
			RETURN 1
		END
	ELSE 
		BEGIN
			RETURN 0
		END
END

 





