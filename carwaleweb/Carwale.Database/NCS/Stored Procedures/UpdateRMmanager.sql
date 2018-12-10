IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[NCS].[UpdateRMmanager]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [NCS].[UpdateRMmanager]
GO

	-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 18th April 2012
-- Description:	It will update the Rmangers table with new Subtree hierarchy
-- Modifier	  : Amit Kumar(removed IsExecutive And added Type) 
-- =============================================
CREATE PROCEDURE [NCS].[UpdateRMmanager]
	@Empid    AS NUMERIC,
	@NewMgrid AS NUMERIC,
	@OrgName  VARCHAR(100),    
	@Designation  VARCHAR(100),      
	@EMail  VARCHAR(250),      
	@MobileNo  VARCHAR(50),         
	@MakeId   NUMERIC,
	@CityId   NUMERIC,
	@MakeGroupId NUMERIC,        
	@LoginId  VARCHAR(50),      
	@PassWord  VARCHAR(50),      
	@UpdatedBy  NUMERIC,      
	@IsActive  BIT,      
	@UpdatedDate DATETIME,
	@OprUserId		NUMERIC(18,0) = null, 
	--@IsExecutive  BIT,  
	@Type		SMALLINT,  
	@SameMngr  INT,
	@Status   NUMERIC OUTPUT    
AS
BEGIN
	IF (@SameMngr =1)
	BEGIN
		UPDATE NCS_RManagers
					SET Name= @OrgName ,Designation = @Designation ,EMail =@EMail,MobileNo = @MobileNo,MakeId = @MakeId  ,
								CityId = @CityId ,MakeGroupId = @MakeGroupId ,LoginId = @LoginId  ,PassWord = @PassWord  ,UpdatedBy = @UpdatedBy  ,      
								IsActive = @IsActive  ,UpdatedDate = @UpdatedDate , Type = @Type  ,OprUserID = @OprUserId
					WHERE Id=@Empid
		SET @Status = 1 
					
	END
	ELSE
	
	BEGIN
	
	DECLARE
		  @old_root AS HIERARCHYID,
		  @new_root AS HIERARCHYID,
		  @new_mgr_hid AS HIERARCHYID,
		  @Current_mgr AS INT 
		  
		  SET @Current_mgr = (SELECT ReportTo FROM NCS_RManagers WHERE Id=@Empid)
		  
		  IF (@Current_mgr = @NewMgrid)
			BEGIN
				SET @Status = 0
			END	
	   ELSE	      

			BEGIN TRAN

					SET @new_mgr_hid = (SELECT HierId FROM NCS_RManagers WITH (UPDLOCK)
						  WHERE Id = @NewMgrid);
					SET @old_root = (SELECT HierId FROM NCS_RManagers
					   WHERE Id = @Empid);

				  -- First, get a new hid for employee that moves
				  SET @new_root = @new_mgr_hid.GetDescendant
					((SELECT MAX(HierId)
					  FROM NCS_RManagers
					  WHERE HierId.GetAncestor(1) = @new_mgr_hid),
					 NULL);

				  -- Next, reparent all descendants of employee that moves
				  UPDATE NCS_RManagers
					SET HierId = HierId.GetReparentedValue(@old_root, @new_root)
				  WHERE HierId.IsDescendantOf(@old_root) = 1;
				  
				   UPDATE NCS_RManagers
					SET ReportTo =@NewMgrid ,Name= @OrgName ,Designation = @Designation ,EMail =@EMail,MobileNo = @MobileNo,MakeId = @MakeId  ,
								CityId = @CityId ,MakeGroupId = @MakeGroupId ,LoginId = @LoginId  ,PassWord = @PassWord  ,UpdatedBy = @UpdatedBy  ,      
								IsActive = @IsActive  ,UpdatedDate = @UpdatedDate , Type = @Type ,OprUserID = @OprUserId 
					WHERE Id=@Empid
				  
				  SET @Status = 1 

			COMMIT TRAN
		END
	END
