IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[TataGreenNumbersFetchData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[TataGreenNumbersFetchData]
GO

	

--Summary	: Fetch TataGreen Numbers
--Author	: Dilip V. 13-Aug-2012
--Modifier	: Dilip V. 21-Aug-2012 (Added Date)

CREATE PROCEDURE [CRM].[TataGreenNumbersFetchData]
@Month	TINYINT,
@Year	SMALLINT
AS 
BEGIN
	SET NOCOUNT ON
	SELECT CBD.ID,CC.FirstName +' '+ CC.LastName CName, VW.Make + ' ' + VW.Model +' '+ VW.Version Car, CDA.CreatedOn,ND.Name DName, TGN.Number
	FROM vwMMV VW
		INNER JOIN CRM_CarBasicData CBD ON CBD.VersionId = VW.VersionId 
		INNER JOIN CRM_CarDealerAssignment CDA ON CDA.CBDId = CBD.Id
		INNER JOIN NCS_Dealers ND ON CDA.DealerId = ND.ID
		INNER JOIN CRM_Leads CL ON CL.ID = CBD.LeadId
		INNER JOIN CRM_Customers CC ON CC.ID = CL.CNS_CustId
		LEFT JOIN CRM.TataGreenNumbers TGN ON CBD.ID = TGN.CBDId
	WHERE 
	VW.ModelId IN (229,463) AND
	MONTH(CDA.CreatedOn) = @Month
	AND YEAR(CDA.CreatedOn) = @Year
	ORDER BY TGN.Number,CDA.CreatedOn

END

