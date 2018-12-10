IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[NewPQAlertDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[NewPQAlertDetails]
GO

	


-- Description	:	Get Detail Report of LeadId and CarBasic ID for Make on that particular date
-- Author		:	Dilip V. 06-Mar-2012
-- Modified		:	Dilip V. 29-Mar-2012
--				:   Amit Kumar 31st May 2012(Added IsNotInterested)
--				

CREATE PROCEDURE [CRM].[NewPQAlertDetails]	
	
	@Day			NUMERIC(2,0),
	@Month			NUMERIC(2,0),
	@Year			NUMERIC(4,0),
	@Type			SMALLINT,
	@DealerAssigned	SMALLINT,
	@Make			NUMERIC(18,0),
	@IsNotInterested NUMERIC(18,0)
	
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @varsql				VARCHAR(1000),
			@SingleQuotesTwice	VARCHAR(2) = ''''''
 BEGIN
   IF(@IsNotInterested!=0)
	  BEGIN	
		SET @varsql = 'SELECT CC.ID,CC.FirstName AS CustomerName, CC.Email, CC.Mobile, VM.Make AS Car, CONVERT(CHAR(14), CL.CreatedOn, 106) AS LeadDate, CONVERT(CHAR(14), AlertDate, 106) AS AlertDate, CBD.IsNotInterested'
		IF(@DealerAssigned = 1)
			SET @varsql += ' ,ND.Name Dealer'
		ELSE
			SET @varsql += ' ,'+@SingleQuotesTwice+' Dealer'
		SET @varsql += ' FROM CRM_PQAlerts CPA WITH (NOLOCK)'
		IF(@DealerAssigned = 1)
		BEGIN
			SET @varsql += ' INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CPA.CBDId
			INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.ID = CDA.DealerId'
		END
		SET @varsql += ' INNER JOIN vwMMV VM WITH (NOLOCK) ON CPA.VersionId = VM.VersionId
		INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CPA.LeadId
		INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CL.CNS_CustId = CC.ID
		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CPA.CBDId=CBD.ID
		WHERE '
		IF(@Type = 1)
		BEGIN
			SET @varsql += 'DAY(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Day, 101) + ' AND MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101)
		END
		ELSE IF (@Type = 2)
		BEGIN
			SET @varsql += 'DAY(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Day, 101) + ' AND MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101) + ' AND VM.MakeId = ' + CONVERT(CHAR, @Make, 101)
		END
		ELSE IF (@Type = 3)
		BEGIN
			SET @varsql += 'MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101)
		END
		ELSE IF (@Type = 4)
		BEGIN
			SET @varsql += 'MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101) + ' AND VM.MakeId = ' + CONVERT(CHAR, @Make, 101)
		END
		
		SET @varsql += ' ORDER BY AlertDate'
		
		PRINT (@varsql)
		EXEC (@varsql)
	  END	
   ELSE
	 BEGIN	
		SET @varsql = 'SELECT CC.ID,CC.FirstName AS CustomerName, CC.Email, CC.Mobile, VM.Make AS Car, CONVERT(CHAR(14), CL.CreatedOn, 106) AS LeadDate, CONVERT(CHAR(14), AlertDate, 106) AS AlertDate, CBD.IsNotInterested'
		IF(@DealerAssigned = 1)
			SET @varsql += ' ,ND.Name Dealer'
		ELSE
			SET @varsql += ' ,'+@SingleQuotesTwice+' Dealer'
		SET @varsql += ' FROM CRM_PQAlerts CPA WITH (NOLOCK)'
		IF(@DealerAssigned = 1)
		BEGIN
			SET @varsql += ' INNER JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CPA.CBDId
			INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.ID = CDA.DealerId'
		END
		SET @varsql += ' INNER JOIN vwMMV VM WITH (NOLOCK) ON CPA.VersionId = VM.VersionId
		INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CPA.LeadId
		INNER JOIN CRM_Customers CC WITH (NOLOCK) ON CL.CNS_CustId = CC.ID
		INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CPA.CBDId=CBD.ID
		WHERE CBD.IsNotInterested=0'
		IF(@Type = 1)
		BEGIN
			SET @varsql += 'AND DAY(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Day, 101) + ' AND MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101)
		END
		ELSE IF (@Type = 2)
		BEGIN
			SET @varsql += 'AND DAY(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Day, 101) + ' AND MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101) + ' AND VM.MakeId = ' + CONVERT(CHAR, @Make, 101)
		END
		ELSE IF (@Type = 3)
		BEGIN
			SET @varsql += 'AND MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101)
		END
		ELSE IF (@Type = 4)
		BEGIN
			SET @varsql += 'AND MONTH(CPA.AlertDate) = ' + CONVERT(CHAR(2), @Month, 101) + ' AND YEAR(CPA.AlertDate) = ' + CONVERT(CHAR(4), @Year, 101) + ' AND VM.MakeId = ' + CONVERT(CHAR, @Make, 101)
		END
		
		SET @varsql += ' ORDER BY AlertDate'
		
		PRINT (@varsql)
		EXEC (@varsql)	
	  END	
 END
	

END





