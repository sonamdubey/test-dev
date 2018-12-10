IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NCDFeedbackPushLeadAutomated]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NCDFeedbackPushLeadAutomated]
GO

	
CREATE PROCEDURE [dbo].[CRM_NCDFeedbackPushLeadAutomated]
AS 
BEGIN

	-- CRETED BY AMIT KUMAR ON 18th april 2013
	-- used for automation of NCD feedback pushled
	CREATE TABLE #tempUserId( id INT IDENTITY(1,1) ,userId NUMERIC(18,0))
	INSERT INTO #tempUserId 

	SELECT DISTINCT UserId  FROM  CRM_ADM_UserRoles WITH(NOLOCK) WHERE RoleId = 6 AND IsActive = 1 
	SELECT * FROM #tempUserId

	DECLARE @sql VARCHAR(200)='exec CRM_NCDFeedbackPushLead'
	CREATE TABLE #tempLead(idLead INT IDENTITY(1,1), leadId NUMERIC(18,0),dealerId NUMERIC(18,0),createdOn DATETIME,CBDid NUMERIC(18,0),CardealerId NUMERIC(18,0))
	INSERT INTO #tempLead 
	EXECUTE(@sql)

	DECLARE @countLead int
	DECLARE @countUser int 
	SELECT @countLead =  COUNT(*) FROM #tempLead
	--print @countLead
	SELECT @countUser = COUNT(*) FROM #tempUserId
	--print @countUser
 
 
	BEGIN
		DECLARE @OuterLoopCount AS INT=1
		DECLARE @InnerLoopCount AS INT=1
		
		DECLARE @id int 
		DECLARE @LeadId bigint 
		DECLARE @CallType bigint 
		DECLARE @IsTeam bit 
		DECLARE @CallerId bigint 
		DECLARE @ScheduledOn datetime 
		DECLARE @CreatedOn datetime 
		DECLARE @DealerId bigint 
		DECLARE @CbdId bigint 

		--SELECT @TotalRecords = COUNT(*) FROM @CRM_FeedBackPushLead

		WHILE ( @OuterLoopCount <= @countLead)
			BEGIN 
				SET @InnerLoopCount = 1
				WHILE(@InnerLoopCount <= @countUser AND  @OuterLoopCount <= @countLead  )
				BEGIN
					
					SELECT @LeadId =leadId FROM  #tempLead WHERE idLead = @OuterLoopCount
					SELECT @CallType = 20
					SELECT @IsTeam =0
					SELECT @CallerId =userId FROM  #tempUserId WHERE id = @InnerLoopCount
					SELECT @ScheduledOn =GETDATE()
					SELECT @CreatedOn =createdOn FROM  #tempLead WHERE idLead = @OuterLoopCount
					SELECT @DealerId =dealerId FROM  #tempLead WHERE idLead = @OuterLoopCount
					SELECT @CbdId =CBDid FROM  #tempLead WHERE idLead = @OuterLoopCount
					
					INSERT INTO CRM_Calls
					(
						LeadId, CallType, IsTeam, CallerId, ScheduledOn, CreatedOn, DealerId , CBDId
					) 
					VALUES( @LeadId,@CallType,@IsTeam,@CallerId,@ScheduledOn, @CreatedOn,@DealerId,@CbdId)
					
				 
				
					INSERT INTO CRM_CallActiveList
					(
						CallId, LeadId, CallType, IsTeam, CallerId, ScheduledOn, DealerId, CBDId
					)
					SELECT  id, LeadId,CallType,IsTeam,CallerId,ScheduledOn,DealerId,CbdId
					FROM CRM_Calls  WHERE  id=scope_identity()
					
					SET @InnerLoopCount = @InnerLoopCount + 1
					SET  @OuterLoopCount=@OuterLoopCount+1
				END
				SET  @OuterLoopCount=@OuterLoopCount+1
			END ---loop end
	END
		DROP TABLE #tempUserId
		DROP TABLE	#tempLead
END
