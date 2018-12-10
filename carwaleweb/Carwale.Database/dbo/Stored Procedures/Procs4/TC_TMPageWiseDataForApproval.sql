IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMPageWiseDataForApproval]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMPageWiseDataForApproval]
GO

	-- =============================================
-- Author	    :	Vishal Srivastava
-- Create date	:	18-11-2013
-- Description	:	Get Pagewise details for Target Management 	
---PAGE ID =0   Month Model wise
---PAGE ID =1   Zone  Model wise
---PAGE ID =2   AM Model wise
---PAGE ID =3   Dealer Model wise
---PAGE ID =4   AM Version wise
---PAGE ID =5   Dealer Version wise
---PAGE ID =6   Zone Version wise
---PAGE ID =7   Version  Month wise
---	EXEC  [dbo].[TC_TMPageWiseDataForApproval] @TC_BrandZoneId=
--- Modified by Vinayak Patil on 22-11-2013. If Area Manager have changed the target data
--- then fetch that data from TC_TMAMTargetChangeMaster and show the changes made in target data 
--Edited By Deepak on 8th December 2013
-- Modified By  :	Vinayak Patil on 11-12-13 Added SELECT statement to return AMComments
-- Modified By  :	Nilesh Utture on 12-12-13, Added year parameter to check wheteher is sent for approval
-- Modified By  :   Nilesh Utture on 17th Dec, 2013 done changes to load activity feed
 -- =============================================
CREATE PROCEDURE [dbo].[TC_TMPageWiseDataForApproval] 
	@PageId TINYINT,
	@CarModelId INT=NULL,
	@TC_AMId INT=NULL,
	@DealerId INT =NULL,
	@StartMonth TINYINT=NULL,
	@EndMonth TINYINT =NULL,
	@Year SMALLINT =NULL
	--,@IsSentForApproval BIT =0 OUTPUT
	
AS
	BEGIN
		-- TO see if data exists for particular AM for that year in intermediate table
		-- Rest of the code remains the same
		IF  EXISTS (SELECT TOP 1 TC_DealersTargetId FROM   [TC_TMAMTargetChangeApprovalReq] WITH (NOLOCK) WHERE  TC_AMId = @TC_AMId AND [Year] = @Year)
			BEGIN
				IF (@PageId=0)  --Month Model wise
					BEGIN
						SELECT   
							CM.ID    CarId,
							CM.Name  CarName, 
							CASE TCA.Month   WHEN 1  THEN  'JAN'
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
							TCA.Month FieldId,
							ROUND( SUM(TCA.Target),0) AS Target,
							ROUND( SUM(TM.Target),0) AS FinalTarget,
							ROUND( SUM(TCA.Target),0)-ROUND( SUM(TM.Target),0) AS Difference 
				
						FROM TC_TMAMTargetChangeApprovalReq TCA  WITH (NOLOCK)
							JOIN  TC_DealersTarget   AS TM  WITH (NOLOCK) ON (TCA.TC_DealersTargetId=TM.TC_DealersTargetId)
							JOIN  Dealers AS D WITH (NOLOCK) ON D.ID=TCA.DealerId
							JOIN  CarVersions AS CV WITH (NOLOCK) ON CV.ID=TCA.CarVersionId
							JOIN  CarModels AS CM WITH (NOLOCK) ON CV.CarModelId=CM.ID
						WHERE (CM.Id=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TCA.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TCA.TC_TargetTypeId=4
							AND TCA.[Year]=@Year
						GROUP BY  CM.Name,TCA.[Month],CM.ID
						ORDER BY TCA.[Month],CM.ID
					END
				ELSE IF (@PageId=3) ---  Dealer Model wise
					BEGIN
						SELECT D.Id  FieldId ,D.Organization FieldName ,M.Id  CarId ,M.Name  CarName , 
							ROUND(SUM(TCA.Target),0) AS Target,
							ROUND( SUM(TM.Target),0) AS FinalTarget,
							ROUND( SUM(TCA.Target),0)-ROUND( SUM(TM.Target),0) AS Difference
						FROM TC_TMAMTargetChangeApprovalReq TCA  WITH (NOLOCK) 
							JOIN  TC_DealersTarget   AS TM  WITH (NOLOCK) ON (TCA.TC_DealersTargetId=TM.TC_DealersTargetId)
							JOIN Dealers           AS D  WITH (NOLOCK) ON TCA.DealerId=D.Id
							JOIN CarVersions       AS V WITH (NOLOCK)  ON V.Id=TCA.CarVersionId
							JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TCA.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TCA.TC_TargetTypeId=4
							AND TCA.[Year]=@Year
						GROUP BY D.Id,D.Organization,M.Id,M.Name
						ORDER BY D.ID,M.ID

					END 
				ELSE IF (@PageId=5) --- Dealer  Version wise
					BEGIN
						SELECT D.Id FieldId ,D.Organization FieldName ,V.Id CarId ,M.Name+' '+V.Name CarName, 
							ROUND(SUM(TCA.Target),0) AS Target,
							ROUND( SUM(TM.Target),0) AS FinalTarget,
							ROUND( SUM(TCA.Target),0)-ROUND( SUM(TM.Target),0) AS Difference
						FROM TC_TMAMTargetChangeApprovalReq TCA  WITH (NOLOCK) 
							JOIN  TC_DealersTarget   AS TM  WITH (NOLOCK) ON (TCA.TC_DealersTargetId=TM.TC_DealersTargetId)
							JOIN Dealers           AS D  WITH (NOLOCK) ON TCA.DealerId=D.Id
							JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=TCA.CarVersionId
							JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TCA.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TCA.TC_TargetTypeId=4
							AND TCA.[Year]=@Year
						GROUP BY D.Id,D.Organization,V.Id,M.Name+' '+V.Name
						ORDER BY D.ID,V.ID
					END
				ELSE IF (@PageId=7)  --Version  Month wise
					BEGIN
						SELECT V.Id CarId ,M.Name+' '+ V.Name CarName,TCA.[Month] FieldId, 
							CASE TCA.[MONTH]   WHEN 1  THEN  'JAN'
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
								WHEN 12  THEN  'DEC' END FieldName,ROUND(SUM(TCA.Target),0) AS Target,
								ROUND( SUM(TM.Target),0) AS FinalTarget,
								ROUND( SUM(TCA.Target),0)-ROUND( SUM(TM.Target),0) AS Difference
						FROM TC_TMAMTargetChangeApprovalReq TCA  WITH (NOLOCK) 
							JOIN  TC_DealersTarget   AS TM  WITH (NOLOCK)  ON (TCA.TC_DealersTargetId=TM.TC_DealersTargetId)
							JOIN Dealers           AS D  WITH (NOLOCK)  ON TCA.DealerId=D.Id
							JOIN CarVersions       AS V  WITH (NOLOCK)  ON V.Id=TCA.CarVersionId
							JOIN TC_SpecialUsers   AS S  WITH (NOLOCK)  ON S.TC_SpecialUsersId=D.TC_AMId
							JOIN TC_BrandZone      AS TCB WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
							JOIN CarModels         AS M  WITH (NOLOCK)  ON M.Id=V.CarModelId
						WHERE (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TCA.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TCA.TC_TargetTypeId=4
							AND TCA.[Year]=@Year
						GROUP BY V.Id,M.Name+' '+ V.Name,TCA.[Month]
						ORDER BY TCA.[MONTH],V.ID
					END 
			END
		ELSE
			-- IF no changes has been initiated yet then feth data from final target TC_DealersTarget----------------------------
			BEGIN
				IF (@PageId=0)  --Month Model wise
					BEGIN
						SELECT   
							CM.ID    CarId,
							CM.Name  CarName, 
							CASE TM.Month   WHEN 1  THEN  'JAN'
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
							TM.Month FieldId,
							ROUND( SUM(TM.Target),0) AS Target,
							ROUND( SUM(TM.Target),0) AS FinalTarget,
							0 AS Difference 
				
						FROM TC_DealersTarget   AS TM  WITH (NOLOCK)
							JOIN  Dealers AS D WITH (NOLOCK) ON D.ID=TM.DealerId
							JOIN  CarVersions AS CV WITH (NOLOCK) ON CV.ID=TM.CarVersionId
							JOIN  CarModels AS CM WITH (NOLOCK) ON CV.CarModelId=CM.ID
						WHERE (CM.Id=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TM.TC_TargetTypeId=4
							AND TM.[Year]=@Year
						GROUP BY  CM.Name,TM.[Month],CM.ID
						ORDER BY TM.[Month],CM.ID
					END
				ELSE IF (@PageId=3) ---  Dealer Model wise
					BEGIN
						SELECT D.Id  FieldId ,D.Organization FieldName ,M.Id  CarId ,M.Name  CarName , 
							ROUND(SUM(TM.Target),0) AS Target,
							ROUND( SUM(TM.Target),0) AS FinalTarget,
							0 AS Difference
						FROM TC_DealersTarget   AS TM  WITH (NOLOCK) 
							JOIN Dealers           AS D  WITH (NOLOCK) ON TM.DealerId=D.Id
							JOIN CarVersions       AS V WITH (NOLOCK)  ON V.Id=TM.CarVersionId
							JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TM.TC_TargetTypeId=4
							AND TM.[Year]=@Year
						GROUP BY D.Id,D.Organization,M.Id,M.Name
						ORDER BY D.ID,M.ID

					END 
				ELSE IF (@PageId=5) --- Dealer  Version wise
					BEGIN
						SELECT D.Id FieldId ,D.Organization FieldName ,V.Id CarId ,M.Name+' '+V.Name CarName, 
							ROUND(SUM(TM.Target),0) AS Target,
							ROUND( SUM(TM.Target),0) AS FinalTarget,
							0 AS Difference
						FROM TC_DealersTarget   AS TM  WITH (NOLOCK)
							JOIN Dealers           AS D  WITH (NOLOCK) ON TM.DealerId=D.Id
							JOIN CarVersions       AS V  WITH (NOLOCK) ON V.Id=TM.CarVersionId
							JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						WHERE (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TM.TC_TargetTypeId=4
							AND TM.[Year]=@Year
						GROUP BY D.Id,D.Organization,V.Id,M.Name+' '+V.Name
						ORDER BY D.ID,V.ID
					END
				ELSE IF (@PageId=7)  --Version  Month wise
					BEGIN
						SELECT V.Id CarId ,M.Name+' '+ V.Name CarName,TM.[Month] FieldId, 
							CASE TM.[MONTH]   WHEN 1  THEN  'JAN'
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
								WHEN 12  THEN  'DEC' END FieldName,ROUND(SUM(TM.Target),0) AS Target,
								ROUND( SUM(TM.Target),0) AS FinalTarget,
								0 AS Difference
						FROM TC_DealersTarget   AS TM  WITH (NOLOCK) 
							JOIN Dealers           AS D  WITH (NOLOCK)  ON TM.DealerId=D.Id
							JOIN CarVersions       AS V  WITH (NOLOCK)  ON V.Id=TM.CarVersionId
							JOIN TC_SpecialUsers   AS S  WITH (NOLOCK)  ON S.TC_SpecialUsersId=D.TC_AMId
							JOIN TC_BrandZone      AS TCB WITH (NOLOCK) ON TCB.TC_BrandZoneId=D.TC_BrandZoneId
							JOIN CarModels         AS M  WITH (NOLOCK)  ON M.Id=V.CarModelId
						WHERE (V.CarModelId=@CarModelId OR @CarModelId IS NULL)
							AND (D.TC_AMId=@TC_AMId  OR @TC_AMId IS NULL)
							AND (D.ID=@DealerId OR @DealerId IS NULL)
							AND TM.[Month] BETWEEN @StartMonth AND @EndMonth
							AND TM.TC_TargetTypeId=4
							AND TM.[Year]=@Year
						GROUP BY V.Id,M.Name+' '+ V.Name,TM.[Month]
						ORDER BY TM.[MONTH],V.ID
					END 
			END
			-- Returns whether updated revised targets are sent for approval .

			-- Modified By  :	Nilesh Utture on 12-12-13
			SELECT TOP 1 TC_AMId FROM TC_TMAMTargetChangeMaster WITH(NOLOCK)
			WHERE TC_AMId = @TC_AMId
			AND IsActive = 1
			AND [Year] = @Year

			-- Modified By  :	Vinayak Patil on 11-12-13
			-- Modified By  :   Nilesh Utture on 17th Dec, 2013
			SELECT  SAM.UserName + ' (AM)'		AS AMName,
					CM.SentForApprovalDate		AS AMDate,  
					ISNULL(CM.AMComments,'-')	AS AMComments, 
					SRM.UserName + ' (RM)'		AS RMName, 
					CM.RMActionDate				AS RMDate,
					CM.IsAprrovedByRM			AS RMStatus,
					ISNULL(CM.RMComments,'-')	AS RMComments, 
					SNSC.UserName + ' (NSC)'	AS NSCName,  
					CM.NSCActionDate			AS NSCDate,
					CM.IsAprrovedByNSC			AS NSCStatus,
					ISNULL(CM.NSCComments,'-')	AS NSCComments
			FROM TC_TMAMTargetChangeMaster CM WITH(NOLOCK)
			JOIN TC_SpecialUsers SAM WITH(NOLOCK) ON SAM.TC_SpecialUsersId = CM.TC_AMId
			LEFT JOIN TC_SpecialUsers SRM WITH(NOLOCK) ON SRM.TC_SpecialUsersId = CM.RMId
			LEFT JOIN TC_SpecialUsers SNSC WITH(NOLOCK) ON SNSC.TC_SpecialUsersId = CM.NSCId
			WHERE CM.TC_AMId = @TC_AMId
			AND CM.[Year] = @Year
			ORDER BY CM.SentForApprovalDate DESC
			--AND CM.IsActive = 1
END








/****** Object:  StoredProcedure [dbo].[TC_TMNSCApprovalButton]    Script Date: 12/18/2013 2:34:32 PM ******/
SET ANSI_NULLS ON
