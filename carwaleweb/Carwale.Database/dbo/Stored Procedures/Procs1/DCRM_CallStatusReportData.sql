IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_CallStatusReportData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_CallStatusReportData]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(23rd May 2014)
-- Description	:	Get Call status report data for 
--					Service Back Office and Field executives
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_CallStatusReportData]--null,5
	
	@ExecutiveId	INT = NULL,
	@RoleId			SMALLINT,
	@RegionId		INT	=	NULL,
	@CityId			INT	=	NULL,
	@Organisation	VARCHAR(100) =	NULL,
	@IsDeleted		BIT = NULL,
	@Status			BIT =	NULL

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		D.ID AS DealerId,
		D.Organization AS Dealer,
		CASE ISNULL(D.Status,0) WHEN 'False' THEN 1 ELSE 0 END AS IsDealerActive ,
		CASE ISNULL(D.IsDealerDeleted,0) WHEN 'True' THEN 1 ELSE 0 END AS IsDealerDeleted ,
		S.Name+' / '+C.Name AS StateCity,
		DC1.Id AS CallId,
		CONVERT(VARCHAR(30),DC1.ScheduleDate)  AS NextCallDate ,
		(SELECT TOP 1 CONVERT(VARCHAR(30),DC2.CalledDate)
			FROM DCRM_Calls	DC2(NOLOCK) WHERE	DC2.DealerId = DAU.DealerId AND DC2.ActionTakenId = 1 order by CalledDate desc) AS LastCalledDate,
		(SELECT TOP 1 DATEDIFF(dd,DC2.CalledDate,getdate()) 
			FROM DCRM_Calls	DC2(NOLOCK) WHERE	DC2.DealerId = DAU.DealerId AND DC2.ActionTakenId = 1 order by CalledDate desc) AS NoOfDays,
		(SELECT TOP 1 OU.UserName  
			FROM DCRM_Calls	DC2(NOLOCK) INNER JOIN OprUsers OU(NOLOCK) ON OU.Id =DC2.UserId
			WHERE	DC2.DealerId = DAU.DealerId AND DC2.ActionTakenId = 1 order by CalledDate desc) AS LastCallBy
	FROM 
		DCRM_ADM_UserDealers	DAU(NOLOCK)
		INNER JOIN Dealers	D(NOLOCK)	ON DAU.DealerId = D.ID AND (@ExecutiveId IS NULL OR DAU.UserId = @ExecutiveId)
		LEFT JOIN DCRM_Calls	DC1(NOLOCK) ON	DC1.DealerId = DAU.DealerId AND DC1.ActionTakenId = 2 AND (@ExecutiveId IS NULL OR DC1.UserId = @ExecutiveId)
		INNER JOIN States	S(NOLOCK)	ON D.StateId = S.ID  
		INNER JOIN Cities	C(NOLOCK)	ON D.CityId = C.ID  
		INNER JOIN DCRM_ADM_RegionCities	DAR(NOLOCK)	ON	D.CityId = DAR.CityId  
		INNER JOIN DCRM_ADM_Regions			R(NOLOCK)	ON	DAR.RegionId = R.Id 
	WHERE 
		(	@RoleId IS NULL OR DAU.RoleId = @RoleId )
		AND		(	@RegionId IS NULL OR R.Id = @RegionId	) 
		AND		(	@CityId IS NULL OR D.CityId = @CityId	) 
		AND		(	@Status IS NULL OR D.Status = @Status	)
		AND		(	@Organisation IS NULL OR D.Organization LIKE @Organisation	)
		AND		(	@IsDeleted IS NULL OR D.IsDealerDeleted = @IsDeleted)

	ORDER BY DC1.ScheduleDate DESC
	
END

