IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetUserDashBoardData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetUserDashBoardData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(9th June 2015)
-- Description	:	Get target and achieved data for L3 level executives
-- Modifier	:	Sahin Bharti(29th June 2015)
-- Purpose	:	Added query for Total,Active,Paid and Live listings dealers
-- DataTable 0 - YTD Or MTD target and Metric Name
-- DataTable 1 - User current performance
-- DataTable 2 - All approved warrantity quantities 
-- DataTable 3 - All converted warrantity quantities 
-- DataTable 4 - Under discussion warrantity quantities
-- DataTable 5 - Total dealers assigned to user
-- DataTable 6 - All active dealers assigned to user
-- DataTable 7 - All paid dealers assigned to user
-- DataTable 8 - All live listing dealers assigned to user
-- Execute M_GetUserDashBoardData 1,null,86
-- =============================================
CREATE PROCEDURE [dbo].[M_GetUserDashBoardData]
	
	@IsMTD BIT = NULL,	--month till date
	@OprUserId	INT = NULL ,--null when getting data for managers
	@ReportingUserId INT = NULL ,--managers user id
	@Type	SMALLINT --used to identify which metric to use

AS
	BEGIN

		--declare table type to store reporting users
		DECLARE @TempTable AS TABLE (UserId INT)

		--get all direct reporting users
		IF @OprUserId IS NULL AND @ReportingUserId IS NOT NULL
			BEGIN
				INSERT INTO @TempTable (UserId) 
					SELECT MU.OprUserId FROM DCRM_ADM_MappedUsers MU(NOLOCK) 
							WHERE MU.NodeRec.GetAncestor(1) = (SELECT NodeRec FROM DCRM_ADM_MappedUsers WHERE OprUserId = @ReportingUserId)
				--now insert reporting user also in the temp table
				INSERT INTO @TempTable (UserId) VALUES(@ReportingUserId)
			END
		--if no reporting user exist then insert current user
		ELSE IF @OprUserId IS NOT NULL AND @ReportingUserId IS NULL
			BEGIN
				INSERT INTO @TempTable (UserId) VALUES(@OprUserId)
			END
		
		IF @Type = 1 OR @Type = 0
			BEGIN
				--YTD Or MTD target and Metric Name
				SELECT 
				(	
					SELECT 
						SUM(ET.UserTarget) 
					FROM 
						DCRM_FieldExecutivesTarget ET 
					WHERE 
						ET.OprUserId IN (SELECT UsersId from [dbo].[Fn_DCRM_GetChildUsersIncludingParent](ET1.OprUserId))
						AND ET.MetricId = 1 -- for car warranties
						AND ET.TargetYear = YEAR(GETDATE())
						AND (@IsMTD IS NULL OR ET.TargetMonth = MONTH(GETDATE()))
				) AS UserTarget,
					DM.MetricName,
					ET1.OprUserId
				FROM
					DCRM_FieldExecutivesTarget ET1(NOLOCK)
					INNER JOIN DCRM_ExecScoreBoardMetric DM(NOLOCK) ON DM.Id = ET1.MetricId
				WHERE
					ET1.OprUserId IN (SELECT UserId FROM @TempTable)
					AND ET1.MetricId = 1 -- for car warranties
					AND ET1.TargetYear = YEAR(GETDATE())
				GROUP BY
					DM.MetricName, ET1.OprUserId

				--current performance
				SELECT 
					[dbo].RoundUp((
						SELECT 
							CAST(ISNULL(SUM(DSD.Quantity),0) AS FLOAT)
						FROM 
							DCRM_SalesDealer DSD(NOLOCK)
							INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId AND DSD.UpdatedBy = MU.OprUserId AND DPD.IsApproved = 1
							INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37
						WHERE
							(@IsMTD IS NULL OR MONTH(DPD.ReceivedDate) = MONTH(GETDATE())) AND
							YEAR(DPD.ReceivedDate) = YEAR(GETDATE())	
					)*100/
					SUM((ISNULL(CASE WHEN ET.UserTarget = 0 THEN 1 ELSE ET.UserTarget END,1))),2) AS Performance,
					MU.OprUserId
				FROM 
					DCRM_FieldExecutivesTarget ET(NOLOCK)
					INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = ET.OprUserId AND MU.IsActive = 1
				WHERE
					ET.MetricId = 1 AND
					(@IsMTD IS NULL OR ET.TargetMonth = MONTH(GETDATE())) AND
					ET.TargetYear = YEAR(GETDATE())	AND
					MU.OprUserId = @OprUserId
				GROUP BY
					MU.OprUserId

				--approved quantity 
				SELECT 
					(	SELECT 
						ISNULL(SUM(DSD.Quantity),0)
					FROM 
						DCRM_SalesDealer DSD(NOLOCK) 
						INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId AND ISNULL(IsApproved,0) =1 --approved payments
						INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37 -- for warranties only
					WHERE
						(@IsMTD IS NULL OR MONTH(DPD.ReceivedDate) = MONTH(GETDATE()))
						AND YEAR(DPD.ReceivedDate) = YEAR(GETDATE())
						AND DSD.LeadStatus = 2		--closed products
						AND DSD.CampaignType = 3	-- paid campaigns only 
						AND DSD.UpdatedBy IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](DSD1.UpdatedBy)))AS Approved,
						DSD1.UpdatedBy AS OprUserId
				FROM 
					DCRM_SalesDealer DSD1(NOLOCK) 
				WHERE
					DSD1.LeadStatus = 2	AND--closed products
					DSD1.CampaignType = 3 AND-- paid campaigns only 
					DSD1.UpdatedBy IN (SELECT UserId FROM @TempTable)
				GROUP BY 
					DSD1.UpdatedBy

				--converted quantity in the current month
				SELECT 
					(	SELECT 
						ISNULL(SUM(DSD.Quantity),0)
					FROM 
						DCRM_SalesDealer DSD(NOLOCK) 
						INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId AND ISNULL(IsApproved,0) =0 --not approved payments
						INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37 -- for warranties only
					WHERE
						(@IsMTD IS NULL OR  MONTH(DPD.AddedOn) = MONTH(GETDATE()))
						AND YEAR(DPD.AddedOn) = YEAR(GETDATE())
						AND DSD.LeadStatus = 2		--closed products
						AND DSD.CampaignType = 3	-- paid campaigns only 
						AND DSD.UpdatedBy IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](DSD1.UpdatedBy)))AS Converted,
						DSD1.UpdatedBy AS OprUserId
				FROM 
					DCRM_SalesDealer DSD1(NOLOCK) 
				WHERE
					DSD1.LeadStatus = 2 AND--closed products
					DSD1.CampaignType = 3 AND-- paid campaigns only 
					DSD1.UpdatedBy IN (SELECT UserId FROM @TempTable)
				GROUP BY 
					DSD1.UpdatedBy

				--in discussion of ie those packages are in open stage
				SELECT 
				(	SELECT 
						ISNULL(SUM(DSD.Quantity),0) 
					FROM 
						DCRM_SalesDealer DSD(NOLOCK) 
						INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37-- for warranties only
						INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId AND DSD.LeadStatus = 1 -- in open stage
					WHERE
						DSD.UpdatedBy IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](DSD1.UpdatedBy))
						AND YEAR(DSD.CreatedOn) = YEAR(GETDATE())
				) AS Discussion,
					DSD1.UpdatedBy AS OprUserId
				FROM 
					DCRM_SalesDealer DSD1(NOLOCK) 
				WHERE
					DSD1.UpdatedBy IN (SELECT UserId FROM @TempTable)
					AND YEAR(DSD1.CreatedOn) = YEAR(GETDATE())
				GROUP BY 
					DSD1.UpdatedBy

			END
		
		IF @Type = 2 OR @Type = 0
			BEGIN
				--Total Dealers assigned to user
				SELECT 
					(
						SELECT 
							COUNT(DISTINCT DAU.DealerId)
						FROM	
							DCRM_ADM_UserDealers DAU(NOLOCK)
						WHERE
							DAU.UserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
					) AS Dealers,
					(
						SELECT 
							SUM(ISNULL(FET.UserTarget,0))
						FROM 
							DCRM_FieldExecutivesTarget FET(NOLOCK) 
						WHERE 
							FET.MetricId = 2 AND 
							FET.TargetMonth = MONTH(GETDATE()) AND
							FET.OprUserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
					)AS Targets,
					AMU.OprUserId
			
				FROM
					DCRM_ADM_UserDealers DAU(NOLOCK)
					INNER JOIN DCRM_ADM_MappedUsers AMU(NOLOCK) ON DAU.UserId = AMU.OprUserId AND AMU.IsActive = 1
				WHERE
					AMU.OprUserId IN (SELECT UserId FROM @TempTable)
				GROUP BY
					AMU.OprUserId

		
				--Get all active dealers count
				SELECT  
				(
					SELECT 	
						ISNULL(COUNT(A.DealerId),0)
					FROM 
						(	
							SELECT 
								DISTINCT DAU.DealerId 
							FROM 
								AbSure_Trans_Credits TC(NOLOCK) 
								INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON TC.DealerId = DAU.DealerId
								AND DAU.UserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
							WHERE
								TC.CreditAmount > 0

							UNION -- removes duplicate rows

							SELECT 
								DISTINCT DAU.DealerId
							FROM 
								ConsumerCreditPoints CCP(NOLOCK)
								INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.DealerId = CCP.ConsumerId AND DATEDIFF(DAY,GETDATE(),CCP.ExpiryDate) > 0 --not expired yet
																											 AND DAU.UserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))		
								INNER JOIN SellInquiries SI(NOLOCK) ON SI.DealerId = DAU.DealerId AND DATEDIFF(DAY,SI.LastUpdated,GETDATE()) BETWEEN 0 AND 30 --stock updated in last 30 days
							WHERE
								CCP.ConsumerType = 1 AND--For Dealer
								DATEDIFF(DAY,CONVERT(VARCHAR(20),GETDATE(),106),CONVERT(VARCHAR(20),CCP.ExpiryDate,106)) > 0
						)AS A
				)AS Dealers,
				(	SELECT 
						ISNULL(SUM(FET.UserTarget),0)
					FROM 
						DCRM_FieldExecutivesTarget FET(NOLOCK) 
					WHERE 
						FET.MetricId = 3 AND --get active dealers count 
						(@IsMTD IS NULL OR FET.TargetMonth = MONTH(GETDATE())) AND
						FET.OprUserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
				)AS Targets,
				AMU.OprUserId
				FROM	
					DCRM_ADM_UserDealers DAU(NOLOCK) 
					INNER JOIN DCRM_ADM_MappedUsers AMU(NOLOCK) ON DAU.UserId = AMU.OprUserId AND AMU.IsActive = 1
				WHERE
					AMU.OprUserId IN (SELECT UserId FROM @TempTable)
				GROUP BY
					AMU.OprUserId

				--Get all paid dealers count
				SELECT  
				(
					SELECT 	
						ISNULL(COUNT( DISTINCT A.DealerId),0)
					FROM 
						(	
							SELECT 
								DISTINCT DAU.DealerId 
							FROM 
								AbSure_Trans_Credits TC(NOLOCK) 
								INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON TC.DealerId = DAU.DealerId
									AND DAU.UserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
							WHERE
								TC.CreditAmount > 0

							UNION -- removes duplicate rows

							SELECT 
								DISTINCT DAU.DealerId
							FROM 
								ConsumerCreditPoints CCP(NOLOCK)
								INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON	DAU.DealerId = CCP.ConsumerId 
																				AND DATEDIFF(DAY,GETDATE(),CCP.ExpiryDate) > 0 --not expired yet
																				AND CCP.PackageType <> 28 --excluding free listings
																				AND DAU.UserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
							WHERE
								CCP.ConsumerType = 1 AND--For Dealer
								DATEDIFF(DAY,CONVERT(VARCHAR(20),GETDATE(),106),CONVERT(VARCHAR(20),CCP.ExpiryDate,106)) > 0
						)AS A
				)AS Dealers,
				(	SELECT 
						ISNULL(SUM(FET.UserTarget),0)
					FROM 
						DCRM_FieldExecutivesTarget FET(NOLOCK) 
					WHERE 
						FET.MetricId = 4 AND --get active dealers target
						(@IsMTD IS NULL OR FET.TargetMonth = MONTH(GETDATE())) AND
						FET.OprUserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
				)AS Targets,
				AMU.OprUserId
				FROM	
					DCRM_ADM_UserDealers DAU(NOLOCK) 
					INNER JOIN DCRM_ADM_MappedUsers AMU(NOLOCK) ON DAU.UserId = AMU.OprUserId AND AMU.IsActive = 1
				WHERE
					AMU.OprUserId IN (SELECT UserId FROM @TempTable)
				GROUP BY
					AMU.OprUserId


				--get all live listing dealers
				SELECT 
					( 
						SELECT 
							COUNT(DISTINCT SI.DealerId)
						FROM
							Livelistings LI(NOLOCK)
							INNER JOIN SellInquiries SI(NOLOCK) ON LI.Inquiryid = SI.ID AND LI.SellerType = 1
							INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.DealerId = SI.DealerId
																		AND DAU.UserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
					)AS Dealers,
					(
						SELECT 
							ISNULL(SUM(FET.UserTarget),0)
						FROM 
							DCRM_FieldExecutivesTarget FET(NOLOCK) 
						WHERE 
							FET.MetricId = 5 AND --live listing dealers target
							(@IsMTD IS NULL OR FET.TargetMonth = MONTH(GETDATE())) AND
							FET.OprUserId IN (SELECT UsersId FROM [dbo].[Fn_DCRM_GetChildUsersIncludingParent](AMU.OprUserId))
					)AS Targets,
					AMU.OprUserId
				FROM
					DCRM_ADM_UserDealers DAU(NOLOCK)
					INNER JOIN DCRM_ADM_MappedUsers AMU(NOLOCK) ON DAU.UserId = AMU.OprUserId AND AMU.IsActive = 1
				WHERE
					AMU.OprUserId IN (SELECT UserId FROM @TempTable)
				GROUP BY
					AMU.OprUserId
			END
	END

