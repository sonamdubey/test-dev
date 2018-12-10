IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NCDFeedbackPushLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NCDFeedbackPushLead]
GO

	
CREATE PROCEDURE [dbo].[CRM_NCDFeedbackPushLead]
AS 
--Name of SP/Function                     : CarWale.[CRM_NCDFeedbackPushLead]
--Applications using SP                   : CRM
--Modules using the SP                    : NCDFeedbackPushLead.cs
--Technical department                    : Database
--Summary							      : NCDFeedbackPushLead
--Author								  : AMIT Kumar 30th april 2013
--Modification history                    : 1 
BEGIN

	DECLARE @LeadCount	     INT						--total No of lead dealer wise 
	DECLARE @LeadToBePushed  INT						-- 75 per of the @leadCount
	DECLARE @DealerId		 NUMERIC(18,0)				-- Id of the dealer
	DECLARE @LoopCount       INT						-- Total no of the dealer . Used to iterate for outermost loop
	DECLARE @countLead		 INT						-- Total no of the lead which is going to be pushed
	DECLARE @countUser		 INT						-- Total no of the user(caller) to whome the lead is pushed
	DECLARE @OuterLoopCount  INT
	DECLARE @InnerLoopCount  INT
	
	DECLARE @id				 INT 
	DECLARE @LeadId			 NUMERIC(18,0) 
	DECLARE @CallType		 NUMERIC(18,0) 
	DECLARE @IsTeam			 BIT 
	DECLARE @CallerId		 NUMERIC(18,0) 
	DECLARE @ScheduledOn	 DATETIME 
	DECLARE @CreatedOn		 DATETIME 
	DECLARE @DealerId1		 NUMERIC(18,0) 
	DECLARE @CbdId			 NUMERIC(18,0)
	
	-----#tempUserId stores the data of telecallers
	CREATE TABLE #tempUserId( id INT IDENTITY(1,1) ,userId NUMERIC(18,0))
	INSERT INTO #tempUserId 
	SELECT DISTINCT UserId  FROM  CRM_ADM_UserRoles WITH(NOLOCK) WHERE RoleId = 6 AND IsActive = 1 --AND UserId IN(19,20)----hard coded 
	SELECT @countUser = COUNT(*) FROM #tempUserId
	--print '@countUser = ' --print @countUser
	-----#tempDealerWithLeadCount stores the data of dealerId and lead associated with that dealerId	
	CREATE TABLE #tempDealerWithLeadCount( id INT IDENTITY(1,1) ,leadCountToEvaluate int,dealerId NUMERIC(18,0))
	INSERT INTO #tempDealerWithLeadCount
		SELECT  COUNT(DISTINCT CL.ID) AS leadcount,NCDC.ID  FROM NCS_Dealers NCDC WITH (NOLOCK)
			INNER JOIN CRM_CarDealerAssignment CDA WITH( NOLOCK)  ON NCDC.ID = CDA.DealerId AND NCDC.IsNCDDealer = 1 AND NCDC.IsActive =1
			INNER JOIN CRM_CarBasicData CBD WITH( NOLOCK) ON CBD. id = CDA .CBDId
			INNER JOIN CRM_Leads CL WITH( NOLOCK) ON CL. ID = CBD .LeadId
		WHERE  NCDC. IsActive = 1 
			AND CONVERT(date,CL.CreatedOn) = CONVERT(date,GETDATE()-7) 
			GROUP BY NCDC.ID ORDER BY NCDC.ID
	
	SELECT @LoopCount = COUNT(*) FROM #tempDealerWithLeadCount
	--print '@LoopCount ' --print @LoopCount
	WHILE(@LoopCount > 0)
		BEGIN
			--print 'inside loop and current value of dealer loop is ' --print @LoopCount
			SELECT @LeadCount = leadCountToEvaluate,@DealerId=dealerId FROM #tempDealerWithLeadCount WHERE id = @LoopCount
			--print 'leadCountToEvaluate is ' 
			--print @LeadCount 
			--print ' And dealer Id is '
			--print @DealerId
			SET @LeadToBePushed = (@LeadCount *75)/100
			--print ' lead to be pushed ' --print @LeadToBePushed
			CREATE TABLE #tempLead(idLead INT IDENTITY(1,1), leadId NUMERIC(18,0),dealerId NUMERIC(18,0),createdOn DATETIME,CBDid NUMERIC(18,0),CardealerId NUMERIC(18,0))
			IF(@LeadToBePushed < 10)
				BEGIN
					--print 'in if block. here count is less than 10';
					WITH CTE AS(
					SELECT  DISTINCT CL.ID AS LeadId,NCDC.ID AS DealerId ,CL .CreatedOn ,CBD. ID AS CBDId ,CDA. Id AS CarDealerId
					FROM NCS_Dealers NCDC WITH (NOLOCK)
					INNER JOIN CRM_CarDealerAssignment CDA WITH( NOLOCK) ON NCDC.ID = CDA.DealerId AND NCDC.IsNCDDealer = 1 AND NCDC.IsActive =1
					INNER JOIN CRM_CarBasicData CBD WITH( NOLOCK) ON CBD. id = CDA .CBDId
					INNER JOIN CRM_Leads CL WITH( NOLOCK) ON CL. ID = CBD .LeadId
					WHERE  NCDC. IsActive = 1 AND NCDC.ID = @DealerId AND CONVERT(date,CL.CreatedOn) = CONVERT(date,GETDATE()-7)
					)
					INSERT INTO #tempLead
					SELECT  * FROM CTE 
				END
			ELSE
				BEGIN
					--print 'in else block.here count is greater than 10';
					WITH CTE AS(
					SELECT  DISTINCT TOP (@LeadToBePushed) CL.ID AS LeadId, NCDC.ID AS DealerId ,CL .CreatedOn ,CBD. ID AS CBDId ,CDA. Id AS CarDealerId
					FROM NCS_Dealers NCDC WITH (NOLOCK)
					INNER JOIN CRM_CarDealerAssignment CDA WITH( NOLOCK)  ON NCDC.ID = CDA.DealerId AND NCDC.IsNCDDealer = 1 AND NCDC.IsActive =1
					INNER JOIN CRM_CarBasicData CBD WITH( NOLOCK) ON CBD. id = CDA .CBDId
					INNER JOIN CRM_Leads CL WITH( NOLOCK) ON CL. ID = CBD .LeadId
					WHERE  NCDC. IsActive = 1 AND NCDC.ID = @DealerId AND CONVERT(date,CL.CreatedOn) = CONVERT(date,GETDATE()-7)
					)
					INSERT INTO #tempLead
					SELECT  * FROM CTE     
				END
				
				SELECT @countLead =  COUNT(*) FROM #tempLead
				SET @OuterLoopCount =1
				--print 'count lead ' --print @countLead
				--print 'count user ' --print @countUser

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
								SELECT @DealerId1 =dealerId FROM  #tempLead WHERE idLead = @OuterLoopCount
								SELECT @CbdId =CBDid FROM  #tempLead WHERE idLead = @OuterLoopCount
								--print 'pushing in call table starts'
								INSERT INTO CRM_Calls
								(
									LeadId, CallType, IsTeam, CallerId, ScheduledOn, CreatedOn, DealerId , CBDId
								) 
								VALUES( @LeadId,@CallType,@IsTeam,@CallerId,@ScheduledOn, @CreatedOn,@DealerId1,@CbdId)
								
							 
							
								INSERT INTO CRM_CallActiveList
								(
									CallId, LeadId, CallType, IsTeam, CallerId, ScheduledOn, DealerId, CBDId
								)
								SELECT  id, LeadId,CallType,IsTeam,CallerId,ScheduledOn,DealerId,CbdId
								FROM CRM_Calls  WHERE  id=scope_identity()
								--print 'pushinng value ends '
								SET @InnerLoopCount = @InnerLoopCount + 1
								SET  @OuterLoopCount=@OuterLoopCount+1
								--print 'incremented counter @OuterLoopCount and @InnerLoopCount '
								--print @OuterLoopCount --print @InnerLoopCount
							END
							--print 'outside the inner loop'
						SET  @OuterLoopCount=@OuterLoopCount+1
						--print 'outer loop counter incremented to ' --print @OuterLoopCount
					END ---loop end
				--print 'outside the outer loop '
				--DROP TABLE #tempUserId
				DROP TABLE	#tempLead
				--print '#tempLead dropped'
			SET @LoopCount = @LoopCount-1
		END
		--print 'dropping #tempUserId and #tempDealerWithLeadCount'
		DROP TABLE #tempUserId
		DROP TABLE #tempDealerWithLeadCount
	----Print @Count
	--SET @LeadToBePushed = (@Count *75)/100
	----Print @LeadToBePushed
	--SELECT  DISTINCT CL.ID AS LeadId, NCDC.ID AS DealerId ,CL .CreatedOn ,CBD. ID AS CBDId ,CDA. Id AS CarDealerId
	--		FROM NCS_Dealers NCDC WITH (NOLOCK)
	--		INNER JOIN CRM_CarDealerAssignment CDA WITH( NOLOCK) ON NCDC.ID = CDA.DealerId AND NCDC.IsNCDDealer = 1 AND NCDC.IsActive =1
	--		INNER JOIN CRM_CarBasicData CBD WITH( NOLOCK) ON CBD. id = CDA .CBDId
	--		INNER JOIN CRM_Leads CL WITH( NOLOCK) ON CL. ID = CBD .LeadId
	--		WHERE  NCDC. IsActive = 1 AND DAY(CDA.CreatedOn) = DAY(GETDATE()-7) AND MONTH(CDA.CreatedOn) = MONTH(GETDATE()-7)
	--		AND YEAR(CDA.CreatedOn) = YEAR(GETDATE()-7)
END