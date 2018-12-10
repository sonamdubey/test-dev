IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CallScheduling]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CallScheduling]
GO

	-- ================================================================================================================================================
-- Author:  Manish  
-- Create date: 09-Jan-13 
-- Details: SP will schedule the call when user select one of the options:
--1. Fake Customer 2.Not Interested 3. Busy to talk  4. Not contacted 5. Shown Interest 
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE
-- Modified By: Manish Chourasiya on 10-04-2013, one more field NextFollowupDate added in the update of tc_calls 
-- Modified By: Manish Chourasiya on 23-04-2013, change in the logic of action on calls release date 02-05-2013 
-- Modified By: Tejashree Patil on 10 Jul 2013, Added parameter @ActionTakenOn instead of GETDATE() for import excel and commented GETDATE()
-- Modified By: Manish on 16-07-2013 for implementing the changes going to made in followup page (next action and feedback dropdown)
-- Modified By: Manish on 14-08-2013 for implementing the changes going to made in followup page (next action and feedback dropdown)
-- Modified By : Suresh Prajapati on 12th Aug, 2015
-- Description : To update Inquiry status
-- Modified By : Khushaboo Patil on 15th Sept Added condition for @Tc_LeadDispositionId = 41(archive)
-- Modified By Vivek Gupta on 23-09-2015, added try catch
-- Modified By Vivek Gupta on 09-01-2016, added @NextCallTo
-- Modified By : Suresh Prajapati on 27th Jan, 2016
-- Description : To Save and Log Inquiry Status in TC_InquiryLead for BW Inquiries
-- Modified By : Suresh Prajapati on 18th July, 2016
-- Description : 1. Passed BusinessTypeId for  SP 'TC_ScheduleCall'
--				 2. Updated LeadDisposition for TC_Service_Inquiries when Customer is fake
-- Modified By : Suresh Prajapati on 09th Aug, 2016
-- Description : Removed hardcoded value for @BusinessTypeId and fetched it from previous value in TC_ActiveCalls
-- Modified By :Ashwini Dhamankar on Sep 02,2016 (Updated TC_LeadDispositionID in TC_InquiriesLead table)
-- modified by : Khushaboo patil on 2 sept 2016 added parameter @NextActionDate
-- ================================================================================================================================================
CREATE PROCEDURE [dbo].[TC_CallScheduling] @TC_LeadId AS INT
	,@TC_UsersId AS INT
	,@TC_CallActionId AS TINYINT
	,@Comment AS VARCHAR(max)
	,@NextFolloupDate AS DATETIME
	,@TC_LeadDispositionId AS TINYINT
	,@TC_InqLeadOwnerId AS INT = NULL
	,----Parametre added by manish on 23-04-2013
	@ActionTakenOn AS DATETIME = NULL
	,-- Modified By: Tejashree Patil on 10 Jul 2013
	@TC_NextActionId AS SMALLINT = NULL -- Added By Manish on 16-07-2013 for capturing next action at call level
	,@InqStatusId SMALLINT = NULL
	,@NextCallTo SMALLINT = NULL -- Individual(user)= 2, dealer= 1
	,@BWLeadInqStatus TINYINT = NULL
	,@BusinessTypeId TINYINT = 3
	--,@NextActionDate DATETIME = NULL
AS
BEGIN
	DECLARE @TC_CallsId AS INT
	DECLARE @ScopeIdentity AS INT
	DECLARE @CallType AS TINYINT

	SET @ActionTakenOn = ISNULL(@ActionTakenOn, GETDATE()) -- Modified By: Tejashree Patil on 10 Jul 2013

	BEGIN TRY
		BEGIN TRANSACTION CallScheduling

			IF (ISNULL(@InqStatusId, 0) <> 0)
				BEGIN
					UPDATE TC_InquiriesLead
					SET TC_InquiryStatusId = ISNULL(@InqStatusId, TC_InquiryStatusId)
						,TC_LeadDispositionID = ISNULL(@TC_LeadDispositionId, TC_LeadDispositionID) -- Modified By :Ashwini Dhamankar on Sep 02,2016
						,ModifiedBy = @TC_UsersId
						,ModifiedDate = GETDATE()
					WHERE TC_LeadId = @TC_LeadId
						AND IsActive = 1
				END

			IF @Tc_LeadDispositionId = 1 OR @Tc_LeadDispositionId = 3 OR @Tc_LeadDispositionId = 41
				BEGIN
					----------Insert Record in TC_DispositionLog table
					EXEC [dbo].[TC_DispositionLogInsert] @TC_UsersId
						,@TC_LeadDispositionId
						,@TC_LeadId
						,1
						,@TC_LeadId

					SELECT @TC_CallsId = TC_CallsId
						,@BusinessTypeId = ISNULL(TC_BusinessTypeId, 3)
					FROM TC_ActiveCalls WITH (NOLOCK)
					WHERE TC_UsersId = @TC_InqLeadOwnerId
						AND TC_LeadId = @TC_LeadId

					------------Delete Lead from Active calls since customer is fake or not interested----------  
					EXEC TC_DisposeCall @TC_CallsId
						,@Comment
						,@TC_CallActionId
						,@NextFolloupDate
						,@TC_UsersId

					----------------Set all inquiry type to closed since customer is fake or not interested----------   
					UPDATE TC_InquiriesLead
					SET TC_LeadStageId = 3
						,TC_LeadDispositionID = @TC_LeadDispositionId
					WHERE TC_LeadId = @TC_LeadId
						AND IsActive = 1

					----------------Set  lead to  closed stage since customer is fake or not interested----------   
					UPDATE TC_Lead
					SET TC_LeadStageId = 3
						,LeadClosedDate = Getdate()
						,TC_LeadDispositionId = @TC_LeadDispositionId
					WHERE TC_LeadId = @TC_LeadId

					----------------if inquiry type is used car buyer than update disposition id since customer is fake or not interested----------   
					UPDATE TC_BuyerInquiries
					SET TC_LeadDispositionId = @TC_LeadDispositionId
					WHERE TC_InquiriesLeadId IN (
							SELECT TC_InquiriesLeadId
							FROM TC_InquiriesLead WITH (NOLOCK)
							WHERE TC_LeadId = @TC_LeadId
								AND TC_LeadInquiryTypeId = 1
							)

					----------------if inquiry type is used car seller than update disposition id since customer is fake or not interested----------   
					UPDATE TC_SellerInquiries
					SET TC_LeadDispositionID = @TC_LeadDispositionId
					WHERE TC_InquiriesLeadId IN (
							SELECT TC_InquiriesLeadId
							FROM TC_InquiriesLead WITH (NOLOCK)
							WHERE TC_LeadId = @TC_LeadId
								AND TC_LeadInquiryTypeId = 2
							)

					----------------if inquiry type is new car buyer than update disposition id since customer is fake or not interested----------   
					UPDATE TC_NewCarInquiries
					SET TC_LeadDispositionId = @TC_LeadDispositionId
					WHERE TC_InquiriesLeadId IN (
							SELECT TC_InquiriesLeadId
							FROM TC_InquiriesLead WITH (NOLOCK)
							WHERE TC_LeadId = @Tc_LeadId
								AND TC_LeadInquiryTypeId = 3
							)

					----------------Change Service Inquiry's TC_LeadDispositionId as customer is fake or not interested----------
					UPDATE TC_Service_Inquiries
					SET TC_LeadDispositionId = @TC_LeadDispositionId
					WHERE TC_InquiriesLeadId IN (
							SELECT TC_InquiriesLeadId
							FROM TC_InquiriesLead WITH (NOLOCK)
							WHERE TC_LeadId = @Tc_LeadId
								AND TC_LeadInquiryTypeId = 4 -- Service
							)

					UPDATE TC_Insurance_Inquiries
					SET TC_LeadDispositionId = @TC_LeadDispositionId
					WHERE TC_InquiriesLeadId IN (
							SELECT TC_InquiriesLeadId
							FROM TC_InquiriesLead WITH (NOLOCK)
							WHERE TC_LeadId = @Tc_LeadId
								AND TC_LeadInquiryTypeId = 6 -- Service
							)

					-------------Below if else block update the customer verification and fake status----------                                         
					IF @TC_LeadDispositionId = 1 ------------- Fake customer 
						BEGIN
							UPDATE TC_CustomerDetails
							SET IsVerified = 0
								,IsFake = 1
								,IsleadActive = 0
								,ActiveLeadId = NULL
							WHERE ActiveLeadId = @TC_LeadId
						END
					ELSE ------------------Not interested Customer or archive lead
						BEGIN
							UPDATE TC_CustomerDetails
							SET IsVerified = 1
								,IsFake = 0
								,IsleadActive = 0
								,ActiveLeadId = NULL
							WHERE ActiveLeadId = @TC_LeadId
						END
					------------------------------------------------------------------------------------------------------------------ 
				END
			ELSE -------------------if customer is not fake and shown iterest  
				BEGIN
					SELECT @TC_CallsId = TC_CallsId
					FROM TC_ActiveCalls WITH (NOLOCK)
					WHERE TC_UsersId = @TC_InqLeadOwnerId
						AND TC_LeadId = @TC_LeadId

					--------------------- below if block updates after the action 'busy to talk' and 'Not contacted' on call--------------     
					IF @TC_CallActionId = 1	OR @TC_CallActionId = 3
						BEGIN
							DECLARE @AlertId INT

							SELECT @CallType = CASE CallType WHEN 1 THEN 2 WHEN 3 THEN 4 ELSE CallType END
							,@AlertId = AlertId
							FROM TC_Calls WITH (NOLOCK)
							WHERE TC_CallsId = @TC_CallsId

							SET @NextFolloupDate = ISNULL(@NextFolloupDate, GETDATE())

							EXEC TC_DisposeCall @TC_CallsId
								,@Comment
								,@TC_CallActionId
								,@NextFolloupDate
								,@TC_UsersId

							EXEC TC_ScheduleCall @TC_InqLeadOwnerId
								,@TC_LeadId
								,@CallType
								,@NextFolloupDate
								,@AlertId
								,@Comment
								,@TC_NextActionId
								,@NextCallTo
								,@ScopeIdentity
								,NULL
								,@BusinessTypeId
						END

					--------------------------------------------------------------------------------------------------------    
					IF @TC_CallActionId = 2 ----------customer shown interest  
						BEGIN
							SELECT @CallType = CallType
							FROM TC_Calls WITH (NOLOCK)
							WHERE TC_CallsId = @TC_CallsId

							EXEC TC_DisposeCall @TC_CallsId
								,@Comment
								,@TC_CallActionId
								,@NextFolloupDate
								,@TC_UsersId

							IF (@CallType = 2 OR @CallType = 1)
								BEGIN
									--EXEC TC_CallUserScheduling @Tc_LeadId
									--	,@TC_InqLeadOwnerId
									--	,--@TC_UsersId, -----Line modified by Manish on 23-04-2013
									--	@NextFolloupDate
									--	,@TC_CallsId
									--	,@Comment
									--	,NULL
									--	,@TC_NextActionId -- Added By Nilesh on 19-08-2013 for capturing next action during diversion
									--	,NULL
									--	,@BusinessTypeId
									EXEC [TC_CallUserScheduling_V2.0] @Tc_LeadId,@TC_InqLeadOwnerId
									SET @CallType = 3
								END
							EXEC TC_ScheduleCall @TC_InqLeadOwnerId
								,@TC_LeadId
								,@CallType
								,@NextFolloupDate
								,@AlertId
								,@Comment
								,@TC_NextActionId
								,@NextCallTo
								,@ScopeIdentity
								,NULL
								,@BusinessTypeId
						
					
							--------- Added for logging BW Lead Inquiry Status ---------
							IF (ISNULL(@BWLeadInqStatus, 0) <> 0)
								BEGIN
									-- Update the existing lead with new inquiry status
									UPDATE TC_InquiriesLead
									SET TC_BWLeadStatusId = @BWLeadInqStatus
									WHERE TC_LeadId = @TC_LeadId
										AND TC_LeadInquiryTypeId = 3 -- for New car purchase only

									IF (@@RowCount > 0)
										BEGIN
											-- Log status change
											EXEC TC_BWLeadInqStatusChangeLog @TC_LeadId
												,@BWLeadInqStatus
												,@TC_UsersId
										END
								END
						END

					IF @TC_CallActionId IS NULL
						BEGIN
							SELECT @CallType = CallType
							FROM TC_Calls WITH (NOLOCK)
							WHERE TC_CallsId = @TC_CallsId

							EXEC TC_DisposeCall @TC_CallsId
								,@Comment
								,@TC_CallActionId
								,@NextFolloupDate
								,@TC_UsersId

							EXEC TC_ScheduleCall @TC_InqLeadOwnerId
								,@TC_LeadId
								,@CallType
								,@NextFolloupDate
								,@AlertId
								,@Comment
								,@TC_NextActionId
								,@NextCallTo
								,@ScopeIdentity
								,NULL
								,@BusinessTypeId
						END
					-----------------------------------------------------------------------------------------------------------    
				END

		COMMIT TRANSACTION CallScheduling
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION CallScheduling

		INSERT INTO TC_Exceptions (
			Programme_Name
			,TC_Exception
			,TC_Exception_Date
			,InputParameters
			)
		VALUES (
			'TC_CallScheduling'
			,(ERROR_MESSAGE() + 'ERROR_NUMBER(): ' + CONVERT(VARCHAR, ERROR_NUMBER()))
			,GETDATE()
			,' @TC_UsersId:' + ISNULL(CAST(@TC_UsersId AS VARCHAR(10)), 'NULL') + ' @TC_CallActionId :' + ISNULL(CAST(@TC_CallActionId AS VARCHAR(10)), 'NULL') + ' @Comment : ' + ISNULL(@Comment, 'NULL') + ' @NextFolloupDate : ' + ISNULL(CAST(@NextFolloupDate AS VARCHAR(50)), 'NULL') + ' @TC_LeadDispositionId: ' + ISNULL(CAST(@TC_LeadDispositionId AS VARCHAR(50)), 'NULL') + ' @TC_InqLeadOwnerId: ' + ISNULL(CAST(@TC_InqLeadOwnerId AS VARCHAR(20)), 'NULL') + ' @ActionTakenOn : ' + ISNULL(CAST(@ActionTakenOn AS VARCHAR(50)), 'NULL') + ' @TC_NextActionId: ' + ISNULL(CAST(@TC_NextActionId AS VARCHAR(50)), 'NULL') + ' @InqStatusId : ' + ISNULL(CAST(@InqStatusId AS VARCHAR(5)), 'NULL') + ' @TC_LeadId : ' + ISNULL(CAST(@TC_LeadId AS VARCHAR(10)), 'NULL') + ' @BusinessTypeId : ' + ISNULL(CAST(@BusinessTypeId AS VARCHAR(10)), 'NULL')
			)
	END CATCH;
END
