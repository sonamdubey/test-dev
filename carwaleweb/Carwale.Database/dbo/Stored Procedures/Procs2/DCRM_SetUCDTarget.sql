IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SetUCDTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SetUCDTarget]
GO

	-- =============================================
-- Author:		Kartik Rathod
-- Create date: 2 March 2016
-- Description:	set the target for the user on the basis of total of target from there reportee. ie. target for L2 is total of All L3 target who are reporting to L2 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SetUCDTarget]
	@DCRM_TblFieldExecutivesTarget DCRM_TblFieldExecutivesTarget READONLY,
	@TargetYear VARCHAR(4),
	@MetricId SMALLINT,
	@AddedBy INT,
	@QuarterId SMALLINT
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @UserLevel INT, @UserNode HIERARCHYID,@UserBusinessId INT,	@OprUser INT , 	@TotalTarget INT,@TargetMonthId  INT,@MonthCount INT,@NoOfRow SMALLINT


	SELECT Distinct TOP 3  OprUserId ,Month AS TargetMonth				--this will fetch only one opruser from @DCRM_TblFieldExecutivesTarget
	INTO #TempTable
	FROm @DCRM_TblFieldExecutivesTarget ORDER BY OprUserId
		
	--SELECT * FROM #TempTable
	
	SELECT DISTINCT TOP 1  @TargetMonthId = TargetMonth FROM #TempTable ORDER BY TargetMonth		--this will select first month for Quarter
	
	SELECT @MonthCount = Count(*) FROM #TempTable GROUP BY OprUserID 
	
	WHILE(@MonthCount > 0)					--loop will run for each month
	BEGIN 

		SELECT	@OprUser = MU.OprUserId							--@OprUser will give the first Ancestor OprUserId for there Reportee 
		FROM	DCRM_ADM_MappedUsers MU WITH(NOLOCK)
		JOIN (	SELECT MU.NodeRec.GetAncestor(1) Ancestor FROM DCRM_ADM_MappedUsers MU 	WITH(NOLOCK) 
				WHERE 	
					OprUserId = (SELECT DISTINCT TOP 1 OprUserId FROm #TempTable ORDER BY OprUserId) AND MU.IsActive = 1 
					AND MU.BusinessUnitId = 1) AS  A  ON A.Ancestor = MU.NodeRec
	
		--SELECT @OprUser opruser
		
		SELECT	@UserLevel = MU.UserLevel,	@UserNode=MU.NodeRec FROM	DCRM_ADM_MappedUsers MU WITH(NOLOCK) WHERE	MU.OprUserId=@OprUser AND MU.BusinessUnitId = 1

		--SELECT @Count Count
	
			WHILE(@UserLevel > 0)			--While loop will perform on the basis of UserLevel ie.for L2 its 3 then it will run for L1 and NationalHead
			BEGIN 
				--SELECT @OprUser OprUser,@UserLevel UserLevel,@TargetMonthId Months

				IF @OprUser IS NOT NULL
				BEGIN
					SELECT	@TotalTarget = SUM(ISNULL(ET.UserTarget,0))					--this will calculate the total of target of all Reportee of User
					FROM	DCRM_ADM_MappedUsers MU WITH(NOLOCK) 
					JOIN		OprUsers OU WITH(NOLOCK) ON OU.Id = MU.OprUserId AND MU.IsActive=1
					LEFT JOIN	DCRM_FieldExecutivesTarget ET WITH(NOLOCK) ON ET.OprUserId = MU.OprUserId	AND ET.TargetMonth = @TargetMonthId	AND ET.TargetYear = @TargetYear	
								AND ET.MetricId = @MetricId	AND ET.BusinessUnitId= 1 
					WHERE 
							MU.NodeRec.IsDescendantOf(@UserNode) = 1 AND MU.UserLevel = @UserLevel + 1

					--SELECT @TotalTarget TargetAncestor

					--check if target is already set for the executive
					SELECT Id FROM DCRM_FieldExecutivesTarget WITH(NOLOCK)
					WHERE OprUserId = @OprUser AND TargetMonth = @TargetMonthId AND TargetYear = @TargetYear AND MetricId = @MetricId AND BusinessUnitId = 1
					
					SET @NoOfRow = @@ROWCOUNT
					--If not then make new entry
					IF @NoOfRow = 0												--if there is no entry for opruser then insert opruser for that metric and Quarter
					BEGIN
						INSERT INTO DCRM_FieldExecutivesTarget
								(	OprUserId,
									BusinessUnitId,
									MetricId,
									UserTarget,
									TargetMonth,
									TargetYear,
									QuarterId,
									AddedBy,
									AddedOn
								)
								VALUES
								(	@OprUser,
									1,
									@MetricId,
									@TotalTarget,
									@TargetMonthId,
									@TargetYear,
									@QuarterId,
									@AddedBy,
									GETDATE()
								)
					END
					ELSE IF @NoOfRow = 1
					BEGIN
						UPDATE DCRM_FieldExecutivesTarget							--this will set the total of target for that user
						SET		UserTarget = @TotalTarget,
								UpdatedBy = @AddedBy,
								UpdatedOn = GETDATE()
							WHERE
								OprUserId = @OprUser  AND TargetMonth = @TargetMonthId	AND TargetYear = @TargetYear AND MetricId = @MetricId	AND BusinessUnitId = 1
					END
			
						SELECT	@OprUser = MU.OprUserId,@UserNode=MU.NodeRec		--this will set the new ancestor of current @opruser
						FROM	DCRM_ADM_MappedUsers MU WITH(NOLOCK)
						JOIN	(SELECT *,MU.NodeRec.GetAncestor(1) Ancestor FROm DCRM_ADM_MappedUsers MU 	WITH(NOLOCK) WHERE 	OprUserId = @OprUser AND MU.IsActive = 1 ) AS  TR ON TR.Ancestor = MU.NodeRec
					
				END	--if ends here
				SET @UserLevel = @UserLevel - 1 
			END		--inner WHILE() ends here
		
		SET @MonthCount = @MonthCount - 1
		SET @TargetMonthId = @TargetMonthId + 1

	END
		
		DROP TABLE #TempTable
END