IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealDealerDetails]
GO

	
-- =============================================
-- Author:		Anchal Gupta
-- Create date: 11-01-2015
-- Description:	updating the deals dealer details in tc_DEALS_DEALERS from dcrm
-- select * from TC_Deals_Dealers
-- Modified By: Tejashree Patil on 19 Jan 2016, Added roleId = 18 for Deals dealer's dealerprinciple.
---- =============================================
CREATE PROCEDURE [dbo].[UpdateDealDealerDetails]
	-- Add the parameters for the stored procedure here
	@dealerId INT
	,@contactMobile VARCHAR(100)
	,@contactEmail VARCHAR(200)
	,@isDealerDealActive BIT
	,@user INT
AS
BEGIN
	IF EXISTS (
			SELECT dealerId
			FROM TC_Deals_Dealers WITH (NOLOCK)
			WHERE dealerId = @dealerId
			)
		UPDATE TC_Deals_Dealers
		SET ContactEmail = @contactEmail
			,ContactMobile = @contactMobile
			,IsDealerDealActive = @isDealerDealActive
		WHERE DealerId = @dealerId
	ELSE
	BEGIN
		IF (@isDealerDealActive = 1)
		BEGIN
			INSERT INTO TC_Deals_Dealers (
				DealerId
				,EnteredOn
				,EnteredBy
				,IsDealerDealActive
				,ContactEmail
				,ContactMobile
				)
			VALUES (
				@dealerId
				,GETDATE()
				,@user
				,@isDealerDealActive
				,@contactEmail
				,@contactMobile
				)

			-- Modified By: Tejashree Patil on 19 Jan 2016, Added roleId = 18 for Deals dealer's dealerprinciple.
			DECLARE @TCUserId INT
			SELECT	@TCUserId = Id
			FROM	TC_Users U WITH (NOLOCK)
					INNER JOIN TC_UsersRole UR WITH (NOLOCK) ON UR.UserId = U.Id
			WHERE	BranchId = @dealerId
					AND IsActive = 1 
					AND UR.RoleId = 1--Dealer Principle role
	
			INSERT INTO TC_UsersRole(UserId,RoleId)
			VALUES(@TCUserId,18)
			
		END
	END

END
