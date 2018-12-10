IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadVerificationSchedulingForMultiUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadVerificationSchedulingForMultiUser]
GO

	-- =============================================   
-- Author:  Manish   
-- Create date: 14-Jan-12   
-- Modified By: Surendar on 13 march for changing roles table
-- Description: Scheduling unverified Leads to User.
-- Modified By Vivek Gupta on 12-03-2015, changes done to schedule all distinct leadtype leads to single role users
-- =============================================   
CREATE PROCEDURE       [dbo].[TC_LeadVerificationSchedulingForMultiUser] @TC_Usersid AS INT, 
                                                      @DealerId   AS INT 
AS 
  BEGIN 
	  IF EXISTS(SELECT TOP 1 Id FROM TC_Users WITH(NOLOCK) WHERE Id=@TC_Usersid AND IsCarwaleUser=0)
		BEGIN
		  DECLARE @TotalVerfCall AS INT=0 
		  DECLARE @TotalCall AS INT=0 
		  DECLARE @UserLeadType AS INT=0 
		  DECLARE @TC_LeadId AS INT 
		  DECLARE @LeadType AS SMALLINT 
          DECLARE @CheckUserModelPermissionFlag BIT=0
		  DECLARE @LeadsWithNostage AS TABLE (ID SMALLINT IDENTITY(1,1),
											  TC_LeadId INT NOT NULL,
											  LeadType TINYINT NOT NULL)
		  DECLARE @ScopeIdentity AS INT 
		  DECLARE @LoopCount AS INT=1

		  DECLARE @RoleId INT
		  DECLARE @ScheduleDate DATETIME =  GETDATE()

		  SELECT @TotalVerfCall = Isnull(Count(TC_CallsId), 0) 
		  FROM   TC_ActiveCalls WITH(NOLOCK) 
		  WHERE  TC_UsersId = @TC_UsersId 
				 AND CallType = 1 

          SET     @RoleId =	  (SELECT Top 1 R.RoleId
							  FROM		TC_Users  U WITH(NOLOCK)
										INNER JOIN TC_UsersRole R WITH(NOLOCK) ON R.UserId=U.Id
							  WHERE		U.BranchId=@DealerId 
										AND U.IsActive=1 
										AND U.IsCarwaleUser=0
										AND U.Id = @TC_Usersid
										AND R.RoleId IN (4,5,6))-- top 1 role selected, the case user has both usedEx and newEx role and the other user exists of either roles has no been handled, 
										-- this query will work perfectly for user having only single role
          
		   IF NOT EXISTS (SELECT Id 
		             FROM TC_Users  U WITH(NOLOCK)
					 INNER JOIN TC_UsersRole R WITH(NOLOCK) ON R.UserId=U.Id
					 WHERE  U.BranchId=@DealerId 
					 AND	U.IsActive=1 
					 AND    U.IsCarwaleUser=0
					 AND    R.RoleId = @RoleId
					 AND    U.Id <> @TC_Usersid)
           BEGIN
			 SET @TotalCall = ( SELECT COUNT(DISTINCT TCL.TC_LeadId)
								FROM TC_Lead  AS TCL WITH(NOLOCK) 								
								WHERE
									TCL.BranchId = @DealerId 
									AND TCL.LeadVerifiedBy is null
									AND TCL.TC_LeadStageId is null
									AND  (
											(@RoleId = 4 AND TCL.LeadType = 3)
										 OR (@RoleId = 5 AND TCL.LeadType = 1)
										 OR (@RoleId = 6 AND TCL.LeadType = 2)
										 )
								)
		   END
		   --selecT * from  TC_LeadInquiryType
		   --selecT * from TC_RolesMaster

		   ELSE
		   BEGIN
				SET @TotalCall=3 - @TotalVerfCall  ----Total  calls need to allocate to the user
		   END

		  IF @TotalCall > 0 
			BEGIN 
	----------------Insert all unassigned lead corresponding to dealer and user tasks maximum up to 3 --------------
		/*  15-03-2013		INSERT INTO @LeadsWithNostage
												  (TC_LeadId,
												   LeadType)
				SELECT DISTINCT TOP (@TotalCall)   TCL.TC_LeadId, 
												   TCL.LeadType 
				FROM TC_Lead         AS TCL 
				JOIN TC_Tasks        AS TCT 
											   ON TCL.leadtype = TCT.tc_inquirytypeid 
				JOIN TC_Roletasks    AS TCRT 
											   ON TCRT.taskid = TCT.id 
				JOIN TC_Users        AS TCU 
											   ON TCU.roleid = TCRT.roleid 
			   WHERE TCU.id = @TC_UsersId AND
				   TCU.BranchId = @DealerId 
				   AND TCL.LeadVerifiedBy is null
				   AND TCL.TC_LeadStageId is null
				   AND TCL.BranchId=@DealerId
				ORDER BY TC_LeadId desc      15-03-2013 */
				
				INSERT INTO @LeadsWithNostage
												  (TC_LeadId,
												   LeadType)
				SELECT DISTINCT TOP (@TotalCall)   TCL.TC_LeadId, 
												   TCL.LeadType 
				FROM TC_Lead         AS TCL  WITH(NOLOCK)
				JOIN TC_RolesMaster        AS TCT  WITH(NOLOCK)
											   ON TCL.leadtype = TCT.tc_inquirytypeid 
				JOIN TC_UsersRole    AS TCRT  WITH(NOLOCK)
											   ON TCRT.RoleId = TCT.TC_RolesMasterId
               JOIN TC_Users        AS TCU  WITH(NOLOCK)
											   ON TCU.Id = TCRT.UserId 											   
				WHERE TCU.Id = @TC_UsersId AND
				   TCU.BranchId = @DealerId 
				   AND TCL.LeadVerifiedBy is null
				   AND TCL.TC_LeadStageId is null
				   AND TCL.BranchId=@DealerId
				ORDER BY TC_LeadId desc 
	            
				SET @TotalCall=@@ROWCOUNT 
	            
	----------inserting  all records into  TC_Call and TC_ActiveCalls table ----------------------           
				WHILE @LoopCount<= @TotalCall 
				  BEGIN 
	  
	                  
					SELECT @TC_LeadId=TC_LeadId,
						   @LeadType=LeadType   
					FROM   @LeadsWithNostage 
					WHERE Id=@LoopCount
					
					
					
				IF @LeadType=3
				 BEGIN
	                IF  (SELECT COUNT(U.Id)
	                 FROM TC_Users AS U WITH(NOLOCK)
	                 JOIN TC_UserModelsPermission AS TCUP WITH(NOLOCK) ON TCUP.TC_UsersId=U.Id
	                                                          AND U.BranchId=@DealerId)<>0
	                     BEGIN 
	                     SET @CheckUserModelPermissionFlag=1
	                     END                                      
				
				 END
					
					IF ((@LeadType=1 or @LeadType=2) OR (@LeadType=3 AND @CheckUserModelPermissionFlag=0))
	                BEGIN
	   -------------------Update Lead stage to verification stage--------------- 
					  UPDATE TC_Lead 
					  SET    LeadVerifiedBy = @TC_Usersid,
							 TC_LeadStageId=1
					  WHERE  TC_LeadId = @TC_LeadId 
	                  
	-------------------Update Lead stage to verification stage---------------                   
					   UPDATE TC_InquiriesLead
					   SET    TC_LeadStageId = 1,
							  TC_UserId=@TC_Usersid
					   WHERE  TC_LeadId = @TC_LeadId 

					   EXEC TC_ScheduleCall @TC_UsersId,@TC_LeadId,1,@ScheduleDate,null,null,null,null,@ScopeIdentity
					END 								  


			  	ELSE IF (@LeadType=3 AND @CheckUserModelPermissionFlag=1)
	                  BEGIN
	                DECLARE  @TC_InquiriesLeadId INT
	                SELECT  @TC_InquiriesLeadId=TC_InquiriesLeadId
	                FROM TC_InquiriesLead  WITH(NOLOCK)
	                WHERE  TC_LeadId=@TC_LeadId
	                   AND  TC_LeadInquiryTypeId=3
	                
			   SELECT TOP 1 @TC_Usersid=U.UserId  FROM TC_UsersRole U  WITH(NOLOCK)
			    JOIN TC_RolesMaster R WITH(NOLOCK) ON U.RoleId=R.TC_RolesMasterId
			    JOIN TC_UserModelsPermission AS UMP WITH(NOLOCK) ON U.UserId=UMP.TC_UsersId
			    JOIN CarVersions AS CV WITH(NOLOCK) ON UMP.ModelId=CV.CarModelId
			    JOIN TC_NewCarInquiries AS NCI WITH(NOLOCK) ON CV.ID=NCI.VersionId 
										   AND TC_InquiriesLeadId=@TC_InquiriesLeadId
			   WHERE R.TC_InquiryTypeId=@LeadType
		    		 AND U.UserId=@TC_Usersid
	                
	   -------------------Update Lead stage to verification stage--------------- 
					  UPDATE TC_Lead 
					  SET    LeadVerifiedBy = @TC_Usersid,
							 TC_LeadStageId=1
					  WHERE  TC_LeadId = @TC_LeadId 
	                  
	-------------------Update Lead stage to verification stage---------------                   
					   UPDATE TC_InquiriesLead
					   SET    TC_LeadStageId = 1,
							  TC_UserId=@TC_Usersid
					   WHERE  TC_LeadId = @TC_LeadId 



					  EXEC TC_ScheduleCall @TC_UsersId,@TC_LeadId,1,@ScheduleDate,null,null,null,null,@ScopeIdentity
					END

					 SET  @LoopCount=@LoopCount+1
	                 
				  END -----end of while loop 
			END 
		END
  END
