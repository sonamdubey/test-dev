IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_SaveRManager]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_SaveRManager]
GO

	


      
CREATE PROCEDURE [dbo].[NCS_SaveRManager]      
@ID    NUMERIC,      
@OrgName  VARCHAR(100),    
@Designation  VARCHAR(100),      
@EMail  VARCHAR(50),      
@MobileNo  VARCHAR(50),    
@NodeCode VARCHAR(100),      
@ReportTo NUMERIC, 
@ReportToCurrent VARCHAR(50),   
@MakeId   NUMERIC,
@CityId   NUMERIC,
@MakeGroupId NUMERIC,        
@LoginId  VARCHAR(50),      
@PassWord  VARCHAR(50),      
@UpdatedBy  NUMERIC,      
@IsActive  BIT,      
@UpdatedDate DATETIME, 
@IsExecutive  BIT,      
@Status   NUMERIC OUTPUT      
      
AS      
       
BEGIN      
 IF @ID = -1      
  
	   IF EXISTS (SELECT Id FROM NCS_RManagers WHERE LoginId = @LoginId)
			BEGIN
				SET @Status = 0
			END	
	   ELSE	      
		   BEGIN      
				INSERT INTO NCS_RManagers (Name, MakeId, CityId, MakeGroupId, LoginId,PassWord, UpdatedDate, UpdatedBy, IsActive, Designation, EMail, MobileNo, ReportTo, NodeCode, ReportToCurrent, IsExecutive)      
				VALUES (@OrgName, @MakeId,@CityId, @MakeGroupId, @LoginId, @PassWord, @UpdatedDate, @UpdatedBy, @IsActive, @Designation, @EMail, @MobileNo, @ReportTo, @NodeCode, @ReportToCurrent, @IsExecutive)      
				
				SET @Status = SCOPE_IDENTITY()      
		   END      
 ELSE      
	  BEGIN      
			 IF EXISTS (SELECT Id FROM NCS_RManagers WHERE ID <> @Id AND LoginId = @LoginId)
				BEGIN
					SET @Status = 0
				END	
			 ELSE	     
				BEGIN      
					 UPDATE NCS_RManagers       
					 SET Name = @OrgName, MakeId = @MakeId, CityId = @CityId, MakeGroupId = @MakeGroupId, LoginId = @LoginId , PassWord = @PassWord,      
					 UpdatedBy = @UpdatedBy, IsActive = @IsActive, UpdatedDate = @UpdatedDate, ReportToCurrent = @ReportToCurrent,  
					 Designation = @Designation, EMail = @EMail, MobileNo = @MobileNo, ReportTo = @ReportTo, NodeCode = @NodeCode,
					 IsExecutive = @IsExecutive      
					 Where ID = @ID 
					      
					 SET @Status = 1      
				END      
	  END      
END      
      
      
      
      


