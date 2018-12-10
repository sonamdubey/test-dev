IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetDealerIdFromNodeCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetDealerIdFromNodeCode]
GO

	


--Summary							: Get DealerId Based on NodeCode and/or ExecId
--Impact							: SP : [CRM].[WeeklyReport_Event],
--Author							: Dilip V. 05-Nov-2012
--Modification						: 1. 
CREATE PROCEDURE [CRM].[GetDealerIdFromNodeCode]
	@NodeCode			NVARCHAR(4000),
	@ExecutiveId		TINYINT = NULL,
	@SubOrganisation	NUMERIC(18,0) = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE	@varsql				VARCHAR(2000),
			@SingleQuotes		VARCHAR(1) = ''''
	
	
		SET @varsql =  'SELECT DISTINCT NRD.DealerId
		FROM NCS_DealerOrganization NDO WITH(NOLOCK) 
		INNER JOIN NCS_SubDealerOrganization NSDO WITH(NOLOCK) ON NDO.ID = NSDO.OId 
		INNER JOIN NCS_RMDealers NRD WITH(NOLOCK) ON NSDO.DId = NRD.DealerId 
		INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON NRD.DealerId = ND.ID 
		INNER JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id 
		WHERE NRM.NodeCode LIKE ' +@SingleQuotes+ @NodeCode +@SingleQuotes+'
		AND NDO.IsCWExecutive = 0 
		AND NDO.IsActive = 1 
		AND NRM.IsActive = 1'
		IF(@ExecutiveId IS NOT NULL)
		BEGIN
			SET @varsql += ' AND NRD.IsExecutive = ' + CONVERT(CHAR(10), @ExecutiveId)
		END
		IF(@SubOrganisation IS NOT NULL)
		BEGIN
			SET @varsql += ' AND NSDO.OId = ' + CONVERT(CHAR(10), @SubOrganisation)
		END
		
		PRINT (@varsql)
		EXEC (@varsql)
END


