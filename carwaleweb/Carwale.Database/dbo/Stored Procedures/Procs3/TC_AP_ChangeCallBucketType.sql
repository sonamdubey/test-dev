IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AP_ChangeCallBucketType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AP_ChangeCallBucketType]
GO

	-- =============================================
-- Author:		<Deepak Tripathi>
-- Create date: <Create Date, 26th June, 2016>
-- Description:	<Chage the lead buckets>
-- Modified By : Suresh Prajapati on 26th Jul, 2016
-- Description : Added Bucket creation for Advantage Leads
-- Modified BY : Ashwini Dhamankar on Aug 9,2016 (Modified logic of pick up request)
-- Modified BY : Nilima More on Aug 9,2016 (Added logic of drop request)
-- Modified BY : Nilima More on Aug 23,2016,change in pending logic for insurance and service
-- Modified BY : Nilima More on Sept 6,2016,change in call today logic for insurance and service
-- Modified By : Suresh Prajapati on 16th Sept, 2016
-- Description : Removed bucket update logic for insurance other than New, Call Today and Pending
-- =============================================
CREATE PROCEDURE [dbo].[TC_AP_ChangeCallBucketType]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--New Lead
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_BusinessTypeId
			WHEN 3
				THEN 2 -- Sales New Lead
			WHEN 4
				THEN 11 -- Service New Lead
			WHEN 5
				THEN 24 -- Advantage New Lead
			WHEN 6
				THEN 26 -- Insurance New Lead
			END
	WHERE CONVERT(DATE, TC_InquiriesLeadCreateDate) = CONVERT(DATE, GETDATE())
		AND TC_CallTypeId = 1
		AND BucketTypeId IS NULL

	-- Set for Call Today
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_BusinessTypeId
			WHEN 3
				THEN 3 -- Sales Call Today
					--WHEN 4
					--	THEN 12 -- Service Call Today
			WHEN 5
				THEN 19 -- Advantage Call Today
					--WHEN 6
					--	THEN 27 -- Insurance Call Today
			END
	WHERE CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
		AND ISNULL(TC_NextActionId, 0) <> 1
		AND TC_BusinessTypeId IN (
			3
			,5
			)
		AND ISNULL(BucketTypeId, 0) NOT IN (
			3
			,6
			,19
			,22
			) -- Lets not change those already tagged booked and call-today

	------added by Nilima More On 6th Sept 2016,added call today logic for service and insurance
	-- Set for Call Today
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_BusinessTypeId
			WHEN 4
				THEN 12 -- Service Call Today
			WHEN 6
				THEN 27 -- Insurance Call Today
			END
	WHERE CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
		AND ISNULL(TC_NextActionId, 0) = 2
		AND TC_BusinessTypeId IN (
			4
			,6
			)
		AND ISNULL(BucketTypeId, 0) NOT IN (
			15 --service booked
			,12 -- service call today
			,27 -- insurance call today
			,29 -- insurance confirmed
			) -- Lets not change those already tagged booked and call-today

	--Set for Personal Visit
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_BusinessTypeId
			WHEN 3
				THEN 4 -- Sales Personal Visit
			WHEN 5
				THEN 20 -- Advantage Personal Visit
			END
	WHERE CONVERT(DATE, ScheduledOn) = CONVERT(DATE, GETDATE())
		AND ISNULL(TC_NextActionId, 0) = 1
		AND ISNULL(BucketTypeId, 0) NOT IN (
			4
			,6
			,20
			,22
			) -- Lets not change those already tagged booked and call-today
		AND TC_BusinessTypeId <> 4

	--Booked Leads
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_BusinessTypeId
			WHEN 3
				THEN 6 -- Sales
			WHEN 5
				THEN 22 -- Advantage
			END
	WHERE TC_BusinessTypeId <> 4
		AND TC_LeadDispositionId = 4
		AND ISNULL(BucketTypeId, 0) NOT IN (
			6
			,22
			)

	--Set 2 months older data
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_BusinessTypeId
			WHEN 3
				THEN 9 -- Sales
			WHEN 5
				THEN 23 -- Advantage
			END
	WHERE DATEDIFF(day, CONVERT(DATE, ScheduledOn), CONVERT(DATE, GETDATE())) > 59
		AND ISNULL(TC_LeadDispositionId, 0) <> 4
		AND TC_BusinessTypeId IN (
			3
			,5
			)
		AND BucketTypeId NOT IN (
			6
			,9
			,22
			,23
			) -- Lets not change those already tagged booked and 2 months older
		AND TC_BusinessTypeId <> 4

	--Set Pending data
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_BusinessTypeId
			WHEN 3
				THEN 5 -- Sales
			WHEN 4
				THEN 14 -- Service
			WHEN 5
				THEN 21 -- Advantage
			WHEN 6
				THEN 28 -- Insurance
			END
	WHERE CONVERT(DATE, ScheduledOn) < CONVERT(DATE, GETDATE())
		AND ISNULL(TC_LeadDispositionId, 0) <> 4
		AND BucketTypeId NOT IN (
			5
			,6
			,9
			,14
			,15
			,21
			,22
			,23
			,11 --Service New --Added By Nilima More On 23rd Aug.
			,26 --Insurance New
			,28 --Pending
			) -- Lets not change those already tagged booked and pending and 2 months older and those who are in new tab of service and insurance

	-- service pick up request and drop request
	UPDATE TC_TaskLists
	SET BucketTypeId = CASE TC_LeadDispositionID
			WHEN 106
				THEN 13
			WHEN 115
				THEN 25
			END
	WHERE TC_BusinessTypeId = 4
		AND DATEDIFF(DD, GETDATE(), TC_NextActionDate) <= 0
		AND BucketTypeId NOT IN (
			13
			,25
			)
		---Added by Nilima More On 23 Aug 2016,added bucket logic for cheque pick up,pay at showroom for insurance
		--UPDATE TC_TaskLists
		--SET BucketTypeId = CASE TC_LeadDispositionID
		--		WHEN 120
		--			THEN 30 --cheque pick up
		--		WHEN 121
		--			THEN 31 --pay at showroom
		--		END
		--WHERE TC_BusinessTypeId = 6
		--	AND DATEDIFF(DD, GETDATE(), TC_NextActionDate) <= 0
		--	AND BucketTypeId NOT IN (
		--		30
		--		,31
		--		)
END
