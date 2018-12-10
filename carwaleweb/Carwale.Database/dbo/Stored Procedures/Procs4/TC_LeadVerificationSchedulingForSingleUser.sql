IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadVerificationSchedulingForSingleUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadVerificationSchedulingForSingleUser]
GO

	
-- =============================================   
-- Author:  SUrendra   
-- Create date: 11-02-2013
-- Description: Scheduling unverified Leads to single User dealership
-- Modified : By Surendra ,ON 19 Feb 2013 put Try Catch
-- Modified by Manish on 01-08-2013 adding WITH (NOLOCK) Keyword in the queries
-- Modified by Manish on 18-04-2014  Use join statement in place of IN keyword in update queries for optimization purpose.
-- Modified by Chetan on 28-06-2016 for sync task_lists table 
-- =============================================   
CREATE PROCEDURE [dbo].[TC_LeadVerificationSchedulingForSingleUser]
@TC_Usersid AS INT,
@DealerId   AS INT 
AS 
BEGIN 

DECLARE @LeadsWithNostage AS TABLE 
(
RowID INT IDENTITY(1, 1), TC_LeadId BIGINT NOT NULL , CreatedDate DATETIME
)

DECLARE @NumberRecords AS INT
DECLARE @RowCount AS INT
DECLARE @TC_LeadId BIGINT
DECLARE @CreatedDate DATETIME

--Get all the data where lead is not at all scheduled
INSERT INTO @LeadsWithNostage(TC_LeadId,CreatedDate) SELECT TC_LeadId,ISNULL(LeadCreationDate,GETDATE()) FROM TC_Lead WITH (NOLOCK)  WHERE  BranchId=@DealerId AND TC_LeadStageId IS NULL AND TC_LeadDispositionId IS NULL

SET @NumberRecords = @@ROWCOUNT
SET @RowCount = 1

IF(@NumberRecords>0)
	BEGIN
		BEGIN TRY
			WHILE @RowCount <= @NumberRecords
				BEGIN
					SELECT @TC_LeadId = TC_LeadId, @CreatedDate = CreatedDate
					FROM @LeadsWithNostage
					WHERE RowID = @RowCount
				
					UPDATE TCL 
					SET TCL.LeadVerifiedBy = @TC_Usersid,TCL.TC_LeadStageId=1
					FROM TC_LEAD AS TCL WITH(NOLOCK)
					WHERE BranchId=@DealerId AND TCL.TC_LeadId = @TC_LeadId

					UPDATE TCIL
					SET TCIL.TC_LeadStageId = 1,TCIL.TC_UserId=@TC_Usersid
					FROM TC_InquiriesLead AS TCIL WITH(NOLOCK)
					WHERE  BranchId=@DealerId AND TCIL.TC_LeadId = @TC_LeadId
					
					EXEC TC_ScheduleCall @TC_UsersId,@TC_LeadId,1,@CreatedDate,NULL,NULL,NULL,NULL,NULL
					
					SET @RowCount = @RowCount + 1;
					 -----------------------------------------------------------------------------------------------------------------------
						
						--INSERT INTO TC_Calls(TC_LeadId,CallType,TC_UsersId,ScheduledOn,IsActionTaken,TC_CallActionId,ActionTakenOn,ActionComments,AlertId) 
						--			SELECT TC_LeadId, 1,@TC_UsersId,CreatedDate,0,NULL,NULL,NULL,NULL FROM @LeadsWithNostage

			                
						--	DECLARE @Tc_Calls_Id int =scope_identity();

						--/*INSERT INTO TC_ActiveCalls (TC_CallsId, TC_LeadId,CallType,TC_UsersId,ScheduledOn, AlertId,LastCallDate,LastCallComment) 
						--			SELECT TC_CallsId,TC_LeadId,1,TC_UsersId,ScheduledOn,NULL,NULL,NULL 
						--			FROM  TC_Calls WITH (NOLOCK) 
						--			WHERE TC_LeadId IN(SELECT TC_LeadId FROM @LeadsWithNostage)	AND CallType =1 AND IsActionTaken=0*/
			            
						--------Above insert statement commented by Manish on 18-04-2014 and replaced by below insert statements-------

						--INSERT INTO TC_ActiveCalls (TC_CallsId, TC_LeadId,CallType,TC_UsersId,ScheduledOn, AlertId,LastCallDate,LastCallComment) 
						--			SELECT TCC.TC_CallsId,TCC.TC_LeadId,1,TCC.TC_UsersId,TCC.ScheduledOn,NULL,NULL,NULL 
						--			FROM  TC_Calls AS TCC WITH (NOLOCK) 
						--			JOIN  @LeadsWithNostage AS LS ON LS.TC_LeadId=TCC.TC_LeadId
					 --          									 AND TCC.CallType =1 
						--										 AND TCC.IsActionTaken=0

			            
						--	  EXEC  [dbo].[TC_TaskListUpdate] @ActionId=2,@CallId=@Tc_Calls_Id,@ApplicationId= 1;
					 -- ------------------------------------------------------------------------------------------------------------------------
					
				END
		END TRY
		BEGIN CATCH		
			INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date,Inputparameters)
			VALUES('TC_LeadVerificationSchedulingForSingleUser',(ERROR_MESSAGE()+', ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),GETDATE(),NULL)
			--SELECT ERROR_NUMBER() AS ErrorNumber;
		END CATCH; 
				
	END			 
END 
