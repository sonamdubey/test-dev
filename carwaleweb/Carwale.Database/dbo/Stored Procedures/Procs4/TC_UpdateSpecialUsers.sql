IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateSpecialUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateSpecialUsers]
GO

	      
CREATE PROCEDURE [dbo].[TC_UpdateSpecialUsers]
@Id						NUMERIC,       
@SameReportTo			BIT,    
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
@Status					NUMERIC OUTPUT      
      
AS      
       
BEGIN    
	IF (@SameReportTo = 1)
		BEGIN
			UPDATE TC_SpecialUsers
						SET UserName = @UserName, Password = @Password, Mobile = @Mobile, 
								ModifiedDate = GETDATE(), ModifiedBy = @ModifiedBy,
								ReportingType = @ReportingType, Designation = @Designation
						WHERE TC_SpecialUsersId = @Id
			SET @Status = 1 
						
		END
	ELSE
		BEGIN
	
			DECLARE
				@Old_Root AS HIERARCHYID,
				@new_root AS HIERARCHYID,
				@New_Manager_Hid AS HIERARCHYID,
				@Current_Manager AS INT 
		  
			SET @Current_Manager = (SELECT ReportsTo FROM TC_SpecialUsers WHERE TC_SpecialUsersId = @Id)
			
			IF (@Current_Manager = @ReportsTo)
				BEGIN
					UPDATE TC_SpecialUsers
					SET UserName = @UserName, Password = @Password, Mobile = @Mobile, 
								ModifiedDate = GETDATE(), ModifiedBy = @ModifiedBy,
								ReportingType = @ReportingType, Designation = @Designation
					WHERE TC_SpecialUsersId = @Id
					SET @Status = 1 
				END	
			ELSE	      
				BEGIN TRAN

					SET @New_Manager_Hid = (SELECT HierId FROM TC_SpecialUsers WITH (UPDLOCK) WHERE TC_SpecialUsersId = @ReportsTo);
					SET @Old_Root = (SELECT HierId FROM TC_SpecialUsers WHERE TC_SpecialUsersId = @Id);

					-- First, get a new hid for employee that moves
					SET @new_root = @New_Manager_Hid.GetDescendant
					((SELECT MAX(HierId)
					  FROM TC_SpecialUsers
					  WHERE HierId.GetAncestor(1) = @New_Manager_Hid),
					 NULL);

					-- Next, reparent all descendants of employee that moves
					UPDATE TC_SpecialUsers
						SET HierId = HierId.GetReparentedValue(@old_root, @new_root)
					WHERE HierId.IsDescendantOf(@old_root) = 1;
					  
					UPDATE TC_SpecialUsers
					SET ReportsTo =@ReportsTo ,UserName = @UserName, Password = @Password, Mobile = @Mobile, 
								ModifiedDate = GETDATE(), ModifiedBy = @ModifiedBy,
								ReportingType = @ReportingType, Designation = @Designation
					WHERE TC_SpecialUsersId = @Id
					  
					SET @Status = 1 

				COMMIT TRAN
		END
END			
				  
      
      
      
      


