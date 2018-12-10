IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ProcessLeadScheduling_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ProcessLeadScheduling_V2]
GO

	

-- Created By:	Deepak Tripathi
-- Create date: 12th July 2016
-- Description:	Assign lead to one consultant
-- DECLARE @NewCallId int exec [TC_ProcessLeadScheduling_V2.0] 5, 26242,1,254,4,1,'2016-07-18 16:26:25.957','asd', 4,'2016-07-18 16:26:25.957',@NewCallId SELECT @NewCallId '@NewCallId'
-- Modified By: Ashwini Dhamankar on Aug 29,2016 (Fetched roleid = 1 for insurance lead)
-- =============================================
CREATE PROCEDURE [dbo].[TC_ProcessLeadScheduling_V2.0] 
	@BranchId INT
	,@LeadId INT
	,@IsExistingLead BIT
	,@LeadOwnerID INT
	,@InquiryType TINYINT
	,@CallType TINYINT
	,@ScheduledOn DATETIME
	,@FollowupComments VARCHAR(500)
	,@NextActionId SMALLINT
	,@NextActionDate DATETIME
	,@NewCallId INT OUTPUT
	--,@NewLeadOwnerID INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @IsUserExist BIT = 0
	DECLARE @IsScheduleCall BIT = 0
	DECLARE @TC_BusinessTypeId TINYINT
	DECLARE @RoleIds VARCHAR(20)

	--Decide the leadtype
	SET @TC_BusinessTypeId = dbo.TC_FNGetBusinessType(@InquiryType)
	SET @RoleIds = CASE @TC_BusinessTypeId 
	WHEN 4 THEN '16' 
	WHEN 6 THEN '1'     --Insurance 
	ELSE '4,5,6' END  --Sales executive roles/ Service Executive 


	IF @IsExistingLead = 1
		BEGIN
			--Existing Lead
			--Check if call is already scheduled or not
			SELECT TOP 1 TC_CallsId FROM TC_ActiveCalls WITH (NOLOCK) WHERE TC_LeadId = @LeadId AND TC_BusinessTypeId = @TC_BusinessTypeId

			IF @@ROWCOUNT = 0 --Lead might be in pool
				SET @IsScheduleCall = 1
			ELSE
				SET @IsScheduleCall = 0
		END
	ELSE
		BEGIN
			SET @IsScheduleCall = 1
		END

	--Schedule Call
	IF @IsScheduleCall = 1
		BEGIN
			--New lead
			IF ISNULL(@LeadOwnerID, 0) > 0
				BEGIN
					-- Lead Owner is already there so schedule the call
					SET @IsUserExist = 1
				END
			ELSE
				BEGIN
					--Lets decide the lead owner
					--Check if dealer is multiuser/Single User
					SELECT @LeadOwnerID = TU.Id FROM TC_Users TU WITH (NOLOCK) WHERE BranchId = @BranchId AND ISNULL(IsCarwaleUser, 0) <> 1
					IF @@ROWCOUNT = 1 -- Single User/Assign lead to same user
						BEGIN
							SET @IsUserExist = 1
						END
					ELSE --Multi user --follow round robin process
						BEGIN
							SELECT TOP 1 @LeadOwnerID = TU.Id
							FROM TC_Users TU WITH (NOLOCK)
								INNER JOIN TC_UsersRole TUR WITH (NOLOCK) ON TU.Id = TUR.UserId
								AND TUR.RoleId IN(SELECT ListMember FROM fnSplitCSV(@RoleIds))
							WHERE BranchId = @BranchId
								AND ISNULL(IsCarwaleUser, 0) <> 1
							ORDER BY TU.TodaysCallCount

							IF @@ROWCOUNT = 1 -- There is a sales executive
								BEGIN
									SET @IsUserExist = 1
								END
							ELSE --No Sales executive/assign lead to DP
								BEGIN
									SELECT TOP 1 @LeadOwnerID = TU.Id
									FROM TC_Users TU WITH (NOLOCK)
									INNER JOIN TC_UsersRole TUR WITH (NOLOCK) ON TU.Id = TUR.UserId
										AND TUR.RoleId IN (1,9) --DP, Super Admin
									WHERE BranchId = @BranchId
										AND ISNULL(IsCarwaleUser, 0) <> 1
									ORDER BY TUR.RoleId DESC

									IF @@ROWCOUNT = 1 -- There is a DP/Super admin
									BEGIN
										SET @IsUserExist = 1
									END
								END
						END
				END

				IF @IsUserExist = 1
					BEGIN
							
						--Increase the call count	
						UPDATE TC_Users
						SET TodaysCallCount = TodaysCallCount + 1
						WHERE Id = @LeadOwnerID

						--Assign owner to lead
						UPDATE TC_Lead
						SET LeadVerifiedBy = @LeadOwnerID
							,TC_LeadStageId = 1
						WHERE TC_LeadId = @LeadId

						--Update Owner details for InquiryLead
						IF @TC_BusinessTypeId = 3
							UPDATE TC_InquiriesLead SET TC_UserId = @LeadOwnerID,TC_LeadStageId = ISNULL(TC_LeadStageId, 1)
							WHERE TC_LeadId = @LeadId AND TC_LeadInquiryTypeId IN (1,2,3)
						ELSE
							UPDATE TC_InquiriesLead SET TC_UserId = @LeadOwnerID,TC_LeadStageId = ISNULL(TC_LeadStageId, 1)
							WHERE TC_LeadId = @LeadId AND TC_LeadInquiryTypeId = @InquiryType
					
						--Schedule Call
						EXEC TC_ScheduleCall @TC_UsersId = @LeadOwnerID
							,@TC_LeadId = @LeadId
							,@CallType = @CallType
							,@ScheduleDate = @ScheduledOn
							,@AlertId = NULL
							,@LastCallComment = @FollowupComments
							,@TC_NextActionId = @NextActionId
							,@NextCallTo = NULL
							,@NewCallId = @NewCallId OUTPUT-- added OUTPUT by kritika choudhary on 3rd oct 2016
							,@TC_NextActionDate = @NextActionDate
							,@TC_BusinessTypeId = @TC_BusinessTypeId
						
						
				END
		END
END
