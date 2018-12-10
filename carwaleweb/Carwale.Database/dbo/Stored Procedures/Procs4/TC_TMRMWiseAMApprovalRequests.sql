IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMRMWiseAMApprovalRequests]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMRMWiseAMApprovalRequests]
GO

	

-- =============================================
-- Author:		Vishal Srivastava AE 1830
-- Create date: 26 december 2013 1534 HRS IST
-- Description:	This SP takes the data which for approval 
--				when RM logins
-- EXEC TC_TMRMApprovalRequests 13, 2014, 1,12
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMRMWiseAMApprovalRequests]
	@TC_RMId INT ,
	@Year SMALLINT ,
	@StartMonth TINYINT,
	@EndMonth TINYINT 
				
AS
BEGIN
	DECLARE  @ListOfAmSendForApproval TABLE  (TC_AMId INT)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- select statements for procedure here
	
		INSERT INTO @ListOfAmSendForApproval  (TC_AMId)
			SELECT DISTINCT D.TC_AMId 
			FROM   Dealers AS D WITH(NOLOCK)
			LEFT JOIN TC_TMAMTargetChangeApprovalReq AS TCAR WITH(NOLOCK) ON D.TC_AMId=TCAR.TC_AMId
			LEFT JOIN TC_TMAMTargetChangeMaster AS TCM WITH (NOLOCK) ON TCM.TC_AMId=TCAR.TC_AMId AND  TCM.[Year]=@Year AND TCM.IsActive=1 AND TCM.IsAprrovedByRM IS  NULL
			WHERE D.TC_BrandZoneId IS NOT NULL
			AND D.IsDealerActive=1
			AND TCM.TC_AMId IS NULL
			AND D.TC_RMID=@TC_RMId

			
                SELECT S.TC_SpecialUsersId  FieldId ,S.UserName FieldName ,M.Id  CarId ,M.Name  CarName , 
                        ROUND(SUM(TCA.Target),0) AS Target,
                        ROUND( SUM(TM.Target),0) AS FinalTarget,
                        ROUND( SUM(TCA.Target),0)-ROUND( SUM(TM.Target),0) AS Difference,
						1   AS IsSendForApproval
                FROM TC_TMAMTargetChangeApprovalReq TCA  WITH (NOLOCK) 
                        JOIN  TC_DealersTarget   AS TM  WITH (NOLOCK) ON (TCA.TC_DealersTargetId=TM.TC_DealersTargetId)
						JOIN Dealers           AS D  WITH (NOLOCK) ON TCA.DealerId=D.Id
						JOIN  TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
                        JOIN CarVersions       AS V WITH (NOLOCK)  ON V.Id=TCA.CarVersionId
                        JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
					    JOIN TC_TMAMTargetChangeMaster TCM ON TCM.TC_AMId = TCA.TC_AMId 
				WHERE    TCA.[Month] BETWEEN @StartMonth AND @EndMonth
                        AND TCA.TC_TargetTypeId=4
                        AND TCA.[Year]=@Year
						AND D.TC_RMID=@TC_RMId
						AND TCM.[Year]=@Year
						AND TCM.IsActive=1
						AND TCM.IsAprrovedByRM IS NULL
			GROUP BY S.TC_SpecialUsersId,M.Id,S.UserName,M.Name
             
UNION ALL
		
         SELECT S.TC_SpecialUsersId  FieldId ,S.UserName FieldName ,M.Id  CarId ,M.Name  CarName , 
                        ROUND(SUM(TCA.Target),0) AS Target,
                        ROUND(SUM(TCA.Target),0) AS FinalTarget,
                        0 AS Difference,
						0 IsSendForApproval
						 FROM TC_DealersTarget TCA  WITH (NOLOCK) 
                       JOIN Dealers           AS D  WITH (NOLOCK) ON TCA.DealerId=D.Id
						JOIN  TC_SpecialUsers   AS S  WITH (NOLOCK) ON S.TC_SpecialUsersId=D.TC_AMId
                        JOIN CarVersions       AS V WITH (NOLOCK)  ON V.Id=TCA.CarVersionId
                        JOIN CarModels         AS M  WITH (NOLOCK) ON M.Id=V.CarModelId
						JOIN @ListOfAmSendForApproval AS L  ON D.TC_AMId=L.TC_AMId
                WHERE   TCA.[Month] BETWEEN @StartMonth AND @EndMonth
                        AND TCA.TC_TargetTypeId=4
                        AND TCA.[Year]=@Year
		        GROUP BY S.TC_SpecialUsersId,S.UserName,M.Id,M.Name

END



