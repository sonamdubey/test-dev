IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ViewDashboardLinks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ViewDashboardLinks]
GO

	-- ========================================================================
-- Author:		TEJASHREE PATIL
-- Create date: 18 April 2012
-- Description:	Retrieve Oraganization name and allows access to dashboad 
-- depends upon logged Dealer's DealerAdminId.

-- Modified By:	  NILESH UTTURE
-- Modified date: 25 September, 2012
-- Description:	  Retrieve DealerTypeId
-- ========================================================================

 CREATE PROCEDURE [dbo].[TC_ViewDashboardLinks]
	-- Add the parameters for the stored procedure here
	@DealerId int,-- DealerId Passed from QueryString
	@DealerIdLogged int,--DealerId of Logged Dealer
	@OrganizationName varchar(60) OUTPUT,
	@DealerTypeId int OUTPUT
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
				
				--Modified BY Nilesh Utture on 25 September, 2012
				SET @DealerTypeId = (SELECT TC_DealerTypeId FROM Dealers WHERE ID=@DealerId)
				IF (@DealerTypeId IS NULL)
				BEGIN
					SET @DealerTypeId= 0
				END
				--Retrieve DealerTypeId of logged dealer
			RETURN 1
		END
	ELSE 
		BEGIN
			RETURN 0
		END
END




