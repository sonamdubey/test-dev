IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetExecutivePerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetExecutivePerformance]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 19/08/2013
-- Description:	Fetches FLC top performers and low performers CRM_GetExecutivePerformance 1,5
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetExecutivePerformance] 
	-- Add the parameters for the stored procedure here
	@Group TINYINT,
	@Top TINYINT = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @LeadThreshold SMALLINT =40
	DECLARE @Date DATE=GETDATE()
	
	DECLARE @Table TABLE (
	    UserId INT,
		Executive VARCHAR(50),
		GroupType TINYINT,
		--Achievement FLOAT,
		Performance FLOAT
		)
    DECLARE @ExecutiveStars TABLE(
       TotalStar INT,
	   UId INT
	   )		

	INSERT INTO @Table
	SELECT DISTINCT Id,UserName,GroupType,
		--AVG(Ranking) OVER (PARTITION BY Username) Achieved,
		--AVG(Ranking) OVER (PARTITION BY Id) * MulFactor Performance
		AVG(Ranking* MulFactor) OVER (PARTITION BY Id) Performance 
	FROM (SELECT *,SUM(Leads) OVER (PARTITION BY Id) As SumLeads
		FROM(
		SELECT OU.Id,OU.UserName,
			FL.NAME AS GroupName,FL.GroupType,COUNT(DISTINCT CL.Id) Leads,
			(
				CAST(COUNT(DISTINCT CASE
								WHEN CDA.Id IS NOT NULL
								THEN CL.Id
								END) AS FLOAT) / COUNT(DISTINCT CL.Id)
				) * 100 Ranking,
			CGB.BenchMark,CGB.MulFactor
		FROM CRM_Calls CC WITH(NOLOCK)
		INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CC.CallerId
		INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CC.LeadId
		INNER JOIN CRM_ADM_FLCGroups FL WITH(NOLOCK) ON FL.Id = CL.GroupId
		INNER JOIN CRM_ADM_GroupBenchMark CGB WITH(NOLOCK) ON CGB.GroupId = CL.GroupId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CL.ID = CBD.LeadId
	    INNER JOIN CarVersions CV WITH(NOLOCK)ON CBD.VersionId=CV.Id 
		INNER JOIN CRM_ADM_GroupModelMapping CG WITH(NOLOCK) ON CV.CarModelId = CG.ModelId AND CG.GroupType = @Group
		LEFT JOIN  CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.CBDId = CBD.ID
			AND CONVERT(DATE, CDA.CreatedOn)  = @Date
		WHERE CC.IsActionTaken = 1
			AND CallType IN (
				1,
				2
				)
			AND IsTeam = 0
			AND FL.GroupType = @Group
			AND CONVERT(DATE, CC.ActionTakenOn) = @Date AND OU.Id <>13
		GROUP BY OU.Id,OU.UserName,
			FL.NAME,
			CGB.BenchMark,CGB.MulFactor,FL.GroupType
		) AS Tab ) AS InTab WHERE SumLeads>= @LeadThreshold AND Leads>=ROUND(MulFactor,0)
	   

    INSERT INTO @ExecutiveStars
		SELECT COUNT(CFDP.UserId) AS TotalStar,CFDP.UserId AS UId
		FROM CRM_FLCDailyPerformers AS CFDP WITH(NOLOCK)
		WHERE CFDP.Performance>=100
			AND  YEAR(CFDP.CreatedOn)= YEAR(GETDATE()) AND MONTH(CFDP.CreatedOn)= MONTH(GETDATE()) 
		GROUP BY CFDP.UserId
	--ORDER BY Performance DESC,Achieved DESC
	IF(@Top IS NOT NULL)
	BEGIN
		SELECT TOP (@Top) *,ROW_NUMBER()OVER(ORDER BY Performance DESC) Row
		FROM @Table AS TT
		LEFT JOIN @ExecutiveStars AS ES ON TT.UserId=ES.UId
		WHERE Performance >= 100
		ORDER BY Performance DESC

		SELECT TOP (@Top) *,ROW_NUMBER()OVER(ORDER BY Performance) Row
		FROM @Table
		WHERE Performance < 100
		ORDER BY Performance 
	END
	
	ELSE
	INSERT INTO CRM_FLCDailyPerformers
           ([UserId]
           ,[Executive]
           ,[GroupType]
           ,[Performance]
           ,[DayRank]
           ,ReportDate)
     SELECT *,ROW_NUMBER()OVER(ORDER BY Performance DESC) Row,@Date
	 FROM @Table
	 ORDER BY Performance DESC

END