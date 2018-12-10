IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListUpdate_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListUpdate_V16]
GO

	

-- =========================================================================================
-- Owner      :  Deepak    
-- Create date : 20-June-2016
-- Modified By : Ashwini Dhamankar on June 30,2016 (Added logic for bucket types of service)
-- Modified By : Deepak Tripathi on July 26,2016 (Added vwAllMMV to get CarName)
-- Modified By : Suresh Prajapari on 27th July, 2016
-- Description : Added Bucket creation for Advantage Leads.
-- Modified By : Ashwini Dhamankar on Aug 8,2016 (Modified buckettype logic,leaddispositionid based)
-- Modified By : Nilima More on 9th Aug 2016,add drop requested bucket(25 bucket id and leaddispositionid = 115).
-- Modified By : Khushaboo Patil on 10 Aug 2016 show drop cancel and drop complete in completed tab
-- Modified By : Khushaboo patil on 16th Aug 2016 added condition to show pickup cancelled in book tab 
-- Modifed  By : Nilima More On 23rd August 2016,fetch registration number and insert into TC_taskList,added bucket change logic for insurance.
-- Modifed  By : Nilima More On 6th Sept 2016,added call today logic for insurance and service.
-- Modified By : Komal Manjare 14 sept 2016 added leaddispositionId for payment failed
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : Categorized Insurance leads into 'New', 'Call Today' and 'All' buckets only
--Modified By :  Deepak on 27th Sep 2016 - Separated Sales from insurance and service data population logic
--Modified By: Tejashree Patil on 28 Sept 2016, Versioning of SP [TC_TaskListUpdateSales]  to [TC_TaskListUpdateSales_V16.10.1] 
-- =========================================================================================
CREATE PROCEDURE [dbo].[TC_TaskListUpdate_V16.10.1]
	-- Add the parameters for the stored procedure here  
	@ActionId TINYINT
	,@CallId BIGINT
	,@ApplicationId TINYINT = 1
AS
BEGIN

	DECLARE @TC_BusinessTypeId TINYINT = 3
	SELECT @TC_BusinessTypeId = TC_BusinessTypeId FROM TC_Lead WITH (NOLOCK) WHERE TC_LeadId = (SELECT TC_LeadId FROM TC_ActiveCalls WITH (NOLOCK) WHERE TC_CallsId = @CallId)
	SET @TC_BusinessTypeId = ISNULL(@TC_BusinessTypeId,3)
	
	IF (@ActionId = 1)
	BEGIN
		DELETE
		FROM TC_TaskLists
		WHERE TC_CallsId = @CallId
	END
	ELSE
		--DECLARE @LeadId AS INT
		IF @ActionId = 2
		BEGIN
			IF NOT EXISTS (SELECT TC_CallsId FROM TC_TaskLists WITH (NOLOCK)WHERE TC_CallsId = @CallId)
			BEGIN
				IF @TC_BusinessTypeId = 6 -- Insurance
					EXEC TC_TaskListUpdateInsurance_16_10_1 @CallId, @ApplicationId
				ELSE
				IF @TC_BusinessTypeId = 4  --service    --added by : Ashwini Dhamankar on Sept 28,2016
					EXEC TC_TaskListUpdateService @CallId, @ApplicationId
				ELSE  --sales and advantage
					EXEC [TC_TaskListUpdateSales_V16.10.1] @CallId, @ApplicationId
			END
		END
END

