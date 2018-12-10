IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerMobile]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerMobile]
GO
	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author:  Umesh Ojha  
-- Create date: 23 jul 2012  
-- Description: Getting All STock count for dealer  
-- Modified By Vivek Gupta on 28-03-2014 , commented Previous query and added new query for websitecontactmobile extraction.
-- Modified By : Suresh Prajapti on 11th Mar, 2016
-- Description : Removed Password Check
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetDealerMobile] (
	@BranchId BIGINT
	,@UserId VARCHAR(50)
	,@Password VARCHAR(50) = NULL
	)
AS
BEGIN
	-- interfering with SELECT STatements.  
	SET NOCOUNT ON;

	DECLARE @DealerId BIGINT = NULL

	SELECT @DealerId = DealerId
	FROM TC_APIUsers WITH (NOLOCK)
	WHERE UserId = @UserId
		--AND Password = @Password
		AND IsActive = 1

	IF (@DealerId = @BranchId)
	BEGIN
		--select MobileNo from dealers with (nolock) where ID=@BranchId
		SELECT WebsiteContactMobile
		FROM dealers WITH (NOLOCK)
		WHERE ID = @BranchId
	END
END

