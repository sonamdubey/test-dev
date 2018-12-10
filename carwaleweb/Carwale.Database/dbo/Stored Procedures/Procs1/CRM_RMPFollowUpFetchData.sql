IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_RMPFollowUpFetchData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_RMPFollowUpFetchData]
GO

	-- =============================================
--Name of SP/Function				: CarWale.[CRM].[RMPFollowUpFetchData]
--Applications using SP				: CRM
--Modules using the SP				: RMPanelFollowUp.cs
--Technical department				: Database
--Summary							: Reports details
--Author							: Amit Kumar 8-Sept-2012
--Modification history				: 1. 
-- =============================================
CREATE PROCEDURE [dbo].[CRM_RMPFollowUpFetchData]
	-- Add the parameters for the stored procedure here
@fromDate	DATETIME,
@toDate		DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT CC.FirstName, CC.Email, CC.Mobile, NR.Name AS RM, ND.Name AS Dealer, CF.Id,CF.CreatedOn, 
		ISNULL(CONVERT(VARCHAR(20),CF.NextCallDate,100),'') AS NextCallDate ,CF.LastCallDate,CF.LastComment, CF.CustId ,
		CASE WHEN CF.Eagerness = 1 THEN 'Very Hot' WHEN CF.Eagerness = 2 THEN 'Hot' WHEN CF.Eagerness = 3 THEN 'Normal' WHEN CF.Eagerness = 4 THEN 'Cold'
		WHEN CF.Eagerness = -1 THEN 'Unknown'  END AS Eagerness, CF.Comment,PS.Name AS ProductStatus 
	FROM CRM_DealerFollowUp CF WITH(NOLOCK), CRM_Customers CC WITH(NOLOCK), NCS_Dealers ND WITH(NOLOCK), NCS_RManagers AS NR WITH(NOLOCK),
		CRM_ProductStatus PS WITH(NOLOCK) 
	WHERE PanelType = 1 
		AND ((CF.CreatedOn BETWEEN @fromDate AND @toDate ) 
		OR (LastCallDate BETWEEN @fromDate AND @toDate)) 
		AND CF.CustId = CC.ID AND CF.UpdatedBy = NR.Id AND CF.DealerId = ND.ID
		AND PS.ID=CF.ProductStatus
END
