IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQLeadAssignment]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQLeadAssignment]
GO

	-- Created By:	Surendra
-- Create date: 17 Jan 2012
-- Description:	Reassignment of Inquiries
-- Modified By: Tejashree Patil on 12 Feb 2013 at 6pm, Inserted LeadCreationDate in ScheduledOn from TC_Calls instead of GETDATE()
-- Modified By: Surendra on 21 March 2013 changing schedule on from null to getdata() for insert into active call
-- Modified By: Manish on 05-04-2013 for correctin lead assignment when lead is already assigned to selected user
-- Modified By: Manish on 03-06-2013 for Considering lead with no stage also
-- Modified By: Umesh  on 26-07-2013 for Lead disposition log for records (who transferred lead from U1 to U2)
-- Modified By: Manish on 21-04-2014 added WITH (Nolock) keyword wherever not found.
-- Modified By Vivek Gupta on 03-08-2015, Inserted data in case if data not exists in tC_active calls 
-- Modified By Vivek Gupta on 21-07-2016, Added sp to schedule call and removed query
-- Modified By : Suresh Prajapati on 03rd Aug, 2016
-- Description : Added BusinessTypeId for advantage sales executive assignment for SP - TC_SheduleCall
-- Modified By : Suresh Prajapati on 05th Aug, 2016
-- Description : Removed variable @LeadDispId 
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQLeadAssignment] @BranchId BIGINT
	,@UserID BIGINT
	,@InqLeadIds VARCHAR(MAX)
	,@ModifiedBy BIGINT
AS
BEGIN
	DECLARE @Separator_position INT -- This is used to locate each separator character 
	-- DECLARE @LeadDispId INT -- for taking lead dispostion id fro making log in dispostion log table
	DECLARE @TC_InquiriesLeadId VARCHAR(30) -- this holds each array value as it is returned  
	DECLARE @IsAdvantageExecutive BIT = 0

	SET @InqLeadIds = @InqLeadIds + ','

	IF EXISTS (
			SELECT 1
			FROM TC_UsersRole WITH (NOLOCK)
			WHERE RoleId = 20
				AND UserId = @UserID
			)
	BEGIN
		SET @IsAdvantageExecutive = 1
	END

	DECLARE @BusinessTypeId TINYINT = CASE @IsAdvantageExecutive
			WHEN 1
				THEN 5 -- Advantage
			ELSE 3 -- Sales
			END

	-- Loop through the string searching for separtor characters    
	WHILE PATINDEX('%' + ',' + '%', @InqLeadIds) <> 0
	BEGIN
		-- patindex matches the a pattern against a string  
		SELECT @Separator_position = PATINDEX('%' + ',' + '%', @InqLeadIds)

		SELECT @TC_InquiriesLeadId = LEFT(@InqLeadIds, @Separator_position - 1)

		DECLARE @LeadId BIGINT
		DECLARE @LeadStage TINYINT = NULL
		DECLARE @TC_CallsId BIGINT
		DECLARE @ScheduledDate DATETIME -- =GETDATE()--Commented By: Tejashree Patil on 12 Feb 2013 at 6pm
		DECLARE @OldLeadOwnerId BIGINT

		-- Getting lead stage and leadid based on tc_inquiryleadid
		SELECT @LeadId = IL.TC_LeadId
			,@LeadStage = IL.TC_LeadStageId
			,@OldLeadOwnerId = IL.TC_UserId
		FROM TC_InquiriesLead IL WITH (NOLOCK) --INNER JOIN TC_Lead L ON IL.TC_LeadId=L.TC_LeadId
		WHERE IL.TC_InquiriesLeadId = @TC_InquiriesLeadId
			AND IL.BranchId = @BranchId
			--AND IL.TC_LeadStageId<>3
			AND (
				IL.TC_LeadStageId <> 3
				OR IL.TC_LeadStageId IS NULL
				) -- Modified by Manish on  03-06-2013

		IF (@LeadId IS NOT NULL) -- checking for security purpose only
		BEGIN
			IF (
					@LeadStage IS NULL
					OR @LeadStage = 1
					)
			BEGIN
				SET @ScheduledDate = NULL

				SELECT @ScheduledDate = LeadCreationDate
				FROM TC_Lead L WITH (NOLOCK)
				WHERE TC_LeadId = @LeadId

				-- assigning new lead owner for verification
				UPDATE TC_Lead
				SET LeadVerifiedBy = @UserId
					,TC_LeadStageId = 1
				WHERE TC_LeadId = @LeadId

				-- Assigning all type inquiries lead to same user for verifications
				--Modified By: Tejashree Patil on 12 Feb 2013 at 6pm, ModifiedDate=GETDATE() instead of @ScheduledDate
				UPDATE TC_InquiriesLead
				SET TC_UserId = @UserId
					,ModifiedBy = @ModifiedBy
					,ModifiedDate = GETDATE()
					,TC_LeadStageId = 1
				WHERE TC_LeadId = @LeadId

				-- Updating call also
				IF (@LeadStage IS NULL) -- need to create calls
				BEGIN
					EXEC TC_ScheduleCall @UserId
						,@LeadId
						,1
						,@ScheduledDate
						,NULL
						,NULL
						,NULL
						,NULL
						,@TC_CallsId
						,NULL
						,@BusinessTypeId
				END
				ELSE
				BEGIN -- calls need to update because lead is already in verification stage	
					EXEC TC_TransferCalls @OldLeadOwnerId
						,@UserId
						,@LeadId
						,2
						,@BusinessTypeId
				END
			END
			ELSE -- It means lead in consultation stage
			BEGIN
				--Modified By: Tejashree Patil on 12 Feb 2013 at 6pm, ModifiedDate=GETDATE() instead of @ScheduledDate		
				UPDATE TC_InquiriesLead
				SET TC_UserId = @UserId
					,ModifiedBy = @ModifiedBy
					,ModifiedDate = GETDATE()
				WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId

				--Transfer the call
				EXEC TC_TransferCalls @OldLeadOwnerId
					,@UserId
					,@LeadId
					,3
					,@BusinessTypeId
			END

			--Insert data into Lead disposition log for records (who transferred lead from U1 to U2)
			--SELECT TOP 1 @LeadDispId = TC_LeadDispositionId
			--FROM TC_LeadDisposition WITH (NOLOCK)
			--WHERE NAME = 'Lead Transferred'

			INSERT INTO TC_DispositionLog (
				TC_LeadDispositionId
				,InqOrLeadId
				,TC_DispositionItemId
				,EventCreatedOn
				,EventOwnerId
				,TC_LeadId
				,LeadOwnerId
				,NewLeadOwnerId
				)
			VALUES (
				76 -- Lead Transferred
				,@TC_InquiriesLeadId
				,2
				,GETDATE()
				,@ModifiedBy
				,@LeadId
				,@OldLeadOwnerId
				,@UserId
				)
		END

		-- This replaces what we just processed with and empty string  
		SELECT @InqLeadIds = STUFF(@InqLeadIds, 1, @Separator_position, '')
	END
END
