IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[INSERTMNG]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[INSERTMNG]
GO
	
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[INSERTMNG]

(
@empid   AS VARCHAR(2000),
@mgrid   AS VARCHAR(2000)
)
AS
	BEGIN
		
		DECLARE  @empIdIndx SMALLINT,@strempId VARCHAR(1000),@mngrIdIndx SMALLINT,@strmngrId VARCHAR(1000),
			@hid     AS HIERARCHYID,
			@mgr_hid  AS HIERARCHYID,
			@last_child_hid AS HIERARCHYID;
			
			
			
			WHILE @empid <> ''       
				
				BEGIN      
					SET @empIdIndx = charindex(',',@empid)    
					SET @mngrIdIndx = CHARINDEX(',',@mgrid)   
				
					if @empIdIndx > 0   
						BEGIN
							SET @strempId = left(@empid,@empIdIndx - 1) 
							SET @empid = RIGHT(@empid,LEN(@empid)-@empIdIndx)    
							SET @strmngrId = LEFT(@mgrid,@mngrIdIndx-1)
							SET @mgrid = RIGHT(@mgrid,LEN(@mgrid)-@mngrIdIndx) 
							
							IF @strmngrId = null
									SET @hid = HIERARCHYID::GetRoot();
								ELSE
									BEGIN
										
										SET @mgr_hid = (SELECT HierId FROM NCS.RmanagersTemp WITH (UPDLOCK) WHERE id = @strmngrId);
										SET @last_child_hid =(SELECT MAX(HierId) FROM NCS.RmanagersTemp WHERE HierId.GetAncestor(1) = @mgr_hid);
										SET @hid = @mgr_hid.GetDescendant(@last_child_hid, NULL);
									END
									
									PRINT @hid.ToString()
									PRINT @strmngrId
						
							INSERT INTO NCS.RmanagersTemp (Id,Name,Designation,HierId,ReportTo,MakeId,MakeGroupId,EMail,MobileNo,LoginId,Password,IsActive,UpdatedDate,UpdatedBy,IsExecutive,CityId)
							SELECT 
								NR.Id,NR.Name,NR.Designation,@hid,@strmngrId,NR.MakeId,NR.MakeGroupId,NR.EMail,NR.MobileNo,NR.LoginId,NR.Password,NR.IsActive,NR.UpdatedDate,NR.UpdatedBy,NR.IsExecutive,NR.CityId 
							FROM NCS_RManagers AS NR WHERE NR.Id= @strempId
						
						
					END                
					ELSE       
						BEGIN
							SET @strempId = @empid  
								SET @empid = ''   
				                SET @strmngrId = @mgrid 
				                IF @strmngrId = null
									SET @hid = HIERARCHYID::GetRoot();
								ELSE
									BEGIN
										SET @mgr_hid = (SELECT HierId FROM NCS.RmanagersTemp WITH (UPDLOCK) WHERE id = @strmngrId);
										SET @last_child_hid =(SELECT MAX(HierId) FROM NCS.RmanagersTemp WHERE HierId.GetAncestor(1) = @mgr_hid);
										SET @hid = @mgr_hid.GetDescendant(@last_child_hid, NULL);
									END
									
						PRINT @hid.ToString()
						PRINT @strmngrId
						
							INSERT INTO NCS.RmanagersTemp (Id,Name,Designation,HierId,ReportTo,MakeId,MakeGroupId,EMail,MobileNo,LoginId,Password,IsActive,UpdatedDate,UpdatedBy,IsExecutive,CityId)
							SELECT 
								NR.Id,NR.Name,NR.Designation,@hid,@strmngrId,NR.MakeId,NR.MakeGroupId,NR.EMail,NR.MobileNo,NR.LoginId,NR.Password,NR.IsActive,NR.UpdatedDate,NR.UpdatedBy,NR.IsExecutive,NR.CityId 
							FROM NCS_RManagers AS NR WHERE NR.Id= @strempId
						
					END  
					END 
    END
    
    

 

