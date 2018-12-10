IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMDataForDisplay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMDataForDisplay]
GO

	-- =============================================
-- Author	    :	Vinayak Patil
-- Create date	:	12-04-2013
-- Description	:	To Display Data For Selected Target Scenario 	
-- Modified By  :   Umesh Ojha on 16/12/2013 for reference data from final target save (target type)
---PAGE ID =0   Month Model wise
---PAGE ID =1   Zone  Model wise
---PAGE ID =2   AM Model wise
---PAGE ID =3   Dealer Model wise
---PAGE ID =4   AM Version wise
---PAGE ID =5   Dealer Version wise
---PAGE ID =6   Zone Version wise
---PAGE ID =7   Version  Month wise
--- EXEC  [dbo].[TC_TMPageWiseData] 	@PageId=7
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMDataForDisplay]  --0, 3, NULL, NULL, NULL, 1, 12, 1, 0, 1
	@PageId TINYINT,
	@TC_BrandZoneId TINYINT=NULL,
	@CarModelId INT=NULL,
	@TC_AMId INT=NULL,
	@DealerId INT =NULL,
	@StartMonth TINYINT=NULL,
	@EndMonth TINYINT =NULL,
	@TC_TMDistributionPatternMasterId1 INT,
	@TC_TMDistributionPatternMasterId2 INT,
	@IsFinalTarget BIT,
	@Year INT=null ------TO CHECK IF DATA IS TO BE FETCHED FROM TC_DealersTarget
	
AS

	BEGIN
		  
		IF(@IsFinalTarget = 0)  ------ FETCH DATA FROM TC_TMTargetScenarioDetail TABLE ONLY FOR RESPECTIVE TC_TMDistributionPatternMasterId
    
			BEGIN
			
        
				   IF (@PageId=0)  --Month Model wise
				   BEGIN
						 ---------- --Month Model wise
							SELECT   
								 CM.ID    CarId,
								 CM.Name  CarName, 
								 CASE [MONTH]   WHEN 1  THEN  'JAN'
											WHEN 2  THEN  'FEB'
											WHEN 3  THEN  'MAR'
											WHEN 4  THEN  'APR'
											WHEN 5  THEN  'MAY'
											WHEN 6  THEN  'JUN'
											WHEN 7  THEN  'JUL'
											WHEN 8  THEN  'AUG'
											WHEN 9  THEN  'SEP'
											WHEN 10  THEN  'OCT'
											WHEN 11  THEN  'NOV'
											WHEN 12  THEN  'DEC' END FieldName,
								 [Month] FieldId,
								 SUM(Target ) AS Target1,
								ROUND(SUM([dbo].[f_TC_TMTargetMapping](TM.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
								--ROUND(SUM(TM.Target*.25),0) Target2

						 FROM  TC_TMTargetScenarioDetail  AS TM  WITH (NOLOCK)
						 JOIN  Dealers AS D  WITH (NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV  WITH (NOLOCK)  ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM  WITH (NOLOCK) ON CV.CarModelId=CM.ID
						 JOIN TC_SpecialUsers AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						 GROUP BY  CM.Name,[Month],CM.ID
						 ORDER BY [Month],CM.ID
		    
				   END
		   	
				  ELSE IF  (@PageId=1)  --Zone  Model wise
				  BEGIN 

						 -------Zone  Model wise
						 SELECT   CM.ID   CarId, 
								  CM.Name  CarName, 
								  TCB.ZoneName  FieldName, 
								  TCB.TC_BrandZoneId FieldId , 
								  SUM(Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](TM.Target,@TC_TMDistributionPatternMasterId2)),0) Target2

						 FROM  TC_TMTargetScenarioDetail AS TM  WITH (NOLOCK)
						 JOIN  Dealers AS D  WITH (NOLOCK) ON D.ID=TM.DealerId
						 JOIN  TC_BrandZone  AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV  WITH (NOLOCK) ON CV.ID=TM.CarVersionId
						 JOIN  CarModels AS CM  WITH (NOLOCK) ON CV.CarModelId=CM.ID
						 JOIN TC_SpecialUsers AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
				 
						 WHERE 	(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
								AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
								AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
								AND (D.ID=@DealerId OR @DealerId IS NULL)
								AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
								AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						 GROUP BY  CM.ID,CM.Name,TCB.ZoneName,TCB.TC_BrandZoneId
						 ORDER BY  TCB.TC_BrandZoneId,CM.ID

				  END 

		
				  ELSE IF (@PageId=2)  --AM Model wise
				   BEGIN 
						--------AM and Model wise
						SELECT  S.TC_SpecialUsersId  FieldId,
								S.UserName FieldName,
								M.Id  CarId,
								M.Name CarName, 
								SUM(Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](TM.Target,@TC_TMDistributionPatternMasterId2)),0) Target2

						FROM TC_TMTargetScenarioDetail AS TM  WITH (NOLOCK)
						JOIN Dealers           AS D  WITH (NOLOCK) ON TM.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=TM.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK)  ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						GROUP BY S.TC_SpecialUsersId,S.UserName,M.Id,M.Name	 
						ORDER BY S.TC_SpecialUsersId,M.ID     
           
				   END
				ELSE IF (@PageId=3) ---  Dealer Model wise
				   BEGIN
	        
						---------Dealer and Model wise 
						SELECT  D.Id  FieldId ,
								D.Organization FieldName ,
								M.Id  CarId ,
								M.Name  CarName , 
								SUM(I.Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](I.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						FROM TC_TMTargetScenarioDetail AS I  WITH (NOLOCK)
						JOIN Dealers           AS D  WITH (NOLOCK) ON I.DealerId=D.Id
						JOIN CarVersions       AS V   WITH (NOLOCK) ON V.Id=I.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND I.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						GROUP BY D.Id,D.Organization,M.Id,M.Name
						ORDER BY D.ID,M.ID

				  END 

				ELSE IF (@PageId=4) --- AM Version wise
				   BEGIN
					   ---------AM and Version wise 
						SELECT S.TC_SpecialUsersId FieldId,
						S.UserName FieldName ,
						V.Id CarId,
						M.Name+' '+V.Name CarName,
						SUM(I.Target ) AS Target1,
						ROUND(SUM([dbo].[f_TC_TMTargetMapping](I.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						FROM TC_TMTargetScenarioDetail AS I  WITH (NOLOCK)
						JOIN Dealers           AS D  WITH (NOLOCK) ON I.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=I.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND I.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						GROUP BY S.TC_SpecialUsersId,S.UserName,V.Id,M.Name+' '+V.Name
						ORDER BY S.TC_SpecialUsersId,V.ID
				   END

			   ELSE IF (@PageId=5) --- Dealer  Version wise
				   BEGIN
		 					 ---------Dealer and Version wise 
						SELECT D.Id FieldId ,
						D.Organization FieldName ,
						V.Id CarId ,
						M.Name+' '+V.Name CarName, 
						SUM(I.Target ) AS Target1,
					    ROUND(SUM([dbo].[f_TC_TMTargetMapping](I.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						FROM TC_TMTargetScenarioDetail AS I  WITH (NOLOCK)
						JOIN Dealers           AS D  WITH (NOLOCK) ON I.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=I.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND I.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						GROUP BY D.Id,D.Organization,V.Id,M.Name+' '+V.Name
						ORDER BY D.ID,V.ID
				 END

			ELSE IF (@PageId=6) --- Zone Version wise
				 BEGIN
		
						------Zone and Version wise
						SELECT TCB.TC_BrandZoneId FieldId ,
						TCB.ZoneName FieldName,
						M.Name+' '+V.Name CarName,
						V.Id CarId,
						SUM(I.Target ) AS Target1,
						ROUND(SUM([dbo].[f_TC_TMTargetMapping](I.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						FROM TC_TMTargetScenarioDetail AS I  WITH (NOLOCK)
						JOIN Dealers           AS D  WITH (NOLOCK) ON I.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=I.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels		   AS M  WITH (NOLOCK) ON  M.Id=V.CarModelId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND I.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						GROUP BY TCB.ZoneName,TCB.TC_BrandZoneId,M.Name+' '+V.Name,V.Id
						ORDER BY TCB.TC_BrandZoneId,V.ID

				END 

			 ELSE IF (@PageId=7)  --Version  Month wise
				BEGIN
	          					---------Version  and Month wise 
						SELECT V.Id CarId ,M.Name+' '+ V.Name CarName,I.[Month] FieldId, 
						CASE [MONTH]   WHEN 1  THEN  'JAN'
										WHEN 2  THEN  'FEB'
										WHEN 3  THEN  'MAR'
										WHEN 4  THEN  'APR'
										WHEN 5  THEN  'MAY'
										WHEN 6  THEN  'JUN'
										WHEN 7  THEN  'JUL'
										WHEN 8  THEN  'AUG'
										WHEN 9  THEN  'SEP'
										WHEN 10  THEN  'OCT'
										WHEN 11  THEN  'NOV'
										WHEN 12  THEN  'DEC' END FieldName,
										SUM(I.Target ) AS Target1,
								        ROUND(SUM([dbo].[f_TC_TMTargetMapping](I.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						FROM TC_TMTargetScenarioDetail AS I  WITH (NOLOCK)
						JOIN Dealers           AS D  WITH (NOLOCK)  ON I.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=I.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND I.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId= @TC_TMDistributionPatternMasterId1
						GROUP BY V.Id,M.Name+' '+ V.Name,I.[Month]
						ORDER BY [MONTH],V.ID
				END 

			END
	
	ELSE

----------------------FETCH DATA FROM TC_DealersTarget Table for Target1-------------------------- 
-- Modified By Deepak on 7th Dec 2013, Kept scenario Table in Left join because its optional

	BEGIN
		
		--First Take Secondary Table Data
		DECLARE @TempDealers Table(DealerId NUMERIC, CarVersionId NUMERIC, [Target] NUMERIC, 
							[Month] INT,[Year] INT)		
	
		INSERT INTO @TempDealers
		SELECT DealerId, CarVersionId, Target, [Month], [Year]
		FROM TC_TMTargetScenarioDetail WITH (NOLOCK) 
		WHERE TC_TMDistributionPatternMasterId = @TC_TMDistributionPatternMasterId2
		
        
				   IF (@PageId=0)  --Month Model wise
				   BEGIN
						 ---------- --Month Model wise
							SELECT   
								 CM.ID    CarId,
								 CM.Name  CarName, 
								 CASE DT.[MONTH]   WHEN 1  THEN  'JAN'
											WHEN 2  THEN  'FEB'
											WHEN 3  THEN  'MAR'
											WHEN 4  THEN  'APR'
											WHEN 5  THEN  'MAY'
											WHEN 6  THEN  'JUN'
											WHEN 7  THEN  'JUL'
											WHEN 8  THEN  'AUG'
											WHEN 9  THEN  'SEP'
											WHEN 10  THEN  'OCT'
											WHEN 11  THEN  'NOV'
											WHEN 12  THEN  'DEC' END FieldName,
								 DT.[Month] FieldId,
								 SUM(DT.Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						 JOIN  Dealers AS D  WITH (NOLOCK) ON D.ID=DT.DealerId
						 JOIN  TC_BrandZone  AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV  WITH (NOLOCK)  ON CV.ID=DT.CarVersionId
						 JOIN  CarModels AS CM  WITH (NOLOCK) ON CV.CarModelId=CM.ID
						 JOIN TC_SpecialUsers AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						 WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
						AND DT.[Year] = @year
						AND DT.TC_TargetTypeId = 4
						 GROUP BY  CM.Name,DT.[Month],CM.ID
						 ORDER BY DT.[Month],CM.ID
		    
				   END
		   	
				  ELSE IF  (@PageId=1)  --Zone  Model wise
				  BEGIN 

						 -------Zone  Model wise
						 SELECT   CM.ID   CarId, 
								  CM.Name  CarName, 
								  TCB.ZoneName  FieldName, 
								  TCB.TC_BrandZoneId FieldId , 
								SUM(DT.Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						 JOIN  Dealers AS D  WITH (NOLOCK) ON D.ID=DT.DealerId
						 JOIN  TC_BrandZone  AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						 JOIN  CarVersions AS CV  WITH (NOLOCK) ON CV.ID=DT.CarVersionId
						 JOIN  CarModels AS CM  WITH (NOLOCK) ON CV.CarModelId=CM.ID
						 JOIN TC_SpecialUsers AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						 LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						        
						 WHERE 	(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
								AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
								AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
								AND (D.ID=@DealerId OR @DealerId IS NULL)
								AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
								AND DT.TC_TargetTypeId = 4
								AND DT.[Year] = @year
						 GROUP BY  CM.ID,CM.Name,TCB.ZoneName,TCB.TC_BrandZoneId
						 ORDER BY  TCB.TC_BrandZoneId,CM.ID

				  END 

		
				  ELSE IF (@PageId=2)  --AM Model wise
				   BEGIN 
						--------AM and Model wise
						SELECT  S.TC_SpecialUsersId  FieldId,
								S.UserName FieldName,
								M.Id  CarId,
								M.Name CarName, 
								 SUM(DT.Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						 
						JOIN Dealers           AS D  WITH (NOLOCK) ON DT.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=DT.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK)  ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
						AND DT.TC_TargetTypeId = 4
						AND DT.[Year] = @year
						GROUP BY S.TC_SpecialUsersId,S.UserName,M.Id,M.Name	 
						ORDER BY S.TC_SpecialUsersId,M.ID     
           
				   END
				ELSE IF (@PageId=3) ---  Dealer Model wise
				   BEGIN
	        
						---------Dealer and Model wise 
						SELECT  D.Id  FieldId ,
								D.Organization FieldName ,
								M.Id  CarId ,
								M.Name  CarName , 
								 SUM(DT.Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						JOIN Dealers           AS D  WITH (NOLOCK) ON DT.DealerId=D.Id
						JOIN CarVersions       AS V   WITH (NOLOCK) ON V.Id=DT.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
						AND DT.[Year] = @year
						AND DT.TC_TargetTypeId = 4
						GROUP BY D.Id,D.Organization,M.Id,M.Name
						ORDER BY D.ID,M.ID

				  END 

				ELSE IF (@PageId=4) --- AM Version wise
				   BEGIN
					   ---------AM and Version wise 
						SELECT S.TC_SpecialUsersId FieldId,
						S.UserName FieldName ,
						V.Id CarId,
						M.Name+' '+V.Name CarName,
						 SUM(DT.Target ) AS Target1,
						 ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						 
						JOIN Dealers           AS D  WITH (NOLOCK) ON DT.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=DT.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
						AND DT.TC_TargetTypeId = 4
						AND DT.[Year] = @year
						GROUP BY S.TC_SpecialUsersId,S.UserName,V.Id,M.Name+' '+V.Name
						ORDER BY S.TC_SpecialUsersId,V.ID
				   END

			   ELSE IF (@PageId=5) --- Dealer  Version wise
				   BEGIN
		 					 ---------Dealer and Version wise 
						SELECT D.Id FieldId ,
						D.Organization FieldName ,
						V.Id CarId ,
						M.Name+' '+V.Name CarName, 
						SUM(DT.Target ) AS Target1,
						ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						 
						JOIN Dealers           AS D  WITH (NOLOCK) ON DT.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=DT.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
						AND DT.[Year] = @year
						AND DT.TC_TargetTypeId = 4
						GROUP BY D.Id,D.Organization,V.Id,M.Name+' '+V.Name
						ORDER BY D.ID,V.ID
				 END

			ELSE IF (@PageId=6) --- Zone Version wise
				 BEGIN
		
						------Zone and Version wise
						SELECT TCB.TC_BrandZoneId FieldId ,
						TCB.ZoneName FieldName,
						M.Name+' '+V.Name CarName,
						V.Id CarId,
						SUM(DT.Target ) AS Target1,
						ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						 
						JOIN Dealers           AS D  WITH (NOLOCK) ON DT.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=DT.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels		   AS M  WITH (NOLOCK) ON  M.Id=V.CarModelId
						LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
						AND DT.[Year] = @year
						AND DT.TC_TargetTypeId = 4
						GROUP BY TCB.ZoneName,TCB.TC_BrandZoneId,M.Name+' '+V.Name,V.Id
						ORDER BY TCB.TC_BrandZoneId,V.ID

				END 

			 ELSE IF (@PageId=7)  --Version  Month wise
				BEGIN
	          					---------Version  and Month wise 
						SELECT V.Id CarId ,M.Name+' '+ V.Name CarName,DT.[Month] FieldId, 
						CASE DT.[MONTH]   WHEN 1  THEN  'JAN'
										WHEN 2  THEN  'FEB'
										WHEN 3  THEN  'MAR'
										WHEN 4  THEN  'APR'
										WHEN 5  THEN  'MAY'
										WHEN 6  THEN  'JUN'
										WHEN 7  THEN  'JUL'
										WHEN 8  THEN  'AUG'
										WHEN 9  THEN  'SEP'
										WHEN 10  THEN  'OCT'
										WHEN 11  THEN  'NOV'
										WHEN 12  THEN  'DEC' END FieldName,
										 SUM(DT.Target ) AS Target1,
								 ROUND(SUM([dbo].[f_TC_TMTargetMapping](DT.Target,@TC_TMDistributionPatternMasterId2)),0) Target2
						 FROM TC_DealersTarget AS DT WITH (NOLOCK)
						 
						JOIN Dealers           AS D  WITH (NOLOCK)  ON DT.DealerId=D.Id
						JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=DT.CarVersionId
						JOIN TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
						JOIN TC_BrandZone      AS TCB  WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
						JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						LEFT JOIN @TempDealers TM ON DT.DealerId =TM.DealerId
						       AND DT.[Year] = TM.[Year] AND DT.[Month] = TM.[Month] AND DT.CarVersionId = TM.CarVersionId
						WHERE 
							(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND DT.[Month] BETWEEN @StartMonth AND @EndMonth
						AND DT.[Year] = @year
						AND DT.TC_TargetTypeId = 4
						GROUP BY V.Id,M.Name+' '+ V.Name,DT.[Month]
						ORDER BY DT.[MONTH],V.ID
				END 

			END
	
	
	END
