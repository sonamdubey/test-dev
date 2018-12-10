IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DB_GetUserTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DB_GetUserTarget]
GO

	
-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 26-02-2016
-- Description:	fetch user target and achivment depending upon UserId(AliasUserId)
-- Modified By : Mihir A Chheda ON 11/3/2016 removed the BusinessUnitId condition from achievement fetch query
-- EXEC DCRM_DB_GetUserTarget 3 ,2 ,'2016-01-01', '2016-04-01'
-- Modified By : Sunil M. Yadav on 17th may 2016 , Get user level 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DB_GetUserTarget] 
	@UserId				INT ,
	@BusinessUnitId     INT ,
	@StartDate			DATE,
	@EndDate			DATE
	
AS
BEGIN
    DECLARE @RevenueTarget		NUMERIC(18,2)=0
	DECLARE @RevenueAchived		NUMERIC(18,2)=0
	DECLARE @RenewalTarget		NUMERIC(18,2)=0
	DECLARE @RenewalAchived		NUMERIC(18,2)=0
	DECLARE @NewCarLeadsTarget	NUMERIC(18,2)=0
	DECLARE @NewCarLeadsAchived NUMERIC(18,2)=0
	DECLARE @NewSignupsTarget	NUMERIC(18,2)=0
	DECLARE @NewSignupsAchived	NUMERIC(18,2)=0
	DECLARE @TemptTable Table(ID INT,MonthNumber INT,NumberOfDays INT,YearNumer INT,ActualDuration INT)
	DECLARE @TempStartDate DATE
	DECLARE @TempEndDate DATE
	DECLARE @MonthNumber INT
	DECLARE @YearNumber  INT
	DECLARE @NumberOfDays NUMERIC(18,2)
	DECLARE @Id INT = 1
	DECLARE @EndId INT
	DECLARE @ActualDays NUMERIC(18,2)
	DECLARE @Target  NUMERIC(18,2) = 0
	DECLARE @UserLevel INT 
	DECLARE @UserNode HIERARCHYID
	DECLARE @TempDescendantUserIds TABLE (UserId INT,BusinessUnitId INT)

	-- Sunil M. Yadav on 17th may 2016 , Get user level 
	SELECT @UserLevel = UserLevel ,  @UserNode = NodeRec
	FROM DCRM_ADM_MappedUsers WITH(NOLOCK) 
	WHERE OprUserId = @UserId AND IsActive = 1

	IF(@UserLevel < 4 )
		BEGIN
			INSERT INTO @TempDescendantUserIds(UserId,BusinessUnitId)
			SELECT OprUserId,BusinessUnitId 
			FROM DCRM_ADM_MappedUsers WITH(NOLOCK)
			WHERE NodeRec.IsDescendantOf(@UserNode) = 1 
				 AND UserLevel = 4 AND IsActive = 1 --To get the L3 .
		END
	ELSE
		BEGIN
			INSERT INTO @TempDescendantUserIds VALUES(@UserId,@BusinessUnitId)
		END

    SET @TempStartDate = DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@StartDate),1))
    SET @TempEndDate = DATEADD(DAY, 1, @EndDate)

	WHILE (@TempStartDate < @TempEndDate)
	BEGIN   
	   SET @MonthNumber=DATEPART(MM,DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@TempStartDate),1)))
		SET @NumberOfDays=DAY(DATEADD(DD,-1,DATEADD(MM,DATEDIFF(MM,-1,@TempStartDate),0)))
		SET @YearNumber=DATEPART(YYYY,DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@TempStartDate),1)))

		--If data is required for last quarter then change the value of @YearNumber to @YearNumber -1 
        IF @MonthNumber < 4
	   BEGIN
		  SET @YearNumber=@YearNumber-1
		END

		INSERT INTO @TemptTable (ID,MonthNumber,NumberOfDays,YearNumer,ActualDuration) 
		VALUES (@Id,@MonthNumber,@NumberOfDays,@YearNumber,@NumberOfDays)
		
		SET @TempStartDate = DATEADD(MONTH, 1, @TempStartDate)
		SET @Id=@Id+1
	END

	SET @Id=1
	SET @EndId=(SELECT COUNT(ID) FROM @TemptTable)

	--Reset actual duration by actual numer of days specified in date range
	IF @EndId > 1
	BEGIN
		DECLARE @TempDuration1 INT=DATEDIFF(DAY,@StartDate,DATEADD(DAY,-1,DATEADD(MONTH,DATEDIFF(MONTH,-1,@StartDate),0)))+1
		DECLARE @TempDuration2 INT=DATEDIFF(DAY,DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@EndDate),1)),@EndDate)+1

		UPDATE @TemptTable
		SET    ActualDuration=@TempDuration1
		WHERE  ID=@Id

		UPDATE @TemptTable
		SET    ActualDuration=@TempDuration2
		WHERE  ID=@EndId

	END
	ELSE
	BEGIN
	   UPDATE @TemptTable
		SET    ActualDuration=(DATEDIFF(DAY, @StartDate,@EndDate))+1
		WHERE  ID=@Id
	END
		 
    --Calculate the target value depending upon the date range passed 		  
    WHILE (@Id <= @EndId)
		BEGIN   
		
		SELECT @MonthNumber=MonthNumber,@YearNumber=YearNumer,@NumberOfDays=NumberOfDays,@ActualDays=ActualDuration
		FROM   @TemptTable
		WHERE  Id=@Id
		 
		SET @Target=0		 		 	
		---get New Car/used car  Revenue target value for user  according to business unit
		SELECT	@Target=DFE.UserTarget 
		FROM	DCRM_FieldExecutivesTarget(NOLOCK) DFE
		WHERE	DFE.BusinessUnitId=@BusinessUnitId AND DFE.OprUserId=@UserId AND DFE.TargetYear=@YearNumber 
				AND DFE.TargetMonth=@MonthNumber AND DFE.MetricId=CASE WHEN @BusinessUnitId=2 THEN 7 ELSE 9 END --New Car Revenue:7 | Used Car Revenue:9
		SET     @RevenueTarget=@RevenueTarget+(@Target/@NumberOfDays)*@ActualDays

		SET @Target=0
		---New Contract Leads
		SELECT	@Target=DFE.UserTarget 
		FROM	DCRM_FieldExecutivesTarget(NOLOCK) DFE
		WHERE   DFE.BusinessUnitId=@BusinessUnitId AND DFE.OprUserId=@UserId AND DFE.TargetYear=@YearNumber 
				AND DFE.TargetMonth=@MonthNumber AND DFE.MetricId=CASE WHEN @BusinessUnitId=2 THEN 8 ELSE 10 END --New Car Leads:8 | Used Car New Paid Dealres:10
        
		IF @BusinessUnitId=2
		BEGIN
			SET	   @NewCarLeadsTarget=@NewCarLeadsTarget+(@Target/@NumberOfDays)*@ActualDays
		END
        
		ELSE
		BEGIN	 		 
			SET		@NewSignupsTarget=@NewSignupsTarget+(@Target/@NumberOfDays)*@ActualDays
		END	 			 

		SET @Id=@Id+1		
	END

	-- round of all the values 
	SET @RevenueTarget=ROUND(@RevenueTarget,0)
	set @NewCarLeadsTarget=ROUND(@NewCarLeadsTarget,0)
	SET @NewSignupsTarget=ROUND(@NewSignupsTarget,0)

     --set new /used car revenue Achivment value depending on business unit 
	SELECT	@RevenueAchived=ISNULL(SUM(DBRV.TotalDelivered*DBRV.CostPerUnit),0)
	FROM	DCRM_DB_Revenue(NOLOCK) DBRV
	WHERE  DBRV.UserId IN (SELECT DISTINCT UserId FROM @TempDescendantUserIds)
			AND CONVERT(DATE,DBRV.EntryDate) BETWEEN @StartDate AND @EndDate --AND DBRV.BusinessUnitId=@BusinessUnitId
        
	--set new lead Achievement by adding only lead based contract
	SELECT	@NewCarLeadsAchived=CASE WHEN @BusinessUnitId=2 THEN ISNULL(SUM(TotalDelivered),0) ELSE 0 END
	FROM	DCRM_DB_Revenue(NOLOCK) DBRV
	WHERE   DBRV.UserId IN (SELECT DISTINCT UserId FROM @TempDescendantUserIds)
			AND CONVERT(DATE,DBRV.EntryDate) BETWEEN @StartDate AND @EndDate AND DBRV.ContractBehaviour=1 --AND DBRV.BusinessUnitId=@BusinessUnitId


	--set used car NewSignupsAchived value by user 
	SELECT	@NewSignupsAchived=COUNT(DBNP.Id)
	FROM	DCRM_DB_NewPaidDealers(NOLOCK) DBNP
	WHERE   DBNP.UserId IN (SELECT DISTINCT UserId FROM @TempDescendantUserIds)
			AND CONVERT(DATE,DBNP.EntryDate) BETWEEN @StartDate AND @EndDate 

    
	---get RenewalTarget/Renewal Achivment value for new car/use car user
	SELECT @RenewalTarget=COUNT(DRW.Id),@RenewalAchived=ISNULL(SUM(CASE WHEN DRW.IsRenewed= 1 THEN 1 ELSE 0 END ),0) 
	FROM   DCRM_DB_Renewals(NOLOCK) DRW
	WHERE  DRW.UserId IN (SELECT DISTINCT UserId FROM @TempDescendantUserIds)
		  AND CONVERT(DATE,DRW.EntryDate) BETWEEN @StartDate AND @EndDate --AND DRW.BusinessUnitId=@BusinessUnitId
    
 
    --Final Target is stored in table
	DECLARE @FinalResult Table(Id INT,Lable VARCHAR(100),TargetValue INT,AchievementValue INT)

	INSERT INTO @FinalResult(Id,Lable,TargetValue,AchievementValue) VALUES (1,'Revenue',@RevenueTarget,@RevenueAchived) 
	INSERT INTO @FinalResult(Id,Lable,TargetValue,AchievementValue) VALUES (2,'New Contract Leads',@NewCarLeadsTarget,@NewCarLeadsAchived) 
    INSERT INTO @FinalResult(Id,Lable,TargetValue,AchievementValue) VALUES (3,'Renewal',@RenewalTarget,@RenewalAchived) 
	INSERT INTO @FinalResult(Id,Lable,TargetValue,AchievementValue) VALUES (4,'New Signups',@NewSignupsTarget,@NewSignupsAchived) 

	SELECT Id,Lable,TargetValue,AchievementValue FROM @FinalResult
END

----------------------------------------------------------------------------------------------------------------------------------------------------------


