IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CallUserScheduling]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CallUserScheduling]
GO

	-- =====================================================================================================================================
-- Author:  Manish  
-- Create date: 14-Jan-12  
-- Description: Scheduling calls to users 
-- Details: SP will schedule the call to user accordint to their role.
-- Remarks: This sp only scheduling the call to user when Lead will cross the verification stage
-- and call will schedule according to Inquiry type in that lead.
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE"
-- Modified By: Surendar on 13 march for changing roles table
-- Modified By: Manish  on 25 April 2013 for user model permission flag 
-- Modified By: Nilesh on 19-08-2013 Added parameter @TC_NextActionId for capturing next action during diversion
-- Modified By: Manish Chourasiya on 15-04-2014 added with(nolock) keyword wherever not found.
-- Modified By: Vivek Gupta on 2-3-2015, to avoide unknown lead vanish
-- Modified By Vivek Gupta on 24-09-2015, removed check of TC_inquiryTypeId = @TC_LeadInquiryTypeId while retrieving @UserId role wise
-- Modified By Vivek Gupta on 24-09-2015, commented retrieval of @UserId for used car and new car permissions both
-- Modified By Vivek Gupta on 09-01-2016, added @NextCallTo
-- Modified By : Suresh Prajapati on 18th July, 2016
-- Description : Passed parameter @BusinessTypeId for SP : TC_ScheduleCall
-- ======================================================================================================================================
CREATE PROCEDURE [dbo].[TC_CallUserScheduling] @TC_LeadId AS INT
	,@TC_Usersid AS INT
	,-- first priority owner
	@NextFolloupDate AS DATETIME
	,@TC_CallsId AS INT
	,@Comment AS VARCHAR(max)
	,@SecondUserId AS INT = NULL
	,@TC_NextActionId AS SMALLINT = NULL -- Added By Nilesh on 19-08-2013 for capturing next action during diversion   
	,@NextCallTo AS SMALLINT = NULL
	,@BusinessTypeId TINYINT =3
AS
BEGIN
	DECLARE @DealerId AS INT
	DECLARE @TC_InquiriesLeadId AS BIGINT
	DECLARE @TC_LeadInquiryTypeId AS TINYINT
	DECLARE @UserId AS INT
	DECLARE @LoopRunCount AS TINYINT = 1
	DECLARE @ExistingLeadOwner INT
	DECLARE @totalRecord TINYINT
	DECLARE @CheckUserModelPermissionFlag BIT = 0
	DECLARE @InquiryTypeId TINYINT

	DECLARE @TempTblInqLead TABLE (
		Id TINYINT identity(1, 1)
		,TC_InquiriesLeadId INT NOT NULL
		,TC_LeadInquiryTypeId TINYINT NOT NULL
		,ExistingLeadOwner INT
		)

	---------Inserting all inquiry type corresponding to any lead------------
	INSERT INTO @TempTblInqLead (
		TC_InquiriesLeadId
		,TC_LeadInquiryTypeId
		,ExistingLeadOwner
		)
	SELECT TC_InquiriesLeadId
		,TC_LeadInquiryTypeId
		,TC_UserId
	FROM TC_InquiriesLead WITH (NOLOCK)
	WHERE TC_LeadId = @TC_Leadid
		AND IsActive = 1
		AND (
			TC_LeadStageId <> 3
			OR TC_LeadStageId IS NULL
			)
		AND (
			TC_LeadDispositionID IS NULL
			OR TC_LeadDispositionID = 4
			) -- Booked(BikeWale) or nothing has been done yet
		AND IsActive = 1
	ORDER BY TC_InquiriesLeadId DESC

	SET @totalRecord = @@ROWCOUNT

	SELECT @DealerId = BranchId
	FROM TC_Users WITH (NOLOCK)
	WHERE Id = @TC_Usersid

	--------------Update customer verification and fake status after showing interest------------    
	UPDATE TC_CustomerDetails
	SET IsVerified = 1
		,IsFake = 0
	WHERE ActiveLeadId = @TC_LeadId

	----------------Updating  Lead verification id and lead stage id since lead is moving form verification to consultation stage     
	UPDATE TC_Lead
	SET LeadVerifiedBy = @TC_Usersid
		,LeadVerificationDate = GETDATE()
		,TC_LeadDispositionId = 2
		,---------Verified
		TC_LeadStageId = 2 -----Consultation Stage
	WHERE TC_LeadId = @TC_LeadId

	DELETE
	FROM TC_ActiveCalls
	WHERE TC_CallsId = @TC_CallsId

	------------Allocating user to all inquity type corresponding to any lead-------------                         
	WHILE @LoopRunCount <= @totalRecord
	BEGIN
		SELECT @TC_LeadInquiryTypeId = TC_LeadInquiryTypeId
			,@TC_InquiriesLeadId = TC_InquiriesLeadId
			,@ExistingLeadOwner = ExistingLeadOwner
		FROM @TempTblInqLead
		WHERE ID = @LoopRunCount

		DECLARE @IsLeadDiverted BIT = 0

		-- check for first priority user
		/* 15-03-2013 SELECT  @UserId=U.Id  FROM TC_Users U INNER JOIN TC_RoleTasks R ON U.RoleId=R.RoleId



								           INNER JOIN TC_Tasks T     ON T.Id=R.TaskId 



								WHERE 



								    T.TC_InquiryTypeId=@TC_LeadInquiryTypeId



								AND U.IsActive=1



								AND T.IsActive=1



								AND U.Id=@TC_Usersid



								AND U.IsCarwaleUser=0     15-03-2013*/
		IF @TC_LeadInquiryTypeId = 3
		BEGIN
			IF (
					SELECT COUNT(U.Id)
					FROM TC_Users AS U WITH (NOLOCK)
					JOIN TC_UserModelsPermission AS TCUP WITH (NOLOCK) ON TCUP.TC_UsersId = U.Id
						AND U.BranchId = @DealerId
					JOIN CarVersions AS CV WITH (NOLOCK) ON TCUP.ModelId = CV.CarModelId
					JOIN TC_NewCarInquiries AS NCI WITH (NOLOCK) ON CV.ID = NCI.VersionId
						AND TC_InquiriesLeadId = @TC_InquiriesLeadId
					) <> 0
				---condition change by Manish  on 25 April 2013 for user model permission flag 
			BEGIN
				SET @CheckUserModelPermissionFlag = 1
			END
		END

		--IF 	((@TC_LeadInquiryTypeId=1 OR 	@TC_LeadInquiryTypeId=2 ) OR (@TC_LeadInquiryTypeId=3 AND @CheckUserModelPermissionFlag=0))
		-- BEGIN 
		--	  SELECT  @UserId=U.UserId  FROM TC_UsersRole U WITH (NOLOCK)
		--	                           INNER JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId=R.TC_RolesMasterId
		--					           WHERE --R.TC_InquiryTypeId=@TC_LeadInquiryTypeId
		--							    	--AND 
		--									U.UserId=@TC_Usersid
		-- END
		--ELSE IF (@TC_LeadInquiryTypeId=3 AND @CheckUserModelPermissionFlag=1)
		--   BEGIN
		--   SELECT TOP 1 @UserId=U.UserId  FROM TC_UsersRole U WITH (NOLOCK)
		--                 JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId=R.TC_RolesMasterId
		--                 JOIN TC_UserModelsPermission AS UMP WITH (NOLOCK) ON U.UserId=UMP.TC_UsersId
		--                 JOIN CarVersions AS CV WITH (NOLOCK) ON UMP.ModelId=CV.CarModelId
		--                 JOIN TC_NewCarInquiries AS NCI WITH (NOLOCK) ON CV.ID=NCI.VersionId AND TC_InquiriesLeadId=@TC_InquiriesLeadId
		--          WHERE --R.TC_InquiryTypeId=@TC_LeadInquiryTypeId
		--	    	--AND 
		--			U.UserId=@TC_Usersid
		--END
		SET @UserId = @TC_Usersid

		-- check for already assigned user						
		IF (
				@UserId IS NULL
				AND @SecondUserId IS NOT NULL
				AND @TC_Usersid <> @SecondUserId
				)
		BEGIN
			/* 15-03-2013  SELECT  @UserId=U.Id  FROM TC_Users U INNER JOIN TC_RoleTasks R ON U.RoleId=R.RoleId



								           INNER JOIN TC_Tasks T     ON T.Id=R.TaskId 



								WHERE 



								    T.TC_InquiryTypeId=@TC_LeadInquiryTypeId



								AND U.IsActive=1



								AND T.IsActive=1



								AND U.Id=@SecondUserId



						AND U.IsCarwaleUser=0     15-03-2013*/
			IF (
					(
						@TC_LeadInquiryTypeId = 1
						OR @TC_LeadInquiryTypeId = 2
						)
					OR (
						@TC_LeadInquiryTypeId = 3
						AND @CheckUserModelPermissionFlag = 0
						)
					)
			BEGIN
				SELECT @UserId = U.UserId
				FROM TC_UsersRole U WITH (NOLOCK)
				INNER JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId = R.TC_RolesMasterId
				WHERE --R.TC_InquiryTypeId=@TC_LeadInquiryTypeId
					--AND 
					U.UserId = @SecondUserId
			END
			ELSE
				IF (
						@TC_LeadInquiryTypeId = 3
						AND @CheckUserModelPermissionFlag = 1
						)
				BEGIN
					SELECT TOP 1 @UserId = U.UserId
					FROM TC_UsersRole U WITH (NOLOCK)
					JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId = R.TC_RolesMasterId
					JOIN TC_UserModelsPermission AS UMP WITH (NOLOCK) ON U.UserId = UMP.TC_UsersId
					JOIN CarVersions AS CV WITH (NOLOCK) ON UMP.ModelId = CV.CarModelId
					JOIN TC_NewCarInquiries AS NCI WITH (NOLOCK) ON CV.ID = NCI.VersionId
						AND TC_InquiriesLeadId = @TC_InquiriesLeadId
					WHERE --R.TC_InquiryTypeId=@TC_LeadInquiryTypeId
						--AND 
						U.UserId = @SecondUserId
				END
		END

		-- takin random user for same inquiry type	
		IF (@UserId IS NULL)
		BEGIN
			/* 15-03-2013	 SELECT TOP 1 @UserId=U.Id  FROM TC_Users U INNER JOIN TC_RoleTasks R ON U.RoleId=R.RoleId



										INNER JOIN TC_Tasks T ON T.Id=R.TaskId 



										WHERE U.BranchId=@DealerId 



										AND T.TC_InquiryTypeId=@TC_LeadInquiryTypeId



										AND U.IsActive=1



										AND T.IsActive=1



										AND U.IsCarwaleUser=0



										ORDER BY U.TodaysCallCount	  15-03-2013 */
			IF (
					(
						@TC_LeadInquiryTypeId = 1
						OR @TC_LeadInquiryTypeId = 2
						)
					OR (
						@TC_LeadInquiryTypeId = 3
						AND @CheckUserModelPermissionFlag = 0
						)
					)
			BEGIN
				SELECT TOP 1 @UserId = U.UserId
				FROM TC_Users AS TCU WITH (NOLOCK)
				INNER JOIN TC_UsersRole U WITH (NOLOCK) ON TCU.Id = U.UserId
				INNER JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId = R.TC_RolesMasterId
				WHERE --R.TC_InquiryTypeId=@TC_LeadInquiryTypeId
					--AND 
					TCU.IsCarwaleUser = 0
					AND TCU.BranchId = @DealerId
					AND TCU.IsActive = 1
			END
			ELSE
				IF (
						@TC_LeadInquiryTypeId = 3
						AND @CheckUserModelPermissionFlag = 1
						)
				BEGIN
					SELECT TOP 1 @UserId = U.UserId
					FROM TC_Users AS TCU WITH (NOLOCK)
					JOIN TC_UsersRole U WITH (NOLOCK) ON TCU.Id = U.UserId
					JOIN TC_RolesMaster R WITH (NOLOCK) ON U.RoleId = R.TC_RolesMasterId
					JOIN TC_UserModelsPermission AS UMP WITH (NOLOCK) ON U.UserId = UMP.TC_UsersId
					JOIN CarVersions AS CV WITH (NOLOCK) ON UMP.ModelId = CV.CarModelId
					JOIN TC_NewCarInquiries AS NCI WITH (NOLOCK) ON CV.ID = NCI.VersionId
						AND TC_InquiriesLeadId = @TC_InquiriesLeadId
					WHERE --R.TC_InquiryTypeId=@TC_LeadInquiryTypeId
						--AND 
						TCU.IsCarwaleUser = 0
						AND TCU.BranchId = @DealerId
						AND TCU.IsActive = 1
				END
						--EXEC TC_DispositionLogInsert  @TC_Usersid,40,@TC_InquiriesLeadId, 2 ,@TC_LeadId
		END

		-- Below code added by Vivek Gupta on 2-3-2015, to avoid unknown lead vanish
		IF (@UserId IS NOT NULL)
		BEGIN
			--EXEC TC_DispositionLogInsert  @TC_Usersid,40,@TC_InquiriesLeadId, 2 ,@TC_LeadId 
			UPDATE TC_InquiriesLead
			SET TC_UserId = @UserId
				,--------update  User id and Stage id into InquiriesLead Table
				TC_LeadStageId = 2
			WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId
				AND IsActive = 1
		END
		ELSE
		BEGIN
			UPDATE TC_InquiriesLead
			SET TC_UserId = @TC_Usersid
				,--------update  User id and Stage id into InquiriesLead Table
				TC_LeadStageId = 2
			WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId
				AND IsActive = 1
		END

		IF NOT EXISTS (
				SELECT TC_CallsId
				FROM TC_ActiveCalls WITH (NOLOCK)
				WHERE TC_USERSID = @UserId
					AND TC_LeadId = @TC_LeadId
				)
		BEGIN
			UPDATE TC_Users
			SET TodaysCallCount = TodaysCallCount + 1 ------Update count of call for user
			WHERE Id = @UserId

			DECLARE @ScopeIdentity AS INT
				,@AlertId INT
				,@CallType INT

			SELECT @AlertId = AlertId
			FROM TC_Calls WITH (NOLOCK)
			WHERE TC_CallsId = @TC_CallsId

			SET @NextFolloupDate = ISNULL(@NextFolloupDate, GETDATE())

			EXEC TC_ScheduleCall @UserId
				,@TC_LeadId
				,3
				,@NextFolloupDate
				,@AlertId
				,@Comment
				,@TC_NextActionId
				,@NextCallTo
				,@ScopeIdentity
				,NULL
				,@BusinessTypeId
		END

		SET @LoopRunCount = @LoopRunCount + 1
		SET @UserId = NULL
	END -----end of while loop         
END

