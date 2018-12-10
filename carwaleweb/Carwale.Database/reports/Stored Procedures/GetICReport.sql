IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetICReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetICReport]
GO

	
-- =============================================
-- Author:		Reshma Shetty
-- Create date: 24/04/2013
-- Description:	Returns the number of fresh calls and backlog calls made
-- =============================================
CREATE PROCEDURE [Reports].[GetICReport] 
	-- Add the parameters for the stored procedure here
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT OU.UserName AS Executive,
						CF.FollowupAction AS Status,
			COUNT(DISTINCT CL.ID) AS [Total Calls],
			COUNT(DISTINCT CASE 
					WHEN DATEDIFF(DAY, CC.EntryDateTime, CL.CalledDateTime) > 0
						THEN CL.ID
					END) AS [BackLog Calls],
			COUNT(DISTINCT CASE 
					WHEN DATEDIFF(DAY, CC.EntryDateTime, CL.CalledDateTime) = 0
						THEN CL.ID
					END) AS [Fresh Calls]
		FROM CH_Calls CC WITH (NOLOCK)
		INNER JOIN CH_Logs CL WITH (NOLOCK) ON CL.CallId = CC.ID
		INNER JOIN OprUsers OU WITH (NOLOCK) ON OU.Id = CL.TCID
		INNER JOIN CH_FollowupActions CF WITH (NOLOCK) ON CF.Id = CL.ActionId
		WHERE CC.TBCType = 2
			AND CC.CallType IN (
				1,
				7
				)
			AND CONVERT(DATE, Cl.CalledDateTime) BETWEEN @FromDate
				AND @ToDate 
		GROUP BY OU.UserName,
						CF.FollowupAction
	

END

