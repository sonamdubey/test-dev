IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CreateSpecialUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CreateSpecialUsers]
GO

	      
CREATE PROCEDURE [dbo].[TC_CreateSpecialUsers]           
@UserName				VARCHAR(100),   
@EMail					VARCHAR(100),   
@PassWord				VARCHAR(50),
@Mobile					VARCHAR(50),
@MakeId					NUMERIC,
@IsActive				BIT,
@IsFirstTimeLoggedIn	BIT,
@ReportingType			TINYINT,
@Designation			TINYINT,      
@ReportsTo				INT,
@ModifiedBy				INT,
@CityId					NUMERIC,   
@AliasUserId			INT,    
@Status					NUMERIC OUTPUT      
      
AS      
       
BEGIN    
	DECLARE
			@HierarchyId			AS HIERARCHYID,
			@Manager_HierarchyId	AS HIERARCHYID,
			@Last_Child_HierarchyId AS HIERARCHYID;
			
	IF @ReportsTo = NULL
		SET @HierarchyId = HIERARCHYID::GetRoot();
	ELSE
		BEGIN
			SET @Manager_HierarchyId = (SELECT HierId FROM TC_SpecialUsers WITH (UPDLOCK) WHERE TC_SpecialUsersId = @ReportsTo);
			SET @Last_Child_HierarchyId =(SELECT MAX(HierId) FROM TC_SpecialUsers WHERE HierId.GetAncestor(1) = @Manager_HierarchyId);
			SET @HierarchyId = @Manager_HierarchyId.GetDescendant(@Last_Child_HierarchyId, NULL);
		END
		
		IF EXISTS (SELECT TC_SpecialUsersId FROM TC_SpecialUsers WHERE EMail = @Email)
			BEGIN
				SET @Status = 0
			END	
	   ELSE	      
		   BEGIN      
				INSERT INTO TC_SpecialUsers(UserName, Email, Password, Mobile, Sex, DOB, Address, MakeId, IsActive, 
							IsFirstTimeLoggedIn, GCMRegistrationId, EntryDate, ModifiedDate, ModifiedBy,
							ReportingType, Designation, ReportsTo, HierId, AliasUserId)
							
				VALUES(@UserName, @Email, @Password, @Mobile, NULL, NULL, NULL, @MakeId, 1, 
							0, NULL, GETDATE(), NULL, @ModifiedBy, 
							@ReportingType, @Designation, @ReportsTo, @HierarchyId, @AliasUserId)
											
				SET @Status = SCOPE_IDENTITY()      
				
				IF @AliasUserId IS NULL
					UPDATE TC_SpecialUsers SET AliasUserId = @Status WHERE TC_SpecialUsersId = @Status
		   END      
		   
END      
      
      
      
      


