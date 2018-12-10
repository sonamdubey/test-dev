IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ExecMeetingsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ExecMeetingsCount]
GO

	-- =============================================
-- Author	:	Sachin Bharti(25th March 2014)
-- Description	:	Get total field visit meeting count based on executive 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_ExecMeetingsCount] 
	
	@Month		INT,
	@Year		INT,
	@UserId		INT = NULL,
	@StateId	INT = NULL,
	@CityId		INT = NULL,
	@DealerId	NUMERIC(18,0) = NULL,
	@MeetingMode	SMALLINT = NULL

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT OU.UserName,OU.Id AS ExecId,COUNT(DSM.Id)AS MeetingCount,
	DAY(DSM.MeetingDate) As MeetingDay,DSM.DealerType 
	FROM DCRM_SalesMeeting DSM(NOLOCK) 
	INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DSM.ActionTakenBy 
	INNER JOIN Dealers D (NOLOCK) ON D.ID = DSM.DealerId AND DSM.DealerType =1
	INNER JOIN Cities C ON C.ID = D.CityId
	WHERE 
	MONTH(DSM.MeetingDate) = @Month AND 
	YEAR(DSM.MeetingDate) = @Year	AND 
	(@UserId	IS NULL OR OU.Id = @UserId) AND
	(@StateId	IS NULL OR D.StateId = @StateId) AND
	(@CityId	IS NULL OR D.CityId = @CityId) AND
	(@DealerId	IS NULL OR D.ID = @DealerId) AND
	(@MeetingMode IS NULL OR DSM.MeetingMode = @MeetingMode)
	GROUP BY DAY(DSM.MeetingDate),OU.UserName,OU.Id,DSM.DealerType 
	
	UNION ALL

	SELECT OU.UserName,OU.Id AS ExecId,COUNT(DSM.Id)AS MeetingCount,
	DAY(DSM.MeetingDate) As MeetingDay,DSM.DealerType 
	FROM DCRM_SalesMeeting DSM(NOLOCK) 
	INNER JOIN OprUsers OU(NOLOCK) ON OU.Id = DSM.ActionTakenBy 
	INNER JOIN NCS_Dealers NCD (NOLOCK) ON NCD.ID = DSM.DealerId AND DSM.DealerType =2
	INNER JOIN Cities C ON C.ID = NCD.CityId
	WHERE 
	MONTH(DSM.MeetingDate) = @Month AND 
	YEAR(DSM.MeetingDate) = @Year	AND 
	(@UserId	IS NULL OR OU.Id = @UserId) AND
	(@CityId	IS NULL OR NCD.CityId = @CityId) AND
	(@StateId	IS NULL OR C.StateId = @StateId) AND
	(@DealerId	IS NULL OR NCD.ID = @DealerId)	AND
	(@MeetingMode IS NULL OR DSM.MeetingMode = @MeetingMode)
	GROUP BY DAY(DSM.MeetingDate),OU.UserName,OU.Id,DSM.DealerType 
	ORDER BY UserName
END



/****** Object:  StoredProcedure [dbo].[DCRM_ExecMeetingDetails]    Script Date: 07/18/2014 13:37:32 ******/
SET ANSI_NULLS ON
