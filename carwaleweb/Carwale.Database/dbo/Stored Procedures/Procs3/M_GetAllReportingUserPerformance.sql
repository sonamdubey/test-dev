IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetAllReportingUserPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetAllReportingUserPerformance]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(11th June 2015)
-- Description	:	Get all reporting users and their performance
-- execute [dbo].[M_GetAllReportingUserPerformance] 86,1
-- =============================================
CREATE PROCEDURE [dbo].[M_GetAllReportingUserPerformance]
	
	@OprUserId	INT,
	@IsMTD BIT = NULL

AS
	BEGIN

		DECLARE @MetricId SMALLINT 
		DECLARE @MetricName VARCHAR(50)

		--declare table type to store reporting users
		DECLARE @TempTable AS TABLE (UserId INT) 
				
		--get all reporting users
		INSERT INTO @TempTable (UserId) 
			SELECT MU.OprUserId FROM DCRM_ADM_MappedUsers MU(NOLOCK) 
					WHERE MU.NodeRec.GetAncestor(1) = (SELECT NodeRec FROM DCRM_ADM_MappedUsers WHERE OprUserId = @OprUserId AND IsActive = 1)

		--Get Metric Id
		SELECT  
			@MetricId = ET.Id,
			@MetricName = ET.MetricName
		FROM 
			DCRM_ExecScoreBoardMetric ET(NOLOCK)
		WHERE
			ET.Id = 1

		--Get reporting users and their performance
		SELECT 
			OU.Id AS UserId,
			OU.UserName AS UserName,
			@MetricName AS MetricName,
			[dbo].RoundUp((((SELECT
				CAST(ISNULL(SUM(DSD.Quantity),0) AS FLOAT)
					FROM
						DCRM_SalesDealer DSD(NOLOCK) 
						INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37 AND DSD.LeadStatus = 2 AND DSD.CampaignType = 3
						INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId AND DPD.IsApproved = 1 
					WHERE
						(@IsMTD IS NULL OR MONTH(DPD.ReceivedDate) = MONTH(GETDATE())) AND
						YEAR(DPD.ReceivedDate) = YEAR(GETDATE())AND
						DSD.UpdatedBy IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](OU.Id)
						)
				)*100/
				(	SELECT ISNULL(SUM(CASE WHEN ET1.UserTarget = 0 THEN 1 ELSE ET1.UserTarget END),1)
					FROM DCRM_FieldExecutivesTarget ET1 
					WHERE 
						ET1.OprUserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](OU.Id)) AND 
						ET1.MetricId = 1 AND
						(@IsMTD IS NULL OR ET1.TargetMonth = MONTH(GETDATE())) AND
						(ET1.TargetYear = YEAR(GETDATE()))
				))),2) AS Performance
		FROM
			DCRM_ADM_MappedUsers MU(NOLOCK)
			INNER JOIN OprUsers OU(NOLOCK) ON  OU.Id = MU.OprUserId AND MU.IsActive = 1
			LEFT JOIN DCRM_FieldExecutivesTarget ET(NOLOCK) ON ET.OprUserId = MU.OprUserId	AND ET.MetricId = 1 
																							AND ET.TargetYear = YEAR(GETDATE())	
																							AND (@IsMTD IS NULL OR ET.TargetMonth = MONTH(GETDATE()))
		WHERE
			MU.OprUserId IN (SELECT UserId FROM @TempTable)
		GROUP BY
			OU.Id,OU.UserName,MU.OprUserId
		ORDER BY 
			UserName 

	END

