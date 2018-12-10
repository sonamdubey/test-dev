IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadVerificationScheduling]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadVerificationScheduling]
GO

	-- =============================================   
-- Author:  Manish   
-- Create date: 14-Jan-12   
-- Description: Scheduling unverified Leads to User.
-- Modified By : Surendra On 11-02-2013, Desc -Lead divertion for single user is different
-- Modified By : Tejashree Patil on 13 May 2013, Commented LeadVerificationScheduling for Single User Dealership.
-- Modified By : Manish Chourasiya 20 May 2013, UnCommented LeadVerificationScheduling for Single User Dealership.For handling the case from multiuser became single user
-- Modified By : Nilesh Utture 16th Dec, 2013 Added condition of roleId to get count of users in dealership
-- Modified By : Khushaboo Patil on 1st June 2016 Added condition in case of dealer stopped automatic lead flow assign leads to dealer principle
-- =============================================   
CREATE PROCEDURE [dbo].[TC_LeadVerificationScheduling] @TC_Usersid AS INT, 
                                                      @DealerId   AS INT 
AS 
BEGIN 
	IF EXISTS(SELECT TOP 1 Id FROM TC_Users WITH(NOLOCK) WHERE Id=@TC_Usersid AND IsCarwaleUser=0 AND IsActive=1)
	BEGIN
		DECLARE @DealerPrincipleUserId INT = NULL
		DECLARE @CntUserInDealership SMALLINT
		SELECT	@CntUserInDealership = COUNT(DISTINCT(U.Id)) 
		FROM	TC_Users  U WITH(NOLOCK)
				INNER JOIN TC_UsersRole R WITH(NOLOCK) ON R.UserId=U.Id
		WHERE	BranchId=@DealerId 
				AND IsActive=1 
				AND IsCarwaleUser=0
				AND	R.RoleId IN (4,5,6) -- Modified By : Nilesh Utture 16th Dec, 2013 
		
		IF(@CntUserInDealership>1) -- Muliti User Dealership
			BEGIN

				-- Modified By : Khushaboo Patil on 1st June 2016 Added condition in case of dealer stopped automatic lead flow assign leads to dealer principle
				IF EXISTS(SELECT MDF.Id FROM 
				TC_MappingDealerFeatures MDF WITH(NOLOCK)
				INNER JOIN Dealers D WITH(NOLOCK) ON MDF.BranchId = D.ID
				WHERE BranchId = @DealerId AND TC_DealerFeatureId = 8 AND D.TC_DealerTypeId IN (2,3))				
				BEGIN
					SELECT TOP 1 @DealerPrincipleUserId = U.ID 
					FROM TC_Users  U WITH(NOLOCK)
					INNER JOIN TC_UsersRole R WITH(NOLOCK) ON R.UserId=U.Id
					WHERE BranchId = @DealerId 
					AND IsActive = 1 
					AND IsCarwaleUser = 0
					AND	R.RoleId = 1
					ORDER BY U.Id
				END

				IF(@DealerPrincipleUserId IS NOT NULL)
					EXECUTE TC_LeadVerificationSchedulingForSingleUser @DealerPrincipleUserId, @DealerId
				ELSE
					EXECUTE TC_LeadVerificationSchedulingForMultiUser @TC_Usersid, @DealerId
			END
		ELSE --Single User Dealership
			BEGIN 
				EXECUTE TC_LeadVerificationSchedulingForSingleUser @TC_Usersid, @DealerId
			END
	END
END
----------------------------------------------------------------------------------------------------------------------------------------------------------------------

