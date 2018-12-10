IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[NCS].[SaveRManager]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [NCS].[SaveRManager]
GO

	CREATE PROCEDURE [NCS].[SaveRManager]            
@OrgName		VARCHAR(100),    
@Designation	VARCHAR(100),      
@EMail			VARCHAR(250),      
@MobileNo		VARCHAR(50),         
@Mgrid			NUMERIC, 
@MakeId			NUMERIC,
@CityId			NUMERIC,
@MakeGroupId	NUMERIC,        
@LoginId		VARCHAR(50),      
@PassWord		VARCHAR(50),      
@UpdatedBy		NUMERIC,      
@IsActive		BIT,      
@UpdatedDate	DATETIME, 
@OprUserId		NUMERIC(18,0) = NULL,
--@IsExecutive	BIT,
@Type			SMALLINT,      
@Status			NUMERIC OUTPUT      
      
AS      
       
BEGIN  
		DECLARE
			@hid     AS HIERARCHYID,
			@mgr_hid  AS HIERARCHYID,
			@last_child_hid AS HIERARCHYID;
 
 

			IF @Mgrid = NULL
				SET @hid = HIERARCHYID::GetRoot();
			ELSE
				BEGIN
						SET @mgr_hid = (SELECT HierId FROM NCS_RManagers WITH (UPDLOCK) WHERE id = @Mgrid);
						SET @last_child_hid =(SELECT MAX(HierId) FROM NCS_RManagers WHERE HierId.GetAncestor(1) = @mgr_hid);
						SET @hid = @mgr_hid.GetDescendant(@last_child_hid, NULL);
				END
    
    
  
	   IF EXISTS (SELECT Id FROM NCS_RManagers WHERE LoginId = @LoginId)
			BEGIN
				SET @Status = 0
			END	
	   ELSE	      
		   BEGIN      
				
				INSERT INTO NCS_RManagers (Name,HierId, MakeId, CityId, MakeGroupId, LoginId,PassWord, UpdatedDate, UpdatedBy, IsActive, Designation, EMail, MobileNo, ReportTo, Type,OprUserID)      
				VALUES (@OrgName,@hid, @MakeId,@CityId, @MakeGroupId, @LoginId, @PassWord, @UpdatedDate, @UpdatedBy, @IsActive, @Designation, @EMail, @MobileNo, @Mgrid, @Type,@OprUserId	)      
				
				SET @Status = SCOPE_IDENTITY()      
		   END      
 
END      
      
      
      
      


