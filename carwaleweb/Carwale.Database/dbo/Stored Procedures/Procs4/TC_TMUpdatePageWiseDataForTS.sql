IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMUpdatePageWiseDataForTS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMUpdatePageWiseDataForTS]
GO

	-- =============================================
-- Author	    :	Manish Chourasiya
-- Create date	:	19-11-2013
-- Description	:	Update and Get Pagewise details for Target Management 	
---PAGE ID =0   Month Model wise
---PAGE ID =1   Zone  Model wise
---PAGE ID =2   AM Model wise
---PAGE ID =3   Dealer Model wise
---PAGE ID =4   AM Version wise
---PAGE ID =5   Dealer Version wise
---PAGE ID =6   Zone Version wise
---PAGE ID =7   Version  Month wise
---PAGE ID =8   Model Version wise
--- EXEC  [dbo].[TC_TMPageWiseData] 	@PageId=3
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMUpdatePageWiseDataForTS] 
	@PageId TINYINT,
	@TC_BrandZoneId TINYINT=NULL,
	@carModelId INT=NULL,
	@TC_AMId INT=NULL,
	@TC_SpecialUsersId INT,
	@TC_TMPageWisePercentageChange TC_TMPageWisePercentageChange READONLY,   ---ID IDENTITY Column,FieldId,CarId,PercentageChange,NewValue
	@DealerId INT =NULL,
	@StartMonth TINYINT=NULL,
	@EndMonth TINYINT =NULL,
	@TC_TMDistributionPatternMasterId INT

AS
	BEGIN
        
		  DECLARE @TargetAfterRoundOff INT,
	              @ActualTarget INT,
				  @WhileLoopControl INT=1,
				  @TotalWhileLoopCount INT,
				  @FieldId INT,
				  @CarId INT

	      SELECT @TotalWhileLoopCount=COUNT(Id) FROM @TC_TMPageWisePercentageChange;
		  

           IF (@PageId=0)  --Month Model wise
		   BEGIN
		      
			     ---------- --Month Model wise update
			   UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	             FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=I.[Month] AND P.CarId=M.ID
				 WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId


				WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					 ------------Month Model wise update
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@TC_BrandZoneId,
						@CarModelId =@CarId,
						@TC_AMId =@TC_AMId,
						@DealerId  =@DealerId,
						@CarVersionId  =NULL,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=@FieldId,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 

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
						ROUND( SUM(Target),0) AS Target
				 FROM  TC_TMTargetScenarioDetail  AS TM
				 JOIN  Dealers AS D ON D.ID=TM.DealerId
				 JOIN  TC_BrandZone  AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN  CarVersions AS CV ON CV.ID=TM.CarVersionId
				 JOIN  CarModels AS CM ON CV.CarModelId=CM.ID
				 JOIN TC_SpecialUsers AS S ON S.TC_SpecialUsersId=D.TC_AMId
				 WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				 GROUP BY  CM.Name,[Month],CM.ID
				 ORDER BY [Month],CM.ID
		    
		   END
		   	
		  ELSE IF  (@PageId=1)  --Zone  Model wise
		  BEGIN 

				
				  ------------Zone  Model wise update
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	              FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=TCB.TC_BrandZoneId AND P.CarId=M.ID
				  WHERE 
						(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
					AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND (D.ID=@DealerId OR @DealerId IS NULL)
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
					AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				
				 
				 WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					 -------Zone  Model wise
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@FieldId,
						@CarModelId =@CarId,
						@TC_AMId =@TC_AMId,
						@DealerId  =@DealerId,
						@CarVersionId  =NULL,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=NULL,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 

				 -------Zone  Model wise
				 SELECT   CM.ID   CarId, 
						  CM.Name  CarName, 
						  TCB.ZoneName  FieldName, 
						  TCB.TC_BrandZoneId FieldId , 
						  ROUND(SUM(Target),0) AS Target
				 FROM  TC_TMTargetScenarioDetail AS TM
				 JOIN  Dealers AS D ON D.ID=TM.DealerId
				 JOIN  TC_BrandZone  AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN  CarVersions AS CV ON CV.ID=TM.CarVersionId
				 JOIN  CarModels AS CM ON CV.CarModelId=CM.ID
				 JOIN TC_SpecialUsers AS S ON S.TC_SpecialUsersId=D.TC_AMId
				 
				 WHERE 	(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
						AND (CM.Id=@CarModelId OR @CarModelId IS NULL)
						AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
						AND (D.ID=@DealerId OR @DealerId IS NULL)
						AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
						AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				 GROUP BY  CM.ID,CM.Name,TCB.ZoneName,TCB.TC_BrandZoneId
				 ORDER BY  TCB.TC_BrandZoneId,CM.ID

		  END 

		
		ELSE IF (@PageId=2)  --AM Model wise
	     BEGIN 

		       ----------AM and Model wise
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	             FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON S.TC_SpecialUsersId=P.FieldId AND P.CarId=M.ID
				 WHERE 
						(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
					AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND (D.ID=@DealerId OR @DealerId IS NULL)
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
					AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId

             
			  WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					 ----------AM and Model wise
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@TC_BrandZoneId,
						@CarModelId =@CarId,
						@TC_AMId =@FieldId,
						@DealerId  =@DealerId,
						@CarVersionId  =NULL,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=NULL,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 





				--------AM and Model wise
				SELECT S.TC_SpecialUsersId  FieldId,S.UserName FieldName,M.Id  CarId,M.Name CarName, ROUND(SUM (Target),0) AS Target
				FROM TC_TMTargetScenarioDetail AS I
				JOIN Dealers           AS D ON I.DealerId=D.Id
				JOIN CarVersions       AS V ON V.Id=I.CarVersionId
				JOIN TC_SpecialUsers   AS S ON S.TC_SpecialUsersId=D.TC_AMId
				JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				JOIN CarModels         AS M ON M.Id=V.CarModelId
				WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				GROUP BY S.TC_SpecialUsersId,S.UserName,M.Id,M.Name	 
				ORDER BY S.TC_SpecialUsersId,M.ID  	      
           
	   END
      ELSE IF (@PageId=3) ---  Dealer Model wise
	   BEGIN
	     
		 
		 
		        ---------Dealer and Model wise 
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	             FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON D.Id=P.FieldId AND P.CarId=M.ID
				 WHERE 
						(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
					AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND (D.ID=@DealerId OR @DealerId IS NULL)
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
					AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId



					WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					 ---------Dealer and Model wise 
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@TC_BrandZoneId,
						@CarModelId =@CarId,
						@TC_AMId =@TC_AMId,
						@DealerId  =@FieldId,
						@CarVersionId  =NULL,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=NULL,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 



		    
				---------Dealer and Model wise 
				SELECT D.Id  FieldId ,D.Organization FieldName ,M.Id  CarId ,M.Name  CarName , ROUND(SUM (Target),0) AS Target
				FROM TC_TMTargetScenarioDetail AS I
				JOIN Dealers           AS D ON I.DealerId=D.Id
				JOIN CarVersions       AS V ON V.Id=I.CarVersionId
				JOIN TC_SpecialUsers   AS S ON S.TC_SpecialUsersId=D.TC_AMId
				JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				JOIN CarModels         AS M ON M.Id=V.CarModelId
				WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				GROUP BY D.Id,D.Organization,M.Id,M.Name
				ORDER BY D.ID,M.ID

	   END 

	    ELSE IF (@PageId=4) --- AM Version wise
	    BEGIN

		      ---------AM and Version wise 
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	             FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON S.TC_SpecialUsersId=P.FieldId AND P.CarId=V.ID
				  WHERE 
						(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
					AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND (D.ID=@DealerId OR @DealerId IS NULL)
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
					AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId



					WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					 ---------AM and Version wise 
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@TC_BrandZoneId,
						@CarModelId =NULL,
						@TC_AMId =@FieldId,
						@DealerId  =@DealerId,
						@CarVersionId  =@CarId,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=NULL,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 




	       ---------AM and Version wise 
			SELECT S.TC_SpecialUsersId FieldId,S.UserName FieldName ,V.Id CarId,M.Name+' '+V.Name CarName, ROUND(SUM (Target),0) AS Target
			FROM TC_TMTargetScenarioDetail AS I
			JOIN Dealers           AS D ON I.DealerId=D.Id
			JOIN CarVersions       AS V ON V.Id=I.CarVersionId
			JOIN TC_SpecialUsers   AS S ON S.TC_SpecialUsersId=D.TC_AMId
			JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
			JOIN CarModels         AS M ON M.Id=V.CarModelId
			WHERE 
				(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
			AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
			AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
			AND (D.ID=@DealerId OR @DealerId IS NULL)
		    AND I.[Month] BETWEEN @StartMonth AND @EndMonth
			AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
			GROUP BY S.TC_SpecialUsersId,S.UserName,V.Id,M.Name+' '+V.Name
			ORDER BY S.TC_SpecialUsersId,V.ID
	   END

	   ELSE IF (@PageId=5) --- Dealer  Version wise
	    BEGIN
				
				 ---------Dealer and Version wise 
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	              FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON D.Id=P.FieldId AND P.CarId=V.ID
				  WHERE 
						(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
					AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND (D.ID=@DealerId OR @DealerId IS NULL)
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
					AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId


					WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					  ---------Dealer and Version wise 
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@TC_BrandZoneId,
						@CarModelId =NULL,
						@TC_AMId =@TC_AMId,
						@DealerId  =@FieldId,
						@CarVersionId  =@CarId,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=NULL,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 


					 ---------Dealer and Version wise 
				SELECT D.Id FieldId ,D.Organization FieldName ,V.Id CarId ,M.Name+' '+V.Name CarName, ROUND(SUM (Target),0) AS Target
				FROM TC_TMTargetScenarioDetail AS I
				JOIN Dealers           AS D ON I.DealerId=D.Id
				JOIN CarVersions       AS V ON V.Id=I.CarVersionId
				JOIN TC_SpecialUsers   AS S ON S.TC_SpecialUsersId=D.TC_AMId
				JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				JOIN CarModels         AS M ON M.Id=V.CarModelId
				WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				GROUP BY D.Id,D.Organization,V.Id,M.Name+' '+V.Name
				ORDER BY D.ID,V.ID

	END

	ELSE IF (@PageId=6) --- Zone Version wise
	    BEGIN
		
		        ------Zone and Version wise
			    UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	              FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=TCB.TC_BrandZoneId AND P.CarId=V.ID
				 WHERE 
						(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
					AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
					AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
					AND (D.ID=@DealerId OR @DealerId IS NULL)
					AND I.[Month] BETWEEN @StartMonth AND @EndMonth
					AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId


					WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					  ------Zone and Version wise
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@FieldId,
						@CarModelId =NULL,
						@TC_AMId =@TC_AMId,
						@DealerId  =@DealerId,
						@CarVersionId  =@CarId,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=NULL,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 

				------Zone and Version wise
				SELECT TCB.TC_BrandZoneId FieldId , TCB.ZoneName FieldName,M.Name+' '+V.Name CarName, V.Id CarId, ROUND(SUM (Target),0) AS Target
				FROM TC_TMTargetScenarioDetail AS I
				JOIN Dealers           AS D ON I.DealerId=D.Id
				JOIN CarVersions       AS V ON V.Id=I.CarVersionId
				JOIN TC_SpecialUsers   AS S ON S.TC_SpecialUsersId=D.TC_AMId
				JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				JOIN CarModels		   AS M ON  M.Id=V.CarModelId
				WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				GROUP BY TCB.ZoneName,TCB.TC_BrandZoneId,M.Name+' '+V.Name,V.Id
				ORDER BY TCB.TC_BrandZoneId,V.ID

      END 

     ELSE IF (@PageId=7)  --Version  Month wise
	    BEGIN
	          		
					
				  	---------Version  and Month wise 
			   UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	             FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=I.[Month] AND P.CarId=V.ID	
				 WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					

					WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					  ---------Version  and Month wise 
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=NULL,
						@CarModelId =NULL,
						@TC_AMId =@TC_AMId,
						@DealerId  =@DealerId,
						@CarVersionId  =@CarId,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=@FieldId,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 


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
							WHEN 12  THEN  'DEC' END FieldName,ROUND(SUM (Target),0) AS Target
				FROM TC_TMTargetScenarioDetail AS I
				JOIN Dealers           AS D ON I.DealerId=D.Id
				JOIN CarVersions       AS V ON V.Id=I.CarVersionId
				JOIN TC_SpecialUsers   AS S ON S.TC_SpecialUsersId=D.TC_AMId
				JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				JOIN CarModels         AS M ON M.Id=V.CarModelId
				WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
				GROUP BY V.Id,M.Name+' '+ V.Name,I.[Month]
				ORDER BY [MONTH],V.ID
	    END 
		 ELSE IF (@PageId=8)  --Model Version wise
	    BEGIN
	          	  	---------Model and Version Wise
			   UPDATE I SET I.Target =  ROUND (I.Target+ (I.Target/100)* P.PercentageChange,0)
	             FROM TC_TMTargetScenarioDetail  I
	             JOIN Dealers           AS D   ON I.DealerId=D.Id
		         JOIN CarVersions       AS V   ON V.Id=I.CarVersionId
		         JOIN CarModels         AS M   ON M.Id=V.CarModelId
				 JOIN TC_SpecialUsers   AS S   ON S.TC_SpecialUsersId=D.TC_AMId
				 JOIN TC_BrandZone      AS TCB ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
				 JOIN @TC_TMPageWisePercentageChange AS P ON P.FieldId=I.CarVersionId AND P.CarId=M.ID	
				 WHERE 
					(D.TC_BrandZoneId=@TC_BrandZoneId OR @TC_BrandZoneId IS NULL)
				AND (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
				AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
				AND (D.ID=@DealerId OR @DealerId IS NULL)
				AND I.[Month] BETWEEN @StartMonth AND @EndMonth
				AND TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId

				WHILE (@WhileLoopControl<=@TotalWhileLoopCount)
				BEGIN 
                     SELECT  @FieldId=FieldId,
					         @CarId=CarId,
							 @ActualTarget=NewValue  
					 FROM @TC_TMPageWisePercentageChange
					 WHERE ID=@WhileLoopControl

					  ---------Model and Version Wise
					 EXEC [dbo].[TC_TMRoundOffHandling] 
					 	@ActualTarget=@ActualTarget ,
						@TC_BrandZoneId=@TC_BrandZoneId,
						@CarModelId =@CarId,
						@TC_AMId =@TC_AMId,
						@DealerId  =@DealerId,
						@CarVersionId  =@FieldId,
						@StartMonth =@StartMonth,
						@EndMonth  =@EndMonth,
						@MonthId=NULL,
						@TC_TMDistributionPatternMasterId=@TC_TMDistributionPatternMasterId
					 
					 SET @WhileLoopControl=@WhileLoopControl+1;
                  END 



	   END 
	END
