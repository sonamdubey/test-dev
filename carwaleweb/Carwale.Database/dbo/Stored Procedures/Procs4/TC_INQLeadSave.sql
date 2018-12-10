IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQLeadSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQLeadSave]
GO

	
-- ===========================================================================================================================================================
-- Created By:	Surendra
-- Create date: 4th Jan 2013
-- Description:	Adding Inquiry Lead
-- Modified By: Surendar on 13 march for changing roles table
-- Modified By: Vivek Gupta on 17th May,2013 Added A parameter @LeadOwnId
-- Modified By: Tejashree Patil on 31 July 2013, Inserted InqSourceId in TC_InquiriesLead table.
-- Modified By: Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
-- Modified By: Umesh Ojha on 13 Feb 2014 adding restriction for duplicate entry in TC_Lead Table
-- Modified By: Manish Chourasiya on 15-04-2014 added with (nolock) keyword wherever not found.
-- Modified By: ViCKY Gupta ON 03-08-2015 added a parameter LatestVersionId and insert LatestVersionId into TC_InquiriesLead
-- Modified By : Afrose on 14-09-2015, added parameter @CampaignId
-- Modified By : Ashwini Dhamankar on Jan 15,2016 (Addded @NextCallTo to set NextCallTo = 2 of active calls for deals request)
-- Modified By : Ashwini Dhamankar on Feb 2,2016 (Inserted userid in TC_InquiriesLead in case of inquirysource 134 and 140)
-- Modified By : Ashwini Dhamankar on May 17,2016 (Added 147 and 148 sourceId's)
-- Modified By : Ashwini Dhamankar on June 2,2016 (insert LeadStageId = 1 and leadverifiedby = leadownerid in TC_Lead and TC_InquiriesLead for deals inquiries)
-- Modified By : Suresh Prajapati on 26th Jul, 2016
-- Description : Passed @BusinessTypeId = 5 for SP 'TC_ScheduleCall' in Advantage Leads case
-- Description : Passed @BusinessTypeId for sales cases - By Deepak on 26th Sept 2016
-- ==============================================================================================================================================================
CREATE PROCEDURE [dbo].[TC_INQLeadSave] (
	@AutoVerified BIT
	,-- If Inquiry will added from Trading query then value=1
	@BranchId BIGINT
	,@CustomerId BIGINT
	,@LeadOwnerId BIGINT
	,@Eagerness SMALLINT
	,@CreatedBy BIGINT
	,@InquirySource SMALLINT
	,@LeadId BIGINT
	,@INQDate DATETIME
	,@LeadInqTypeId SMALLINT
	,@CarDetails VARCHAR(MAX)
	,@LeadStage SMALLINT
	,@LeadIdOutput BIGINT OUTPUT
	,@INQLeadIdOutput BIGINT OUTPUT
	,@NextFollowupDate DATETIME
	,@FollowupComments VARCHAR(MAX)
	,@ReturnStatus TINYINT OUTPUT
	,-- will tell the status of what happend with inquiry
	@LeadDivertedTo VARCHAR(100) OUTPUT
	,@LeadOwnId BIGINT = NULL OUTPUT
	,@TC_CampaignSchedulingId INT = NULL
	,-- Modified By: Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
	@LatestVersionId INT = NULL
	,@CampaignId INT = NULL
	,@NextCallTo INT = NULL
	)
AS
BEGIN
	SET NOCOUNT ON;
	-- IF Lead is not created then need to create
	SET @ReturnStatus = 1 --default success status

	DECLARE @TC_BusinessTypeId TINYINT

	SET @TC_BusinessTypeId = dbo.TC_FNGetBusinessType(@LeadInqTypeId)

	DECLARE @CallsId INT
	DECLARE @ExistingCallType TINYINT = NULL
	DECLARE @ExistingLeadOwner BIGINT
	DECLARE @TC_INQLeadIdInner BIGINT

	SET @LeadOwnId = @LeadOwnerId

	IF (@FollowupComments IS NOT NULL)
	BEGIN
		SET @FollowupComments = @FollowupComments
	END
	ELSE
	BEGIN
		SET @FollowupComments = 'Inquiry Added ' + ISNULL(@FollowupComments, '')
	END

	IF (@NextFollowupDate IS NULL)
	BEGIN
		SET @NextFollowupDate = @INQDate
	END

	IF (@LeadId IS NULL) -- IF NO active lead is created then need to create
	BEGIN
		IF (@AutoVerified = 1) -- Inquiries is directly added from trading cars
		BEGIN
			SELECT @LeadIdOutput = TC_LeadId
			FROM TC_Lead WITH (NOLOCK)
			WHERE BranchId = @BranchId
				AND TC_CustomerId = @CustomerId
				AND ISNULL(TC_LeadStageId, 0) <> 3

			IF (@LeadIdOutput IS NULL)
			BEGIN
				INSERT INTO TC_Lead (
					BranchId
					,TC_CustomerId
					,TC_InquirySourceId
					,TC_LeadStageId
					,LeadCreationDate
					,LeadType
					,LeadVerifiedBy
					,LeadVerificationDate
					,TC_LeadDispositionId
					,TC_CampaignSchedulingId
					,TC_BusinessTypeId
					) -- Modified By: Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
				VALUES (
					@BranchId
					,@CustomerId
					,@InquirySource
					,2
					,@INQDate
					,@LeadInqTypeId
					,@LeadOwnerId
					,@INQDate
					,2
					,@TC_CampaignSchedulingId
					,@TC_BusinessTypeId
					)

				SET @LeadIdOutput = SCOPE_IDENTITY();
			END

			UPDATE TC_CustomerDetails
			SET ActiveLeadId = @LeadIdOutput
				,IsVerified = 1
				,IsleadActive = 1
			WHERE Id = @CustomerId

			INSERT INTO TC_InquiriesLead (
				BranchId
				,TC_CustomerId
				,TC_UserId
				,TC_InquiryStatusId
				,CreatedBy
				,CreatedDate
				,TC_LeadId
				,TC_LeadInquiryTypeId
				,TC_LeadStageId
				,CarDetails
				,LatestInquiryDate
				,InqSourceId
				,-- Modified By: Tejashree Patil on 31 July 2013
				TC_CampaignSchedulingId
				,LatestVersionId
				,CampaignId
				) -- Modified By: Tejashree Patil on 11 Oct 2013, Added parameter @TC_CampaignSchedulingId while adding inquiry.
			VALUES (
				@BranchId
				,@CustomerId
				,@LeadOwnerId
				,@Eagerness
				,@CreatedBy
				,@INQDate
				,@LeadIdOutput
				,@LeadInqTypeId
				,2
				,@CarDetails
				,@INQDate
				,@InquirySource
				,@TC_CampaignSchedulingId
				,@LatestVersionId
				,@CampaignId
				) --Added by Afrose, param @CampaignId

			SET @INQLeadIdOutput = SCOPE_IDENTITY();

			INSERT INTO TC_Calls (
				TC_LeadId
				,CallType
				,TC_UsersId
				,ScheduledOn
				,IsActionTaken
				,TC_CallActionId
				,ActionTakenOn
				,ActionComments
				,CreatedOn
				,TC_BusinessTypeId
				)
			VALUES (
				@LeadIdOutput
				,3
				,@LeadOwnerId
				,@INQDate
				,1
				,2
				,@INQDate
				,@FollowupComments
				,@INQDate
				,@TC_BusinessTypeId
				)

			EXEC TC_ScheduleCall @LeadOwnerId
				,@LeadIdOutput
				,3
				,@NextFollowupDate
				,NULL
				,NULL
				,NULL
				,NULL
				,@CallsId

			SET @ReturnStatus = 1 -- success	
		END
		ELSE -- Inquiruy came from other then trading cars and Lead is null
		BEGIN -- Inuiry is pushed from dealer website or CarWale site
			SELECT @LeadIdOutput = TC_LeadId
			FROM TC_Lead WITH (NOLOCK)
			WHERE BranchId = @BranchId
				AND TC_CustomerId = @CustomerId
				AND ISNULL(TC_LeadStageId, 0) <> 3

			DECLARE @IsDealsInquiry BIT = 0

			SET @IsDealsInquiry = CASE 
					WHEN @InquirySource IN (
							134
							,140
							,146
							,147
							,148
							)
						THEN 1
					ELSE 0
					END

			IF (@LeadIdOutput IS NULL)
			BEGIN
				INSERT INTO TC_Lead (
					BranchId
					,TC_CustomerId
					,TC_InquirySourceId
					,TC_LeadStageId
					,LeadCreationDate
					,LeadType
					,LeadVerifiedBy
					,TC_BusinessTypeId
					)
				VALUES (
					@BranchId
					,@CustomerId
					,@InquirySource
					,CASE @IsDealsInquiry
						WHEN 1
							THEN 1
						ELSE NULL
						END --modified by : Ashwini Dhamankar on June 2,2016
					,@INQDate
					,@LeadInqTypeId
					,CASE @IsDealsInquiry
						WHEN 1
							THEN @LeadOwnerId
						ELSE NULL
						END --modified by : Ashwini Dhamankar on June 2,2016
					,@TC_BusinessTypeId
					)

				SET @LeadIdOutput = SCOPE_IDENTITY();
			END

			UPDATE TC_CustomerDetails
			SET ActiveLeadId = @LeadIdOutput
				,IsleadActive = 1
			WHERE Id = @CustomerId

			INSERT INTO TC_InquiriesLead (
				BranchId
				,TC_CustomerId
				,TC_InquiryStatusId
				,CreatedBy
				,CreatedDate
				,TC_LeadId
				,TC_LeadInquiryTypeId
				,TC_LeadStageId
				,CarDetails
				,LatestInquiryDate
				,InqSourceId
				,LatestVersionId
				,TC_UserId
				) -- Modified By: Tejashree Patil on 31 July 2013
			VALUES (
				@BranchId
				,@CustomerId
				,@Eagerness
				,@CreatedBy
				,@INQDate
				,@LeadIdOutput
				,@LeadInqTypeId
				,CASE @IsDealsInquiry
					WHEN 1
						THEN 1
					ELSE @LeadStage
					END --modified by : Ashwini Dhamankar on June 2,2016
				,@CarDetails
				,@INQDate
				,@InquirySource
				,@LatestVersionId
				,CASE @IsDealsInquiry
					WHEN 1
						THEN @LeadOwnerId
					ELSE NULL
					END ----Modified By : Ashwini Dhamankar on May 17,2016 (Added 147 and 148 sourceId's)
				)

			SET @INQLeadIdOutput = SCOPE_IDENTITY()

			-- need to create calls for inquiries comes from the sources 134 or 140
			IF (@IsDealsInquiry = 1)
			BEGIN
				DECLARE @SchedulDate DATETIME = GETDATE()

				EXEC TC_ScheduleCall @LeadOwnerId
					,@LeadIdOutput
					,1
					,@SchedulDate
					,NULL
					,NULL
					,NULL
					,@NextCallTo
					,@CallsId
					,NULL
					,5
			END

			SET @ReturnStatus = 1 -- success	
		END
	END
	ELSE
	BEGIN
		--If advantage lead then cheage existing new car to advantage
		EXEC TC_UpdateLeadBusinessType @LeadId
			,@TC_BusinessTypeId

		-- Updating lead type based on dealer preferences
		DECLARE @ExistingLeadType TINYINT
		DECLARE @NewPreference TINYINT
		DECLARE @ExitingPreference TINYINT
		DECLARE @ExistingLeadStage TINYINT = NULL
		DECLARE @ExistingInqLeadStage TINYINT = NULL

		SELECT @ExistingLeadOwner = L.LeadVerifiedBy
			,@ExistingLeadStage = L.TC_LeadStageId
		FROM TC_Lead L WITH (NOLOCK)
		WHERE L.TC_LeadId = @LeadId

		IF (@ExistingLeadStage IS NULL)
		BEGIN
			SELECT @NewPreference = PrefOrder
			FROM TC_DealerPreferences WITH (NOLOCK)
			WHERE BranchId = @BranchId
				AND TC_LeadInquiryType = @LeadInqTypeId

			IF (@NewPreference IS NULL)
			BEGIN
				SET @NewPreference = 3
			END

			SELECT @ExistingLeadType = LeadType
				,@ExitingPreference = P.PrefOrder
			FROM TC_Lead L WITH (NOLOCK)
			INNER JOIN TC_DealerPreferences P WITH (NOLOCK) ON L.BranchId = P.BranchId
				AND L.LeadType = P.TC_LeadInquiryType
			WHERE TC_LeadId = @LeadId

			IF (@NewPreference < @ExitingPreference)
			BEGIN
				UPDATE TC_Lead
				SET LeadType = @LeadInqTypeId
				WHERE TC_LeadId = @LeadId
			END
		END

		SET @LeadIdOutput = @LeadId -- assiging existing lead id to leadid output	

		-- Check whether inquiries lead is created for same inquiry type
		SELECT @TC_INQLeadIdInner = TC_InquiriesLeadId
			,@ExistingInqLeadStage = TC_LeadStageId
		FROM TC_InquiriesLead WITH (NOLOCK)
		WHERE TC_LeadId = @LeadIdOutput
			AND TC_LeadInquiryTypeId = @LeadInqTypeId

		IF (@TC_INQLeadIdInner IS NULL) -- adding inqlead is not exists
		BEGIN
			INSERT INTO TC_InquiriesLead (
				BranchId
				,TC_CustomerId
				,TC_InquiryStatusId
				,CreatedBy
				,CreatedDate
				,TC_LeadId
				,TC_LeadInquiryTypeId
				,TC_LeadStageId
				,CarDetails
				,LatestInquiryDate
				,InqSourceId
				,LatestVersionId
				) -- Modified By: Tejashree Patil on 31 July 2013
			VALUES (
				@BranchId
				,@CustomerId
				,@Eagerness
				,@CreatedBy
				,@INQDate
				,@LeadIdOutput
				,@LeadInqTypeId
				,1
				,@CarDetails
				,@INQDate
				,@InquirySource
				,@LatestVersionId
				)

			SET @INQLeadIdOutput = SCOPE_IDENTITY();
		END
		ELSE -- updating inqlead since already exists
		BEGIN
			SET @INQLeadIdOutput = @TC_INQLeadIdInner

			UPDATE TC_InquiriesLead
			SET CarDetails = @CarDetails
				,LatestInquiryDate = @INQDate
				,TC_InquiryStatusId = ISNULL(@Eagerness, TC_InquiryStatusId)
				,ModifiedBy = @CreatedBy
				,ModifiedDate = @INQDate
				,LatestVersionId = @LatestVersionId
			WHERE TC_InquiriesLeadId = @INQLeadIdOutput

			--Modified By : Tejashree Patil on 8 Feb 2013 at 4pm
			IF (@ExistingInqLeadStage = 3)
			BEGIN
				UPDATE TC_InquiriesLead
				SET TC_LeadStageId = 2
					,TC_LeadDispositionID = NULL
				WHERE TC_InquiriesLeadId = @INQLeadIdOutput
			END
		END

		IF (@AutoVerified = 1) -- Inqury is added from trading car
		BEGIN
			IF (@ExistingLeadStage IS NULL)
			BEGIN
				INSERT INTO TC_Calls (
					TC_LeadId
					,CallType
					,TC_UsersId
					,ScheduledOn
					,IsActionTaken
					,TC_CallActionId
					,ActionTakenOn
					,ActionComments
					,CreatedOn
					,TC_BusinessTypeId
					)
				VALUES (
					@LeadIdOutput
					,1
					,@LeadOwnerId
					,@INQDate
					,1
					,2
					,@INQDate
					,@FollowupComments
					,@INQDate
					,@TC_BusinessTypeId
					)

				SET @CallsId = SCOPE_IDENTITY();

				-- not creating active calla because that will be delete in following SP
				-- scheduling all esisitng inqlead with following sp
				EXECUTE TC_CallUserScheduling @TC_LeadId = @LeadIdOutput
					,@TC_Usersid = @LeadOwnerId
					,@NextFolloupDate = @NextFollowupDate
					,@TC_CallsId = @CallsId
					,@Comment = @FollowupComments

				SET @ReturnStatus = 1 -- success 
			END
			ELSE
				IF (@ExistingLeadStage = 1) -- first priority should be given to existing lead owner
				BEGIN
					-- Getting existing Call id
					DECLARE @ExistingInqLeadOwner INT

					SELECT @CallsId = TC_CallsId
						,@ExistingInqLeadOwner = A.TC_UsersId
					FROM TC_ActiveCalls A WITH (NOLOCK)
					WHERE TC_LeadId = @LeadId

					-- Lead is trasferring to user wha has added this inquiry
					UPDATE TC_Calls
					SET IsActionTaken = 1
						,-- TC_UsersId=@LeadOwnerId,
						ActionTakenOn = @INQDate
						,ActionComments = @FollowupComments
						,TC_CallActionId = 2
					WHERE TC_LeadId = @LeadId

					-- Here lead will be diverted based on user's role
					EXECUTE TC_CallUserScheduling @TC_LeadId = @LeadIdOutput
						,@TC_Usersid = @ExistingInqLeadOwner
						,@NextFolloupDate = @NextFollowupDate
						,@TC_CallsId = @CallsId
						,@Comment = @FollowupComments
						,@SecondUserId = @LeadOwnerId

					SET @ReturnStatus = 3 -- success 
				END
				ELSE
				BEGIN
					IF (@TC_INQLeadIdInner IS NULL) -- Inquiry lead was not there for same lead type	
					BEGIN
						DECLARE @Assignee INT

						SELECT TOP 1 @Assignee = R.UserId
						FROM TC_UsersRole R WITH (NOLOCK)
						JOIN TC_RolesMaster AS RM WITH (NOLOCK) ON R.RoleId = RM.TC_RolesMasterId
						WHERE RM.TC_InquiryTypeId = @LeadInqTypeId
							AND R.UserId IN (
								SELECT TC_userid
								FROM TC_InquiriesLead WITH (NOLOCK)
								WHERE TC_LeadId = @LeadIdOutput
									AND TC_UserId IS NOT NULL
								)

						IF (@Assignee IS NULL)
						BEGIN
							SET @Assignee = @LeadOwnerId
						END
						ELSE
						BEGIN
							SET @ReturnStatus = 3
						END

						UPDATE TC_InquiriesLead
						SET TC_LeadStageId = 2
							,TC_UserId = @Assignee
						WHERE TC_InquiriesLeadId = @INQLeadIdOutput

						IF NOT EXISTS (
								SELECT TOP 1 TC_LeadId
								FROM TC_ActiveCalls WITH (NOLOCK)
								WHERE TC_UsersId = @Assignee
									AND TC_LeadId = @LeadIdOutput
								)
						BEGIN
							IF (@NextFollowupDate IS NULL)
							BEGIN
								SET @NextFollowupDate = @INQDate
							END

							INSERT INTO TC_Calls (
								TC_LeadId
								,CallType
								,TC_UsersId
								,ScheduledOn
								,IsActionTaken
								,TC_CallActionId
								,ActionTakenOn
								,ActionComments
								,CreatedOn
								,TC_BusinessTypeId
								)
							VALUES (
								@LeadIdOutput
								,3
								,@Assignee
								,@INQDate
								,1
								,2
								,@INQDate
								,@FollowupComments
								,@INQDate
								,@TC_BusinessTypeId
								)

							EXEC TC_ScheduleCall @Assignee
								,@LeadIdOutput
								,3
								,@NextFollowupDate
								,NULL
								,@FollowupComments
								,NULL
								,@NextCallTo
								,@CallsId

							SET @ReturnStatus = 3
						END
					END
					ELSE
					BEGIN
						SET @ReturnStatus = 3 -- Lead owner was already there for this type of inquiry
					END
				END
		END
		ELSE -- inquiry added from other then trading cars
		BEGIN
			--IF(@ExistingCallType =1 OR @ExistingCallType=2) commented on 30-01-2013 3pm
			IF (@ExistingLeadStage = 1) -- Lead was in verification stage
			BEGIN
				UPDATE TC_InquiriesLead
				SET TC_UserId = @ExistingLeadOwner
					,TC_LeadStageId = 1
				WHERE TC_InquiriesLeadId = @INQLeadIdOutput

				SET @ReturnStatus = 1 --Success
			END
					--ELSE IF (@ExistingCallType =3 OR @ExistingCallType=4)
			ELSE
				IF (@ExistingLeadStage = 2) --commented on 30-01-2013 3pm
				BEGIN -- Lead was in consultation stage 
					IF (@TC_INQLeadIdInner IS NULL)
					BEGIN
						DECLARE @NewLeadOwner BIGINT

						SELECT TOP 1 @NewLeadOwner = R.UserId
						FROM TC_UsersRole R WITH (NOLOCK)
						JOIN TC_RolesMaster AS RM WITH (NOLOCK) ON R.RoleId = RM.TC_RolesMasterId
						WHERE RM.TC_InquiryTypeId = @LeadInqTypeId
							AND R.UserId IN (
								SELECT TC_userid
								FROM TC_InquiriesLead WITH (NOLOCK)
								WHERE TC_LeadId = @LeadIdOutput
									AND TC_UserId IS NOT NULL
								)

						IF (@NewLeadOwner IS NULL)
						BEGIN
							IF (@LeadInqTypeId <> 3)
							BEGIN
								SELECT TOP 1 @NewLeadOwner = U.Id
								FROM TC_Users U WITH (NOLOCK)
								INNER JOIN TC_UsersRole R WITH (NOLOCK) ON U.Id = R.UserId
								INNER JOIN TC_RolesMaster T WITH (NOLOCK) ON T.TC_RolesMasterId = R.RoleId
								WHERE U.BranchId = @BranchId
									AND T.TC_InquiryTypeId = @LeadInqTypeId
									AND U.IsActive = 1
									AND T.IsActive = 1
									AND U.IsCarwaleUser = 0
								ORDER BY U.TodaysCallCount
							END
							ELSE
							BEGIN
								IF (
										(
											SELECT COUNT(U.Id)
											FROM TC_Users AS U WITH (NOLOCK)
											JOIN TC_UserModelsPermission AS TCUP WITH (NOLOCK) ON TCUP.TC_UsersId = U.Id
												AND U.BranchId = @BranchId
											) <> 0
										)
								BEGIN
									SELECT TOP 1 @NewLeadOwner = U.UserId
									FROM TC_UsersRole U WITH (NOLOCK)
									JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId = R.TC_RolesMasterId
									JOIN TC_UserModelsPermission AS UMP WITH (NOLOCK) ON U.UserId = UMP.TC_UsersId
									JOIN CarVersions AS CV WITH (NOLOCK) ON UMP.ModelId = CV.CarModelId
									JOIN TC_NewCarInquiries AS NCI WITH (NOLOCK) ON CV.ID = NCI.VersionId
									JOIN TC_InquiriesLead L WITH (NOLOCK) ON NCI.TC_InquiriesLeadId = L.TC_InquiriesLeadId
										AND L.BranchId = @BranchId
									WHERE R.TC_InquiryTypeId = @LeadInqTypeId
								END
								ELSE
								BEGIN
									SELECT TOP 1 @NewLeadOwner = U.Id
									FROM TC_Users U WITH (NOLOCK)
									INNER JOIN TC_UsersRole R WITH (NOLOCK) ON U.Id = R.UserId
									INNER JOIN TC_RolesMaster T WITH (NOLOCK) ON T.TC_RolesMasterId = R.RoleId
									WHERE U.BranchId = @BranchId
										AND T.TC_InquiryTypeId = @LeadInqTypeId
										AND U.IsActive = 1
										AND T.IsActive = 1
										AND U.IsCarwaleUser = 0
									ORDER BY U.TodaysCallCount
								END
							END

							UPDATE TC_Users
							SET TodaysCallCount = TodaysCallCount + 1
							WHERE Id = @NewLeadOwner
						END

						UPDATE TC_InquiriesLead
						SET TC_LeadStageId = 2
							,TC_UserId = @NewLeadOwner
						WHERE TC_InquiriesLeadId = @INQLeadIdOutput

						IF NOT EXISTS (
								SELECT TOP 1 TC_LeadId
								FROM TC_ActiveCalls WITH (NOLOCK)
								WHERE TC_UsersId = @NewLeadOwner
									AND TC_LeadId = @LeadIdOutput
								)
						BEGIN
							EXEC TC_ScheduleCall @NewLeadOwner
								,@LeadIdOutput
								,3
								,@INQDate
								,NULL
								,NULL
								,NULL
								,NULL
								,@CallsId
						END
					END
					ELSE
					BEGIN
						SET @ReturnStatus = 3 --diverted 
					END
				END
		END
	END

	-- This code will check whether lead is diverted to different user
	IF @ReturnStatus = 3
	BEGIN
		SELECT @LeadDivertedTo = U.UserName
			,@LeadOwnId = U.Id
		FROM TC_InquiriesLead L WITH (NOLOCK)
		INNER JOIN TC_Users U WITH (NOLOCK) ON L.TC_UserId = U.Id
			AND L.TC_LeadInquiryTypeId = @LeadInqTypeId
		WHERE L.TC_InquiriesLeadId = @INQLeadIdOutput

		IF (@LeadOwnId = @LeadOwnerId)
		BEGIN
			SET @LeadDivertedTo = NULL
		END
	END
END
